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

    [Table("vwentity")]
    public partial class vwEntity : BaseEntity
    {
        [Column, Key]
        public long? fldentityid { get; set; }


        public string fldentityname { get; set; } = "";

        public long? fldentitytypeid { get; set; }


        public string fldaddress { get; set; } = "";


        public string fldnumber { get; set; } = "";


        public string fldtk { get; set; } = "";

        public string fldcity { get; set; } = "";

        public string fldcountry { get; set; } = "";

        public string fldfixedphone { get; set; }

        public string fldmobile { get; set; }

        public string fldmail { get; set; }

        public string fldentitytypedesc { get; set; }
    }

    public class vwEntityDac : DapperBaseRepository<vwEntity>
    {

        private const string tableName = "vwentity";
        private const string pkeyName = "fldentityid";

        public vwEntityDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public vwEntityDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {
        }

        public vwEntity Get(long id)
        {
            try
            {
                return Connection.QueryFirstOrDefault<vwEntity>(SqlSelectCommand + " WHERE fldentityid = @id", new { id });
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(vwEntity).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message:" + ex.Message);
                return null;
            }
        }

        public List<vwEntity> GetAllSortedByID()
        {
            _logger.LogTrace(1, "Get All Sorted is called");
            try
            {
                var obj = Connection.Query<vwEntity>(SqlSelectCommand + " ORDER BY fldentityid DESC  ").ToList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(tblEntity).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

       
        ~vwEntityDac()
        {
            if (Connection != null)
                Connection.Close();
        }

    }
}