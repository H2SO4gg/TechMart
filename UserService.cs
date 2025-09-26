using System;
using System.Data;
using System.Data.SqlClient;
using TechMart.Data;

namespace TechMart.Services
{
    public class UserService
    {
       
        public DataTable GetAllEmployees()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string sql = "SELECT UserID, Name, Phone, Useremail FROM Users WHERE Role = 'Salesman' ORDER BY UserID DESC";
                using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                {
                    da.Fill(dt);
                }
            }
            return dt;
        }

        public bool EmailExists(string email)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Useremail = @Email", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        public bool PhoneExists(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Phone = @Phone AND Phone <> ''", conn))
                {
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

      
        public bool PasswordExists(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Password = @Password", conn))
                {
                    cmd.Parameters.AddWithValue("@Password", password);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        
        public bool AddEmployee(string name, string phone, string email, string password)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

              
                using (SqlCommand check = new SqlCommand("SELECT COUNT(1) FROM Users WHERE Useremail = @Email OR (Phone = @Phone AND Phone <> '')", conn))
                {
                    check.Parameters.AddWithValue("@Email", email);
                    check.Parameters.AddWithValue("@Phone", phone ?? string.Empty);
                    int cnt = Convert.ToInt32(check.ExecuteScalar());
                    if (cnt > 0) return false;
                }

                string sql = @"INSERT INTO Users (Useremail, Password, Role, Name, Phone)
                               VALUES (@Email, @Password, @Role, @Name, @Phone)";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password); 
                    cmd.Parameters.AddWithValue("@Role", "Salesman");
                    cmd.Parameters.AddWithValue("@Name", name ?? "");
                    cmd.Parameters.AddWithValue("@Phone", phone ?? "");
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

       
        public bool HasSales(int userId)
        {
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM Sales WHERE UserID = @UserID", conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    int cnt = Convert.ToInt32(cmd.ExecuteScalar());
                    return cnt > 0;
                }
            }
        }

        public bool DeleteEmployee(int userId, out string errorMessage)
        {
            errorMessage = null;
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                
                using (SqlCommand cmdRole = new SqlCommand("SELECT Role FROM Users WHERE UserID = @UserID", conn))
                {
                    cmdRole.Parameters.AddWithValue("@UserID", userId);
                    var roleObj = cmdRole.ExecuteScalar();
                    if (roleObj == null)
                    {
                        errorMessage = "User not found.";
                        return false;
                    }

                    string role = roleObj.ToString();
                    if (!role.Equals("Salesman", StringComparison.OrdinalIgnoreCase))
                    {
                        errorMessage = "Only Salesman users can be deleted via this panel.";
                        return false;
                    }
                }

                
                using (SqlCommand cmdCheckSales = new SqlCommand("SELECT COUNT(1) FROM Sales WHERE UserID = @UserID", conn))
                {
                    cmdCheckSales.Parameters.AddWithValue("@UserID", userId);
                    int salesCount = Convert.ToInt32(cmdCheckSales.ExecuteScalar());
                    if (salesCount > 0)
                    {
                        errorMessage = $"Cannot delete user: they have {salesCount} sale(s) recorded.";
                        return false;
                    }
                }

                
                using (SqlCommand cmdDelete = new SqlCommand("DELETE FROM Users WHERE UserID = @UserID", conn))
                {
                    cmdDelete.Parameters.AddWithValue("@UserID", userId);
                    int rows = cmdDelete.ExecuteNonQuery();
                    if (rows > 0) return true;
                    errorMessage = "Delete failed (no rows affected).";
                    return false;
                }
            }
        }


    }
}
