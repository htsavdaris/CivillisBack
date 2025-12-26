using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Dapper.Database;
using Dapper.Database.Extensions;
using Npgsql;
using NpgsqlTypes;
using Microsoft.Extensions.Logging;

using CivilisDB.Models;

namespace CivilisDB
{
    public class DapperBaseRepository<TEntity> : IDisposable
            where TEntity : BaseEntity

    {
        private string _connectionString;
        private IDbConnection _connection;
        public readonly ILogger _logger;
        protected readonly string _tableName;
        protected readonly string _pkeyName;
        protected readonly string SqlSelectCommand;
        protected readonly string SqlDeleteCommand;
        public string ConnectionString
        {
            protected get { return _connectionString; }
            set { _connectionString = value; }
        }

        public IDbConnection Connection
        {
            protected get { return _connection; }
            set { _connection = value; }
        }

        
        public DapperBaseRepository(string ConnectionString, ILogger logger)
        {
            throw new NotImplementedException(); 
            
        }

        public DapperBaseRepository(string tableName, string pkeyName, string ConnectionString, ILogger logger)
        {
            _pkeyName = pkeyName;
            _tableName = tableName;
            SqlSelectCommand = "SELECT * FROM " + _tableName + " ";
            
            _logger = logger;
            _logger.LogTrace(1, "DapperBaseRepository is called");
            try
            {
                Connection = ConnectionFactory.createConnection(ConnectionString);
            } catch (Exception ex) {
                _logger.LogError(1, "DapperBaseRepository error:" + ex.Message );
            }
        }


        public DapperBaseRepository(string tableName,  string pkeyName, IDbConnection connection, ILogger logger)
        {
            _pkeyName = pkeyName;
            _tableName = tableName;
            SqlSelectCommand = "SELECT * FROM " + _tableName + " ";
            _logger = logger;
            Connection = connection;
        }

        public TEntity Get(long id)
        {
            _logger.LogTrace(1, "Get DB in BR is called");
            try
            {
                var obj = Connection.Get<TEntity>(id);
                return obj;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(TEntity).FullName + " Get: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);                
                return null;
            }
        }

        public List<TEntity> GetAll()
        {
            try
            {
                var oList = Connection.GetList<TEntity>().AsList();
                return oList;
            }
            catch (NpgsqlException ex)
            {
                
                _logger.LogError(3, typeof(TEntity).FullName + " GetAll: NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public List<TEntity> GetAllSortedDesc()
        {
            try
            {
                var obj = Connection.Query<TEntity>(SqlSelectCommand + " ORDER BY + " + _pkeyName + " DESC LIMIT 300").ToList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                
                _logger.LogError(3, typeof(TEntity).FullName + " GetAllSortedDesc: NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public List<TEntity> GetAllSortedDescByUpdate()
        {
            try
            {
                var obj = Connection.Query<TEntity>(SqlSelectCommand + " ORDER BY fldupdatedat DESC LIMIT 300").ToList();
                return obj;
            }
            catch (NpgsqlException ex)
            {

                _logger.LogError(3, typeof(TEntity).FullName + " GetAllSortedDescByUpdate: NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }


        public List<TEntity> GetForSQL(string SQL)
        {
            try
            {
                var obj = Connection.Query<TEntity>(SQL).ToList();
                return obj;
            }
            catch (NpgsqlException ex)
            {
                
                _logger.LogError(3, typeof(TEntity).FullName + " GetForSQL: NpgsqlException Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return null;
            }
        }

        public bool Insert(TEntity obj)
        {
            try
            {
                var res = Connection.Insert<TEntity>(obj);
                return res;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(TEntity).FullName + " Insert: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }

        public bool Update(TEntity obj)
        {
            try
            {
                var isSuccess = Connection.Update<TEntity>(obj);
                return isSuccess ;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(3, typeof(TEntity).FullName + " Update: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }

        public bool Delete(long id)
        {
            try
            {
                var isSuccess = Connection.Delete<TEntity>(id);
                return isSuccess;
            }
            catch (NpgsqlException ex)
            {
                _logger.LogError(1, typeof(TEntity).FullName + " Delete: Npgsql Exception Code:" + ex.ErrorCode + " Message :" + ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Close();

            _connection = null;
        }

    }
}
