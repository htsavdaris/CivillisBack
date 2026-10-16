using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using CivilisDB.Models;
using Npgsql;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace CivilisApp.ApiTests;

[Collection(ApiDatabaseCollection.Name)]
public sealed class SpecializedControllerApiTests
{
    private const string TestPassword = "ExistingTestPassword!42";
    private readonly PostgreSqlApiFixture _fixture;
    private HttpClient Client => _fixture.Client;

    public SpecializedControllerApiTests(PostgreSqlApiFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task CaseViewController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/CaseView"));
        await AssertOkAsync(await Client.GetAsync("/api/CaseView/1"));
        await AssertOkAsync(await Client.GetAsync("/api/CaseView/status/0"));
        await AssertOkAsync(await Client.GetAsync("/api/CaseView/manager/1"));
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/CaseView/search2", new
        {
            pAllOrMine = 1,
            pOpen = true,
            pClosed = true
        }));
    }

    [Fact]
    public async Task DashboardController_ExercisesEveryAction()
    {
        foreach (var route in new[]
        {
            "/api/Dashboard/generatedash/1",
            "/api/Dashboard/getdashforperson/1",
            "/api/Dashboard/opencases",
            "/api/Dashboard/cases",
            "/api/Dashboard/pendinginvitations",
            "/api/Dashboard/invitations",
            "/api/Dashboard/futureinvitations"
        })
        {
            await AssertOkAsync(await Client.GetAsync(route));
        }
    }

    [Fact]
    public async Task ImportPersonController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/ImportPerson"));
        await AssertOkAsync(await Client.GetAsync("/api/ImportPerson/5"));
        using (var multipart = new MultipartFormDataContent())
        {
            multipart.Add(new ByteArrayContent([1, 2, 3]), "file", "people.csv");
            var response = await Client.PostAsync("/api/ImportPerson", multipart);
            using (response)
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal("File Upload Successful", await response.Content.ReadAsStringAsync());
            }
        }
        Assert.Equal(HttpStatusCode.NoContent, (await Client.PutAsJsonAsync("/api/ImportPerson/5", "unused")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync("/api/ImportPerson/5")).StatusCode);
    }

    [Fact]
    public async Task UserController_ExercisesEveryActionAndAuthorization()
    {
        var anonymousClient = _fixture.CreateAnonymousClient();
        using (anonymousClient)
        {
            Assert.Equal(HttpStatusCode.Unauthorized, (await anonymousClient.GetAsync("/api/User")).StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, (await anonymousClient.GetAsync("/api/User/1")).StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, (await anonymousClient.GetAsync("/api/User/login/test")).StatusCode);
            Assert.Equal(HttpStatusCode.Forbidden, (await anonymousClient.PostAsJsonAsync("/api/User/changepasswordbyadmin", new { flduserid = 1, newpassword = "x" })).StatusCode);
        }

        await AssertOkAsync(await Client.GetAsync("/api/User"));
        await AssertOkAsync(await Client.GetAsync("/api/User/1"));
        await AssertOkAsync(await Client.GetAsync("/api/User/login/testadmin"));
        Assert.Equal(HttpStatusCode.NoContent, (await Client.PostAsJsonAsync("/api/User", "unused")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await Client.PutAsJsonAsync("/api/User/1", "unused")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync("/api/User/1")).StatusCode);

        var authenticationFailure = await Client.PostAsJsonAsync("/api/User/authenticate", new { login = "missing", password = "incorrect", rememberMe = false });
        using (authenticationFailure)
        {
            Assert.Equal(HttpStatusCode.Forbidden, authenticationFailure.StatusCode);
        }

        var badRefresh = await Client.PostAsJsonAsync("/api/User/reenter", new { accesstoken = "", refreshtoken = Guid.NewGuid() });
        using (badRefresh)
        {
            Assert.Equal(HttpStatusCode.BadRequest, badRefresh.StatusCode);
        }
        var invalidRefresh = await Client.PostAsJsonAsync("/api/User/refresh", new { accesstoken = "", refreshtoken = Guid.NewGuid() });
        using (invalidRefresh)
        {
            Assert.Equal(HttpStatusCode.BadRequest, invalidRefresh.StatusCode);
        }

        var userId = await CreateTestUserAsync();
        var authentication = await Client.PostAsJsonAsync("/api/User/authenticate", new { login = "apiuser", password = TestPassword, rememberMe = false });
        using (authentication)
        {
            Assert.Equal(HttpStatusCode.OK, authentication.StatusCode);
            using var body = await System.Text.Json.JsonDocument.ParseAsync(await authentication.Content.ReadAsStreamAsync());
            var refreshToken = body.RootElement.GetProperty("refreshtoken").GetGuid();
            Assert.NotEqual(Guid.Empty, refreshToken);

            foreach (var route in new[] { "reenter", "refresh" })
            {
                var refreshResponse = await Client.PostAsJsonAsync($"/api/User/{route}", new { accesstoken = "expired-token", refreshtoken = refreshToken });
                using (refreshResponse)
                {
                    Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
                }
            }
        }

        await AssertOkAsync(await Client.PostAsJsonAsync("/api/User/changepassword", new { login = "apiuser", oldpassword = TestPassword, newpassword = TestPassword + "-next" }));
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/User/changepasswordbyadmin", new { flduserid = userId, newpassword = TestPassword + "-admin" }));
    }

    [Fact]
    public async Task ReportController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/Report"));
        Assert.Equal("value", await Client.GetStringAsync("/api/Report/1"));
        Assert.Equal(HttpStatusCode.NoContent, (await Client.PostAsJsonAsync("/api/Report", "unused")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await Client.PutAsJsonAsync("/api/Report/1", "unused")).StatusCode);
        Assert.Equal(HttpStatusCode.NoContent, (await Client.DeleteAsync("/api/Report/1")).StatusCode);

        await AssertOkAsync(await Client.GetAsync("/api/Report/personReport"));
        await AssertOkAsync(await Client.GetAsync("/api/Report/entityReport"));
        var reportId = await ExecuteScalarAsync<long>("SELECT MAX(fldreportid) FROM tblreport");
        var download = await Client.GetAsync($"/api/Report/personReportDownload/{reportId}");
        using (download)
        {
            Assert.Equal(HttpStatusCode.OK, download.StatusCode);
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", download.Content.Headers.ContentType?.MediaType);
            Assert.True((await download.Content.ReadAsByteArrayAsync()).Length > 0);
        }
    }

    private async Task<long> CreateTestUserAsync()
    {
        var login = "apiuser";
        var hash = CivilisDB.PasswordHasher.ComputeHash(TestPassword, login, "ThisIsaNic3P3pp3r");
        await using var connection = await _fixture.OpenDatabaseConnectionAsync();
        await using var command = new NpgsqlCommand(
            "INSERT INTO tbluser (fldlogin, fldpassword, fldfirstname, fldlastname, fldpersonid) VALUES (@login, @hash, 'API', 'Test', 1) RETURNING flduserid",
            connection);
        command.Parameters.AddWithValue("login", login);
        command.Parameters.AddWithValue("hash", hash);
        return Convert.ToInt64(await command.ExecuteScalarAsync());
    }

    private async Task<T> ExecuteScalarAsync<T>(string sql)
    {
        await using var connection = await _fixture.OpenDatabaseConnectionAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        var value = await command.ExecuteScalarAsync();
        Assert.NotNull(value);
        return (T)Convert.ChangeType(value, typeof(T));
    }

    private static async Task AssertOkAsync(HttpResponseMessage response)
    {
        using (response)
        {
            var body = await response.Content.ReadAsStringAsync();
            Assert.True(response.StatusCode == HttpStatusCode.OK,
                $"{response.RequestMessage?.Method} {response.RequestMessage?.RequestUri} returned {(int)response.StatusCode}: {body}");
        }
    }
}
