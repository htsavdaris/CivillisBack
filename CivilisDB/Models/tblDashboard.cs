using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace CivilisDB.Models
{

    [Table("tbldashboard")]
    public partial class tblDashboard : BaseEntity
    {
        [Column, Key]
        public long fldpersonid { get; set; }
        public long fldtotalcases { get; set; }
        public long fldtotalopen { get; set; }
        public long fldtotalclosed { get; set; }
        public long fldtotalsuccess { get; set; }
        public long fldtotalfail { get; set; }
        public long fldmycases { get; set; }
        public long fldmycasesopen { get; set; }
        public long fldmycasesclosed { get; set; }
        public long fldmycasessuccess { get; set; }
        public long fldmycasesfail { get; set; }
        public long fldtotalopenlm { get; set; }
        public long fldtotalclosedlm { get; set; }
        public long fldtotalsuccesslm { get; set; }
        public long fldtotalfaillm { get; set; }
        public long fldmycaseslm { get; set; }
        public long fldmycasesopenlm { get; set; }
        public long fldmycasesclosedlm { get; set; }
        public long fldmycasessuccesslm { get; set; }
        public long fldmycasesfaillm { get; set; }

    }

    public class tblDashboardDac : DapperBaseRepository<tblDashboard>
    {
        private const string tableName = "tbldashboard";
        private const string pkeyName = "fldpersonid";

        public tblDashboardDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblDashboardDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        public int GenerateDash(long fldpersonid)
        {
            try
            {
                var sql = "CALL \"public.generatedashboardforuser\"(@personid)";
                var p = new DynamicParameters();
                p.Add("@personid", fldpersonid);
                var res = Connection.Execute(sql, p);
                return res;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GenerateDash: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return -1;
            }

        }

        ~tblDashboardDac()
        {
            if (Connection != null)
                Connection.Close();
        }


    }

}
