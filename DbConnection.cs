using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechMart.Data
{
    public static class DbConnection
    {
        private static string connectionString =
            @"Server=localhost\SQLEXPRESS;Database=TechMartDB;Trusted_Connection=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
