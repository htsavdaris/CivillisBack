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

    [Table("tbluserrole")]
    public partial class tblUserRole : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long flduserroleid { get; set; }

        public long flduserid { get; set; }

        public long fldroleid { get; set; }

    }

    public class tblUserRoleDac : DapperBaseRepository<tblUserRole>
    {
        private const string tableName = "tbluserrole";
        private const string pkeyName = "flduserroleid";

        public tblUserRoleDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblUserRoleDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }



        ~tblUserRoleDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}
