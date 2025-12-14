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
    [Table("tblprofession")]
    public partial class tblProfession : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldprofessionid { get; set; }

        public string fldprofessionname { get; set; } = "";

    }

    public class tblProfessionDac : DapperBaseRepository<tblProfession>
    {
        private const string tableName = "tblprofession";
        private const string pkeyName = "fldprofessionid";

        public tblProfessionDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblProfessionDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        ~tblProfessionDac()
        {
            if (Connection != null)
                Connection.Close();
        }


    }
}
