using Microsoft.Extensions.Configuration;
using System;

namespace CivilisApp

{
    public class DbConnectionStringResolver
    {

        private readonly IConfiguration configuration;

        public DbConnectionStringResolver(IConfiguration config)
        {
            this.configuration = config;

        }

        public string GetNameOrConnectionString()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var use_docker = Environment.GetEnvironmentVariable("USE_DOCKER");            
            switch (environment)
            {
                case "Development":
                    {
                        if (use_docker == "False")
                        {
                            return configuration["ConnectionStrings:DBConnection"]; 
                        }
                        else
                        {
                            return configuration["ConnectionStrings:DBConnection"]; 
                        }
                    }                                                                                                                     
                case "Production":
                    //string connstring = Environment.GetEnvironmentVariable("proddbconnectionstring");
                    string connstring = configuration["ConnectionStrings:DBConnection"];
                    return connstring;
                default:
                    return configuration.GetConnectionString("ConnectionStrings:DBConnection");
            }
        }

    }
}
