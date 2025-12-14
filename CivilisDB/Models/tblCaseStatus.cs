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
     

    [Table("tblcasestatus")]
    public partial class tblCaseStatus : BaseEntity
    {

        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldcasestatusid { get; set; }

        public string fldcasestatusdesc { get; set; } = "";
    }


    public class tblCaseStatusDac : DapperBaseRepository<tblCaseStatus>
    {
        private const string tableName = "tblcasestatus";
        private const string pkeyName = "fldcasestatusid";

        public tblCaseStatusDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblCaseStatusDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        ~tblCaseStatusDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}
