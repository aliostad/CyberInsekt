using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
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
                var urls = con.Query("UrlExists", 
                    param: new {UrlHash = hash},
                    commandType: CommandType.StoredProcedure);
                return urls.Any();
            }

        }

        protected override void Enqueue(byte[] hash, string url)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                con.Execute("Enqueue",
                            param: new { UrlHash = hash, Url = url },
                            commandType: CommandType.StoredProcedure);
            }
        }

        public override bool TryDequeue(out Uri uri)
        {
            uri = null;
            using (var con = new SqlConnection(_connectionString))
            {
                con.Open();
                IEnumerable<dynamic> result = con.Query("Dequeue", commandType: CommandType.StoredProcedure)
                    .ToList();
                if (result.Any())
                {
                    string url = result.First().Url;
                    if (string.IsNullOrEmpty(url))
                    {
                        return false;
                    }
                    else
                    {
                        uri = new Uri(url);
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
