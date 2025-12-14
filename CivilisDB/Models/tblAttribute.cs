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
    [Table("tblattribute")]
    public partial class tblAttribute : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldattributeid { get; set; }

        public string fldattributedescription { get; set; } = "";
    }

    public class tblAttributeDac : DapperBaseRepository<tblAttribute>
    {
        private const string tableName = "tblattribute";
        private const string pkeyName = "fldattributeid";

        public tblAttributeDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblAttributeDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        ~tblAttributeDac()
        {
            if (Connection != null)
                Connection.Close();
        }


    }
}
