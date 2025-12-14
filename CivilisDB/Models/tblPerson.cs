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
    [Table("tblperson")]
    public partial class tblPerson :BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldpersonid { get; set; }
        public string fldpersonlastname { get; set; } = "";
        public string fldpersonfirstname { get; set; } = "";
        public string fldpersongender { get; set; } = "";
        public string fldpersonprefix { get; set; } = "";
        public string fldpersonstreet1 { get; set; } = "";
        public string fldpersonnumber1 { get; set; } = "";
        public string fldpersontk1 { get; set; } = "";
        public string fldpersonmobile { get; set; } = "";
        public string fldpersonemail { get; set; } = "";
        public string fldprofession { get; set; } = "";
        public string fldxorikidomiid { get; set; } = "";
        public string flddatasource { get; set; } = "";
        public string fldcomment { get; set; } = "";

        public string fldcity { get; set; } = "";

        public bool? fldofficevisit { get; set; } = false;

        public bool? fldcommunucationconsent { get; set; } = false;
        public bool? fldmobilizationconsent { get; set; } = false;

    }

    public class tblPersonDac : DapperBaseRepository<tblPerson>
    {

        private const string tableName = "tblperson";
        private const string pkeyName = "fldpersonid";

        public tblPersonDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblPersonDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        
        ~tblPersonDac()
        {
            if (Connection != null)
                Connection.Close();
        }

    }

}