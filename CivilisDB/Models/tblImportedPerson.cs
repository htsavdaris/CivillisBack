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
    [Table("tblimportedperson")]
    public partial class tblImportedPerson :BaseEntity
    {
        [Column, Key]
        public long fldpersonid { get; set; }
        public string fldpersonlastname { get; set; }
        public string fldpersonfirstname { get; set; }
        public string fldpersongender { get; set; }
        public string fldpersonprefix { get; set; }
        public string fldpersonstreet1 { get; set; }
        public string fldpersonnumber1 { get; set; }
        public string fldpersontk1 { get; set; }
        public string fldpersonmobile { get; set; }
        public string fldpersonemail { get; set; }
        public string fldcity { get; set; }
        public string fldprofession { get; set; }
        public string flddatasource { get; set; }
       
    }

    public class tblImportedPersonDac : DapperBaseRepository<tblImportedPerson>
    {
        private const string tableName = "tblimportedperson";
        private const string pkeyName = "fldpersonid";

        public tblImportedPersonDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblImportedPersonDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        ~tblImportedPersonDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}
