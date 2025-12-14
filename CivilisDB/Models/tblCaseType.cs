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
    [Table("tblcasetype")]
    public partial class tblCaseType : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldcasetypeid { get; set; }

        public string fldcasetypedesc { get; set; } = "";

    }


    public class tblCaseTypeDac : DapperBaseRepository<tblCaseType>
    {
        private const string tableName = "tblcasetype";
        private const string pkeyName = "fldcasetypeid";

        public tblCaseTypeDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblCaseTypeDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        ~tblCaseTypeDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}
