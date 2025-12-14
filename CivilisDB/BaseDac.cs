using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Npgsql;
using NpgsqlTypes;

namespace CivilisDB
{
    public partial class BaseDac : IDisposable
    {
        private string _connectionString = "";
        private IDbConnection? _connection;
        private IDbTransaction? _transaction;

        public string ConnectionString
        {
            get { return _connectionString; }
            protected set { _connectionString = value; }
        }

        public IDbConnection Connection
        {
            get { return _connection; }
            protected set { _connection = value; }
        }

        protected BaseDac()
        {
            //Connection = ConnectionFactory.createConnection();
        }

        public BaseDac(string ConnectionString)
        {
            //Connection = ConnectionFactory.createConnection(ConnectionString);
        }

        public BaseDac(IDbConnection connection)
        {
            Connection = connection;
        }

        public IDbTransaction Transaction
        {
            get { return _transaction; }
            set
            {
                _transaction = value;
                if (_transaction != null)
                    _connection = _transaction.Connection;
            }
        }

        public IDbTransaction BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            _transaction = _connection.BeginTransaction(isolationLevel);
            return _transaction;
        }

        protected void OpenConnection()
        {
            if (_connection != null && _connection.State != ConnectionState.Open)
                _connection.Open();
        }

        protected void CloseConnection()
        {
            if (_connection != null)
                _connection.Close();
        }

        public void Dispose()
        {
            if (_transaction != null)
                _transaction.Dispose();

            if (_connection != null)
                _connection.Dispose();

            _transaction = null;
            _connection = null;
        }
    }
}
