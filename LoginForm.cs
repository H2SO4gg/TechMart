using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechMart.Data;
using TechMart.Common;

namespace TechMart.Forms
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string useremail = txtUseremail.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT UserID, Useremail, Role FROM Users WHERE Useremail=@user AND Password=@pass";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", useremail);
                cmd.Parameters.AddWithValue("@pass", password);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32(0);
                        string email = reader.GetString(1);
                        string role = reader.GetString(2);

                        
                        LoggedInUser.UserID = userId;
                        LoggedInUser.Email = email;
                        LoggedInUser.Role = role;

                        if (role == "Admin")
                        {
                            AdminDashboard admin = new AdminDashboard();
                            admin.Show();
                            this.Hide();
                        }
                        else if (role == "Salesman")
                        {
                            SalesmanDashboard salesman = new SalesmanDashboard();
                            salesman.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username or Password!");
                    }
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUseremail.Text = "";
            txtPassword.Text = "";
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
