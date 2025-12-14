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
    [Table("vwcase")]
    public partial class vwCase : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldcaseid { get; set; }

        public string fldcasetitle { get; set; } = "";

        public long? fldcasetypeid { get; set; } = 0;

        public string fldcasedescription { get; set; } = "";

        public DateTime? fldcaseopendate { get; set; }

        public long? fldcasestatusid { get; set; }

        public string fldcomments { get; set; } = "";

        public long? fldintermediateid { get; set; }
        public string fldintermediaterdetails { get; set; } = "";

        public long? fldbeneficiaryid { get; set; }

        public string fldbeneficiarydetails { get; set; } = "";

        public long? fldmanagerid { get; set; }

        public bool? fldconfidential { get; set; }

        public long? fldentityid { get; set; }

        public string fldentityname { get; set; } = "";

        public long? fldentitycontactid { get; set; }

        public string fldentitycontactlastname { get; set; } = "";

        public string fldentitycontactfirstname { get; set; } = "";

        public string fldentitycontactmobile { get; set; } = "";

        public long? fldreceptionid { get; set; }

        public string fldcasetypedesc { get; set; } = "";

        public string fldcasestatusdesc { get; set; } = "";

        public string fldlastname { get; set; } = "";

        public string fldbeneficiarylastname { get; set; } = "";

        public string fldbeneficiaryfirstname { get; set; } = "";

        public string fldbeneficiarymobile { get; set; } = "";

        public string fldintermediatelastname { get; set; } = "";

        public string fldintermediatefirstname { get; set; } = "";

        public string fldintermediatemobile { get; set; } = "";

        public string fldmanagerlastname { get; set; } = "";

        public string fldmanagerfirstname { get; set; } = "";

        public string fldmanagermobile { get; set; } = "";

        public string fldreceptionlastname { get; set; } = "";

        public string fldreceptionfirstname { get; set; } = "";

        public string fldreceptionmobile { get; set; } = "";

    }


    public class vwCaseDac : DapperBaseRepository<vwCase>
    {
        private const string tableName = "vwcase";
        private const string pkeyName = "fldcaseid";

        public vwCaseDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public vwCaseDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        public List<vwCase>? GetByStatusByDate(long statusid)
        {
            // statusid = 0 => Active Case
            //   statusid = 1 => Closed Case
            try
            {
                var obj = Connection.Query<vwCase>(SqlSelectCommand + " WHERE fldcasestatusid = @statusid ORDER BY fldupdatedat DESC ", new { statusid = statusid }).AsList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByStatusByDate: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        public List<vwCase>? GetbyManager(long managerid)
        {
            // statusid = 0 => Active Case
            //   statusid = 1 => Closed Case
            try
            {
                var obj = Connection.Query<vwCase>(SqlSelectCommand + " WHERE fldmanagerid = @managerid ORDER BY createdat DESC ", new { managerid = managerid }).AsList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByStatusByDate: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }


        

        ~vwCaseDac()
        {
            Connection?.Close();
        }
    }

}
