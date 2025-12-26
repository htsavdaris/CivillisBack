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
    [Table("vwinvitation")]
    public partial class vwInvitation : BaseEntity
    {
        [Column, Key]
        public long fldinvitationid { get; set; }

        public string fldtitle { get; set; } = "";

        public string fldlocation { get; set; } = "";

        public DateTime? flddatetime { get; set; }

        public string? fldhost { get; set; } = "";

        public long? fldentityid { get; set; }

        public long? fldreplied { get; set; }

        public long? fldgopersonid { get; set; }

        public long? fldhostpersonid { get; set; }

        public string fldcomments { get; set; } = "";

        public string? fldreplydesc { get; set; }

        public string? fldorglastname { get; set; } = "";

        public string? fldorgfirstname { get; set; } = "";

        public string? fldorgmobile { get; set; } = "";

        public string? fldgoeslastname { get; set; } = "";

        public string? fldgoesfirstname { get; set; } = "";

        public string? fldgoesmobile { get; set; } = "";

        public string? fldentityname { get; set; } = "";

        public string? fldentitymobile { get; set; } = "";
    }

    public class vwInvitationDac : DapperBaseRepository<vwInvitation>
    {

        private const string tableName = "vwinvitation";
        private const string pkeyName = "fldinvitationid";

        public vwInvitationDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {

        }

        public vwInvitationDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {
        }

        public List<vwInvitation>? GetAllSortedByID()
        {
            _logger.LogTrace(1, "Get All Sorted is called");
            try
            {
                var obj = Connection.Query<vwInvitation>(SqlSelectCommand + " ORDER BY fldinvitationid DESC  ").ToList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(vwInvitation).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }


        //public List<vwInvitation>? GetAllSortedByUpdated()
        //{
        //    _logger.LogTrace(1, "Get All Sorted is called");
        //    try
        //    {
        //        var obj = Connection.Query<vwInvitation>(SqlSelectCommand + " ORDER BY fldupdatedat DESC  ").ToList();
        //        return obj;
        //    }
        //    catch (NpgsqlException ex)
        //    {
        //        _logger.LogError(3, typeof(vwInvitation).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
        //        return null;
        //    }
        //}


        ~vwInvitationDac()
        {
            Connection?.Close();
        }

    }

}
