﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using CyberInsekt.Storage;
using Dapper;


namespace CyberInsekt.SqlUrlStore
{
    public class UrlStore : UrlStoreBase
    {
        private string _connectionString;
        private const string ConnectionStringName = "UrlStore";

        /// <summary>
        /// Assumes there is a connection string defined named "UrlStore". 
        /// </summary>
        public UrlStore() : this(ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString)
        {
            
        }

        public UrlStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Store(byte[] hash, string url)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute("InsertUrl", param: 
                    new { UrlHash = hash, Url = url },
                    commandType: CommandType.StoredProcedure);
            }
        }

        protected override bool Exists(byte[] hash, string url)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                var urls = con.Query("GetUrl", 
                    param: new {UrlHash = hash},
                    commandType: CommandType.StoredProcedure);
                return urls.Any();
            }

        }
    }
}
