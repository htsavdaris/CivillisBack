using Xunit;

namespace CivilisApp.ApiTests;

[Collection(ApiDatabaseCollection.Name)]
public class ApiSmokeTests
{
    private readonly PostgreSqlApiFixture _fixture;

    public ApiSmokeTests(PostgreSqlApiFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task SettingsEndpoint_ReturnsVersion()
    {
        var response = await _fixture.Client.GetAsync("/api/Settings");

        response.EnsureSuccessStatusCode();
        Assert.Contains("Version 0.92", await response.Content.ReadAsStringAsync());
    }
}
