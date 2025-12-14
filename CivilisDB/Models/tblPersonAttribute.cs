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
    [Table("tblpersonattribute")]
    public partial class tblPersonAttribute : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldpersonattrid { get; set; }

        public long fldpersonid { get; set; }

        public long fldattributeid { get; set; }


    }


    public class tblPersonAttributeDac : DapperBaseRepository<tblPersonAttribute>
    {
        private const string tableName = "tblpersonattribute";
        private const string pkeyName = "fldpersonattrid";

        public tblPersonAttributeDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblPersonAttributeDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        ~tblPersonAttributeDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }

}
