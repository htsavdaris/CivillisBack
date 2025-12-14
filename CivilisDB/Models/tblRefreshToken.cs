using Dapper;
using Dapper.Database;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security.Cryptography;
using static Dapper.SqlMapper;

namespace CivilisDB.Models
{
    [Table("tblrefreshtoken")]
    public partial class tblRefreshToken : BaseEntity
    {
        [Column, Key]
        public long flduserid { get; set; }
        public Guid fldrefreshtoken { get; set; } 
        public DateTime? fldissued { get; set; }
        public DateTime? fldexpires { get; set; }

        public tblRefreshToken()
        {
            this.flduserid = 0;
            this.fldrefreshtoken = new Guid();
            this.fldissued = DateTime.Now;
            this.fldexpires = DateTime.Now + new TimeSpan(8, 0, 0);
        }

        public tblRefreshToken(long userid) {
            this.flduserid = userid;
            this.fldrefreshtoken = Guid.NewGuid();
            this.fldissued = DateTime.Now;
            this.fldexpires = DateTime.Now + new TimeSpan(8, 0, 0);
        }

        //private static string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[64];
        //    using var rng = RandomNumberGenerator.Create();
        //    rng.GetBytes(randomNumber);
        //    return Convert.ToBase64String(randomNumber);
        //}

    }

    public class tblRefreshTokenDac : DapperBaseRepository<tblRefreshToken>
    {

        private const string tableName = "tblrefreshtoken";
        private const string pkeyName = "flduserid";

        public tblRefreshTokenDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblRefreshTokenDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {
        }

        public tblRefreshToken? GetbyRefreshToken(Guid? token)
        {
            _logger.LogTrace(1, "GetbyRefreshToken in  called");
            try
            {
                if (token == null || token == Guid.Empty)
                {
                    return null;
                }   
                var obj = Connection.QueryFirstOrDefault<tblRefreshToken>(SqlSelectCommand + " WHERE fldrefreshtoken = @token  ", new { token = token.Value });
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(tblRefreshToken).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        ~tblRefreshTokenDac()
        {
            if (Connection != null)
                Connection.Close();
        }

    }
}
