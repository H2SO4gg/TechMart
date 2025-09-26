using System;
using System.Data;
using System.Data.SqlClient;
using TechMart.Data;

namespace TechMart.Services
{
    public class EmployeeService
    {
        public DataTable GetAllEmployees()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT UserID, Name, Phone, Useremail FROM Users WHERE Role='Salesman'";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.Fill(dt);
            }
            return dt;
        }

        public void AddEmployee(string name, string phone, string email, string password)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = @"INSERT INTO Users (Name, Phone, Useremail, Password, Role)
                                 VALUES (@Name, @Phone, @Email, @Password, 'Salesman')";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Phone", phone);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
