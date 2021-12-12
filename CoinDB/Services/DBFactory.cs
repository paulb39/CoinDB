using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinDB.Models;
using MySql.Data.MySqlClient;

namespace CoinDB.Services
{
    public class DBFactory
    {
        private string _connectionString = null;

        public DBFactory()
        {
            var mysqlConnectionStringBuilder = new MySqlConnectionStringBuilder();
            mysqlConnectionStringBuilder.Server = "localhost";
            mysqlConnectionStringBuilder.Database = "coins";
            mysqlConnectionStringBuilder.UserID = "root";
            mysqlConnectionStringBuilder.Password = "root";
            mysqlConnectionStringBuilder.SslMode = MySqlSslMode.None;

            _connectionString = mysqlConnectionStringBuilder.ToString();
        }

        public List<Transaction> GetTransactionDataFromTicker(string ticker)
        {
            var transaction = new List<Transaction>();

            string sql = String.Format(@"SELECT * from transaction where ticker = '{0}' and sold = false;", ticker);

            using (MySqlConnection conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    using (MySqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var foundTransaction = new Transaction();
                            foundTransaction.Name = rdr.GetString(1);
                            foundTransaction.ticker = rdr.GetString(2);
                            foundTransaction.BuyDate = rdr.GetDateTime(3);
                            foundTransaction.Quantity = rdr.GetDouble(4);
                            foundTransaction.Price = rdr.GetDouble(5);
                            foundTransaction.Total = rdr.GetDouble(6);
                            foundTransaction.Staked = rdr.GetBoolean(7);
                            foundTransaction.Sold = rdr.GetBoolean(8);

                            var ord = rdr.GetOrdinal("sell_date");
                            if (!rdr.IsDBNull(ord))
                            {
                                foundTransaction.SellDate = rdr.GetDateTime(9);
                            }                          

                            transaction.Add(foundTransaction);
                        }
                    }
                }
            }

            return transaction;
        }

    }
}
