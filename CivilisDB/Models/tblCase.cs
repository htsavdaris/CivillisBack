using Dapper;
using Dapper.Database;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;


namespace CivilisDB.Models
{

    [Table("tblcase")]
    public partial class tblCase : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldcaseid { get; set; }

        public string fldcasetitle { get; set; } = "";

        public long? fldcasetypeid { get; set; } = 0;

        public string fldcasedescription { get; set; } = "";

        public DateTime? fldcaseopendate { get; set; } = DateTime.Now;

        public long? fldcasestatusid { get; set; }

        public string fldcomments { get; set; } = "";

        public long? fldintermediateid { get; set; } = 0;

        public string fldintermediaterdetails { get; set; } = "";

        public long? fldbeneficiaryid { get; set; } = 0;

        public string fldbeneficiarydetails { get; set; } = "";

        public long? fldmanagerid { get; set; } = 0;

        public bool fldconfidential { get; set; } = false;

        public long? fldentityid { get; set; } = 0;

        public long? fldentitycontactid { get; set; } = 0;

        public long? fldreceptionid { get; set; } = 0;
        
    }


    public class tblCaseDac : DapperBaseRepository<tblCase>
    {
        private const string tableName = "tblcase";
        private const string pkeyName = "fldcaseid";

        public tblCaseDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblCaseDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        public List<tblCase> GetByStatusByDate(long statusid)
        {             
            // statusid = 0 => Active Case
            //   statusid = 1 => Closed Case
            try
            {
                var obj = Connection.Query<tblCase>(SqlSelectCommand + " WHERE fldcasestatusid = @statusid ORDER BY fldcreatedat DESC ", new { statusid = statusid }).AsList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByStatusByDate: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        public List<tblCase> GetbyManager(long managerid)
        {
            // statusid = 0 => Active Case
            //   statusid = 1 => Closed Case
            try
            {
                var obj = Connection.Query<tblCase>(SqlSelectCommand + " WHERE fldmanagerid = @managerid ORDER BY fldcreatedat DESC ", new { managerid = managerid }).AsList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByStatusByDate: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }



        ~tblCaseDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}