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
    [Table("vwuserrole")]
    public partial class vwUserRole : BaseEntity
    {
        [Column, Key]
        public long flduserroleid { get; set; } = 0;


        public long fldroleid { get; set; } = 0;

        public long flduserid { get; set; } = 0;

        public string fldlastname { get; set; } = "";

        public string fldfirstname { get; set; } = "";

        public string fldlogin { get; set; } = "";

        public string fldemail { get; set; } = "";

        public string fldmobile { get; set; } = "";

        public string fldroledesc { get; set; } = "";


    }



    public class vwUserRoleDac : DapperBaseRepository<vwUserRole>
    {

        private const string tableName = "vwuserrole";
        private const string pkeyName = "flduserroleid";

        public vwUserRoleDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public vwUserRoleDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {
        }

        public List<vwUserRole> GetAllForUserId(long userid)
        {
            _logger.LogTrace(1, "GetAllForId is called");
            try
            {
                var obj = Connection.Query<vwUserRole>(SqlSelectCommand + " WHERE flduserid = @userid ", new { userid = userid }).ToList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(vwUserRole).FullName + " GetAllForUserId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }


        ~vwUserRoleDac()
        {
            if (Connection != null)
                Connection.Close();
        }

    }
}
