using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CivilisApp.ApiTests;

public class ApiSmokeTests
{
    [Fact]
    public async Task SettingsEndpoint_ReturnsVersion()
    {
        using var factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureAppConfiguration((_, configuration) => configuration.AddInMemoryCollection(
                new Dictionary<string, string?>
                {
                    ["JwtOptions:Issuer"] = "https://test.invalid",
                    ["JwtOptions:Audience"] = "https://test.invalid",
                    ["JwtOptions:SigningKey"] = "test-signing-key",
                    ["JwtOptions:ExpirationSeconds"] = "3600",
                    ["JwtOptions:Authority"] = "https://test.invalid"
                }));
        });

        using var client = factory.CreateClient();
        var response = await client.GetAsync("/api/Settings");

        response.EnsureSuccessStatusCode();
        Assert.Contains("Version 0.92", await response.Content.ReadAsStringAsync());
    }
}
