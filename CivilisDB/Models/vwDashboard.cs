using System;
using System.Collections.Generic;
using Npgsql;
using Dapper;
using System.Data;
using Microsoft.Extensions.Logging;

namespace CivilisDB.Models
{
    public partial class vwDashboard:BaseEntity
    {

        public long NoOfOpenCases { get; set; }

        public long NoOfCases { get; set; }

        public long NoOfPendingInvitations { get; set; }

        public long Invitations { get; set; }

        public long FutureInvitations { get; set; }
    }

    public class DashboardDac : DapperBaseRepository<vwDashboard>
    {

        private const string tableName = "";
        private const string pkeyName = "";

        public DashboardDac(string ConnectionString, ILogger logger) : base(tableName, pkeyName, ConnectionString, logger)
        {
            
        }

        public DashboardDac(IDbConnection connection, ILogger logger) : base(tableName, pkeyName, connection, logger)
        {

        }


        public long GetNoOfOpenCases()
        {
            try
            {
                var obj = Connection.QueryFirst<long>("Select count(*) from tblCase where fldcasestatusid = " + Constants.CASE_OPEN.ToString());
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetNoOfOpenCases: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }

        public long GetNoOfCases()
        {
            try
            {
                var obj = Connection.QueryFirst<long>("Select count(*) from tblCase ");
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetNoOfCases: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }

        public long GetNoOfPendingInvitations()
        {
            try
            {
                var obj = Connection.QueryFirst<long>("Select count(*) from tblinvitation where fldreplied = false");
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetNoOfPendingInvitations: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }


        public long GetNoOfInvitations()
        {
            try
            {
                var obj = Connection.QueryFirst<long>("Select count(*) from tblinvitation");
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetNoOfInvitations: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }


        public long GetNoOfFutureInvitations()
        {
            try
            {
                var obj = Connection.QueryFirst<long>("Select count(*) from tblinvitation where Select count(*) from tblinvitation where flddatetime >= now()");
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, "GetNoOfFuture: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return 0;
            }
        }

        ~DashboardDac()
        {
            if (Connection != null)
                Connection.Close();
        }

    }

}
