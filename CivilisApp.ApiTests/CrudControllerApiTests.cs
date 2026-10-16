using System.Net;
using System.Net.Http.Json;
using Npgsql;
using Xunit;

namespace CivilisApp.ApiTests;

[Collection(ApiDatabaseCollection.Name)]
public sealed class CrudControllerApiTests
{
    private readonly PostgreSqlApiFixture _fixture;
    private HttpClient Client => _fixture.Client;

    public CrudControllerApiTests(PostgreSqlApiFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task AttributeController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/Attribute"));
        await AssertOkAsync(await Client.GetAsync("/api/Attribute/1"));

        var marker = Marker();
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Attribute", new { fldattributedescription = marker }));
        var id = await FindIdAsync("SELECT fldattributeid FROM tblattribute WHERE fldattributedescription = @value", marker);
        await AssertOkAsync(await Client.GetAsync($"/api/Attribute/{id}"));
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/Attribute/{id}", new { fldattributeid = id, fldattributedescription = marker + "-updated" }));
        await AssertOkAsync(await Client.DeleteAsync($"/api/Attribute/{id}"));
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/Attribute/{id}")).StatusCode);
    }

    [Fact]
    public async Task CaseController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/Case"));
        await AssertOkAsync(await Client.GetAsync("/api/Case/1"));
        await AssertOkAsync(await Client.GetAsync("/api/Case/status/0"));

        var marker = Marker();
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Case", CasePayload(0, marker)));
        var id = await FindIdAsync("SELECT fldcaseid FROM tblcase WHERE fldcasetitle = @value", marker);
        await AssertOkAsync(await Client.GetAsync($"/api/Case/{id}"));
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/Case/{id}", CasePayload(id, marker + "-updated")));
        await AssertOkAsync(await Client.DeleteAsync($"/api/Case/{id}"));
    }

    [Fact]
    public async Task CaseRowController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/CaseRow"));
        await AssertOkAsync(await Client.GetAsync("/api/CaseRow/1"));
        await AssertOkAsync(await Client.GetAsync("/api/CaseRow/withCaseid/1"));

        var marker = Marker();
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/CaseRow", new { fldcaseid = 1, fldactiondesc = marker }));
        var id = await FindIdAsync("SELECT fldcaserowid FROM tblcaserow WHERE fldactiondesc = @value", marker);
        await AssertOkAsync(await Client.GetAsync($"/api/CaseRow/{id}"));
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/CaseRow/{id}", new { fldcaserowid = id, fldcaseid = 1, fldactiondesc = marker + "-updated" }));
        await AssertOkAsync(await Client.DeleteAsync($"/api/CaseRow/{id}"));
    }

    [Fact]
    public async Task CaseTypeController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/CaseType"));
        await AssertOkAsync(await Client.GetAsync("/api/CaseType/1"));

        var marker = Marker();
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/CaseType", new { fldcasetypedesc = marker }));
        var id = await FindIdAsync("SELECT fldcasetypeid FROM tblcasetype WHERE fldcasetypedesc = @value", marker);
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/CaseType/{id}", new { fldcasetypeid = id, fldcasetypedesc = marker + "-updated" }));
        await AssertOkAsync(await Client.DeleteAsync($"/api/CaseType/{id}"));
    }

    [Fact]
    public async Task EntityController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/Entity"));
        await AssertOkAsync(await Client.GetAsync("/api/Entity/1"));

        var marker = Marker();
        var entity = EntityPayload(0, marker);
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Entity/search1", new
        {
            fldentityname = marker,
            fldmobile = "",
            fldcity = "",
            fldentitytypeid = 0
        }));
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Entity", entity));
        var id = await FindIdAsync("SELECT fldentityid FROM tblentity WHERE fldentityname = @value", marker);
        await AssertOkAsync(await Client.GetAsync($"/api/Entity/getByName/{marker}"));
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/Entity/{id}", EntityPayload(id, marker + "-updated")));
        await AssertOkAsync(await Client.DeleteAsync($"/api/Entity/{id}"));
    }

    [Fact]
    public async Task EntityTypeController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/EntityType"));
        await AssertOkAsync(await Client.GetAsync("/api/EntityType/1"));

        var marker = Marker();
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/EntityType", new { fldentitytypedesc = marker }));
        var id = await FindIdAsync("SELECT fldentitytypeid FROM tblentitytype WHERE fldentitytypedesc = @value", marker);
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/EntityType/{id}", new { fldentitytypeid = id, fldentitytypedesc = marker + "-updated" }));
        await AssertOkAsync(await Client.DeleteAsync($"/api/EntityType/{id}"));
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/EntityType/{id}")).StatusCode);
    }

    [Fact]
    public async Task PersonController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/Person"));
        await AssertOkAsync(await Client.GetAsync("/api/Person/1"));

        var marker = Marker();
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Person", PersonPayload(0, marker)));
        var id = await FindIdAsync("SELECT fldpersonid FROM tblperson WHERE fldpersonfirstname = @value", marker);
        await AssertOkAsync(await Client.GetAsync($"/api/Person/{id}"));
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Person/search1", PersonSearch(marker)));
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Person/search2", PersonSearch(marker)));
        await AssertOkAsync(await Client.PostAsync("/api/Person/search", content: null));
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/Person/{id}", PersonPayload(id, marker + "-updated")));
        await AssertOkAsync(await Client.DeleteAsync($"/api/Person/{id}"));
    }

    [Fact]
    public async Task PersonAttributeController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/PersonAttribute"));
        await AssertOkAsync(await Client.GetAsync("/api/PersonAttribute/1"));

        var description = Marker();
        var attributeId = await ExecuteScalarAsync<long>(
            "INSERT INTO tblattribute (fldattributedescription) VALUES (@value) RETURNING fldattributeid",
            new NpgsqlParameter("value", description));
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/PersonAttribute", new { fldpersonid = 1, fldattributeid = attributeId }));
        var id = await ExecuteScalarAsync<long>(
            "SELECT fldpersonattrid FROM tblpersonattribute WHERE fldpersonid = 1 AND fldattributeid = @value",
            new NpgsqlParameter("value", attributeId));
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/PersonAttribute/{id}", new { fldpersonattrid = id, fldpersonid = 1, fldattributeid = attributeId }));
        await AssertOkAsync(await Client.DeleteAsync($"/api/PersonAttribute/{id}"));
    }

    [Fact]
    public async Task InvitationController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/Invitation"));
        await AssertOkAsync(await Client.GetAsync("/api/Invitation/1"));

        var marker = Marker();
        await AssertOkAsync(await Client.PostAsJsonAsync("/api/Invitation", InvitationPayload(0, marker)));
        var id = await FindIdAsync("SELECT fldinvitationid FROM tblinvitation WHERE fldtitle = @value", marker);
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/Invitation/{id}", InvitationPayload(id, marker + "-updated")));
        await AssertOkAsync(await Client.DeleteAsync($"/api/Invitation/{id}"));
    }

    [Fact]
    public async Task UserRoleController_ExercisesEveryAction()
    {
        await AssertOkAsync(await Client.GetAsync("/api/UserRole"));
        await AssertOkAsync(await Client.GetAsync("/api/UserRole/1"));

        await AssertOkAsync(await Client.PostAsJsonAsync("/api/UserRole", new { flduserid = 1, fldroleid = 2 }));
        var id = await ExecuteScalarAsync<long>(
            "SELECT flduserroleid FROM tbluserrole WHERE flduserid = 1 AND fldroleid = 2 ORDER BY flduserroleid DESC LIMIT 1");
        await AssertOkAsync(await Client.PutAsJsonAsync($"/api/UserRole/{id}", new { flduserroleid = id, flduserid = 1, fldroleid = 2 }));
        await AssertOkAsync(await Client.DeleteAsync($"/api/UserRole/{id}"));
        Assert.Equal(HttpStatusCode.NotFound, (await Client.GetAsync($"/api/UserRole/{id}")).StatusCode);
    }

    [Fact]
    public async Task LookupControllers_ExerciseReadActions()
    {
        foreach (var route in new[]
        {
            "/api/CaseStatus",
            "/api/Role",
            "/api/Role/1",
            "/api/Role/user/1",
            "/api/EklogikiDomi",
            "/api/EklogikiDomi/001",
            "/api/EklogikiDomi/type/1",
            "/api/EntityView",
            "/api/EntityView/1",
            "/api/InvitationView",
            "/api/InvitationView/1",
            "/api/PersonAttributeView/personid/1",
            "/api/PersonAttributeView/attrid/1",
            "/api/UserRoleView",
            "/api/UserRoleView/1",
            "/api/UserRoleView/user/1"
        })
        {
            await AssertOkAsync(await Client.GetAsync(route));
        }
    }

    private async Task<long> FindIdAsync(string sql, string value) =>
        await ExecuteScalarAsync<long>(sql, new NpgsqlParameter("value", value));

    private async Task<T> ExecuteScalarAsync<T>(string sql, params NpgsqlParameter[] parameters)
    {
        await using var connection = await _fixture.OpenDatabaseConnectionAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);
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

    private static string Marker() => $"api-test-{Guid.NewGuid():N}";

    private static object CasePayload(long id, string title) => new
    {
        fldcaseid = id,
        fldcasetitle = title,
        fldcasetypeid = 1,
        fldcasedescription = "API integration case",
        fldcaseopendate = DateTime.Now,
        fldcasestatusid = 0,
        fldcomments = "",
        fldintermediateid = 1,
        fldintermediaterdetails = "",
        fldbeneficiaryid = 1,
        fldbeneficiarydetails = "",
        fldmanagerid = 1,
        fldconfidential = false,
        fldentityid = 1,
        fldentitycontactid = 1,
        fldreceptionid = 1
    };

    private static object EntityPayload(long id, string name) => new
    {
        fldentityid = id,
        fldentityname = name,
        fldentitytypeid = 1,
        fldaddress = "Test Street",
        fldnumber = "1",
        fldtk = "10000",
        fldcity = "Athens",
        fldcountry = "Greece",
        fldfixedphone = "2100000000",
        fldmobile = "6900000000",
        fldmail = $"{name}@example.invalid"
    };

    private static object PersonPayload(long id, string name) => new
    {
        fldpersonid = id,
        fldpersonlastname = name,
        fldpersonfirstname = name,
        fldpersongender = "",
        fldpersonprefix = "",
        fldpersonstreet1 = "Test Street",
        fldpersonnumber1 = "1",
        fldpersontk1 = "10000",
        fldpersonmobile = "6900000000",
        fldpersonemail = $"{name}@example.invalid",
        fldprofession = "Tester",
        fldxorikidomiid = "",
        flddatasource = "API test",
        fldcomment = "",
        fldcity = "Athens",
        fldofficevisit = false,
        fldcommunucationconsent = false,
        fldmobilizationconsent = false
    };

    private static object PersonSearch(string name) => new
    {
        fldpersonfirstname = name,
        fldpersonlastname = name,
        fldpersonmobile = "",
        fldcity = "",
        fldattributeid = 0
    };

    private static object InvitationPayload(long id, string title) => new
    {
        fldinvitationid = id,
        fldtitle = title,
        fldlocation = "Test location",
        flddatetime = DateTime.Now,
        fldhost = "API test",
        fldentityid = 1,
        fldhostpersonid = 1,
        fldgopersonid = 1,
        fldreplied = 0,
        fldcomments = "API integration test"
    };
}
