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
            return configuration.GetConnectionString("DBConnection")
                ?? throw new InvalidOperationException("Connection string 'DBConnection' is not configured.");
        }

    }
}
