using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace CivilisDB
{
    public static class ConnectionFactory
    {
        
        public static IDbConnection createConnection(string ConnectionString)
        {
            try
            {
                NpgsqlConnection DataConnection = new NpgsqlConnection(ConnectionString);
                return DataConnection;
            }
            catch (NpgsqlException ex)
            {
                return null;
            }
        }


        public static IDbConnection createConnection()
        {
            try
            {
                NpgsqlConnection DataConnection = new NpgsqlConnection();
                return DataConnection;
            }
            catch (NpgsqlException ex)
            {
                return null;
            }
        }
    }
}
