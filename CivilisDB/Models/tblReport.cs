using Dapper;
using Dapper.Database;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Formats.Asn1;

namespace CivilisDB.Models
{
    [Table("tblreport")]
    public partial class tblReport :BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldreportid { get; set; } = 0;

        public string fldreportname { get; set; } = "";

        public long? flduserid { get; set; } = 0;

        public DateTime? fldreportdate { get; set; } = DateTime.Now;

        public string fldfilename { get; set; } = "";

        public string fldbloburl { get; set; } = "";

    }

    public class tblReportDac : DapperBaseRepository<tblReport>
    {
        private const string tableName = "tblreport";
        private const string pkeyName = "fldreportid";

        public tblReportDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblReportDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        public List<tblReport> GetByFlduserId(long flduserid)
        {
            try
            {
                var obj = Connection.Query<tblReport>(SqlSelectCommand + " WHERE flduserid = @flduserid ORDER BY fldcreatedat DESC ", new { flduserid = flduserid }).AsList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByFlduserId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        public List<tblReport> GetReportsOlderThanId(long id)
        {
            try
            {
                var obj = Connection.Query<tblReport>(SqlSelectCommand + " where fldreportid <= @fldreportid", new { fldreportid = id }).AsList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetLatestByFlduserId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        public tblReport GetLatestReport()
        {
            try
            {
                var obj = Connection.QueryFirst<tblReport>(SqlSelectCommand + " ORDER BY fldcreatedat DESC LIMIT 1");
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetLatestByFlduserId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }



        ~tblReportDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}
