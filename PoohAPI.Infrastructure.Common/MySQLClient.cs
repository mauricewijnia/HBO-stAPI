﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace PoohAPI.Infrastructure.Common
{
    public class MySQLClient : IMySQLClient
    {
        private MySqlConnection _connection;
        private string _server;
        private string _database;
        private string _uid;
        private string _password;
        private IConfiguration _config;

        public MySQLClient(IConfiguration config)
        {
            _config = config;
            Init();
        }

        //Had to turn this into a method to expose the connection to the caller.
        public MySqlConnection Connection()
        {
            return _connection; 
        }

        private void Init()
        {
            //These values should be retrieved from the appsettings.json and the configurationportal on Azure later on.
            
            //_server = "localhost";
            //_database = "elbho_register";
            //_uid = "root";
            //_password = "";

            _server = _config.GetValue<string>("DatabaseHost");
            _database = _config.GetValue<string>("DatabaseName");
            _uid = _config.GetValue<string>("DatabaseUid");
            _password = _config.GetValue<string>("DatabasePassword");

            //var connectionString = string.Format("server={0};database={1};uid={2};password={3};", _server, _database,
            //    _uid, _password);


            //SslMode is set to none for local db
            //var connectionString = string.Format("server={0};database={1};uid={2};password={3};SslMode=none", _server, _database, _uid, _password);

            // SslMode is set to none for local db
            var connectionString = string.Format("server={0};database={1};uid={2};password={3};SslMode=none", _server, _database,
                _uid, _password);
            _connection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Open)
                return true;

            _connection.Open();
            return true;
        }

        public bool CloseConnection()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
                return true;

            _connection.Close();
            return true;
        }
    }
}