using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace CivilisDB.Models
{
    [Table("tblrole")]
    public partial class tblRole :BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldroleid { get; set; }

        public string fldroledesc { get; set; } = "";

    }


    public class tblRoleDac : DapperBaseRepository<tblRole>
    {
        private const string tableName = "tblrole";
        private const string pkeyName = "fldroleid";

        public tblRoleDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblRoleDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        ~tblRoleDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}
