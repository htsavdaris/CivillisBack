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

    [Table("tbleklogikidomi")]
    public partial class tblEklogikiDomi : BaseEntity
    {

        [Column, Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string flddomiid { get; set; } = "";

        public long fkddomitypeid { get; set; } = 0;

        public string fldperigrafi { get; set; } = "";
       
    }


    public class tblEklogikiDomiDac : DapperBaseRepository<tblEklogikiDomi>
    {
        private const string tableName = "tbleklogikidomi";
        private const string pkeyName = "flddomiid";

        public tblEklogikiDomiDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public tblEklogikiDomiDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }

        public tblEklogikiDomi GetByDomiId(string flddomiid)
        {
            try
            {
                var obj = Connection.QueryFirst<tblEklogikiDomi>(SqlSelectCommand + " WHERE flddomiid = @flddomiid ", new { flddomiid = flddomiid });
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByStatusByDomiId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        public List<tblEklogikiDomi> GetByDomiTypeId(long fkddomitypeid)
        {
            try
            {
                var obj = Connection.Query<tblEklogikiDomi>(SqlSelectCommand + " WHERE fkddomitypeid = @fkddomitypeid ", new { fkddomitypeid = fkddomitypeid }).AsList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetByStatusByDomiTypeId: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }

        }

        ~tblEklogikiDomiDac()
        {
            if (Connection != null)
                Connection.Close();
        }
    }
}
