using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using static Dapper.SqlMapper;

namespace CivilisDB.Models
{

    [Table("tblentity")]
    public partial class tblEntity : BaseEntity
    {
        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long fldentityid { get; set; }
        public string fldentityname { get; set; }
        public long fldentitytypeid { get; set; }
        public string fldaddress { get; set; }

        public string fldnumber { get; set; }

        public string fldtk { get; set; }

        public string fldcity { get; set; }

        public string fldcountry { get; set; }

        public string fldfixedphone { get; set; }

        public string fldmobile { get; set; }

        public string fldmail { get; set; }



    }



    public class tblEntityDac : DapperBaseRepository<tblEntity>
    {

        private const string tableName = "tblentity";
        private const string pkeyName = "fldentityid";

        public tblEntityDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblEntityDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {
        }

        public List<tblEntity> GetAllSortedByID()
        {
            _logger.LogTrace(1, "Get All Sorted is called");
            try
            {
                var obj = Connection.Query<tblEntity>(SqlSelectCommand + " ORDER BY fldentityid DESC  ").ToList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(tblEntity).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

       
        public tblEntity GetbyName(string name)
        {
            _logger.LogTrace(1, "Get DB in BR is called");
            try
            {
                var obj = Connection.QueryFirst<tblEntity>(SqlSelectCommand + " WHERE fldentityname = @fldentityname  ", new { fldentityname = name } );                
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(tblEntity).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        ~tblEntityDac()
        {
            if (Connection != null)
                Connection.Close();
        }

    }
}
