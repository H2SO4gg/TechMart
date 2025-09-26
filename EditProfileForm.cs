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

namespace TechMart.Forms
{
    public partial class EditProfileForm : Form
    {
        private int currentUserId;

        public EditProfileForm(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            LoadUserData();
        }

        private void LoadUserData()
        {
            using (SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=TechMartDB;Trusted_Connection=True;"))
            {
                conn.Open();
                string query = "SELECT Name, Useremail, Phone, Password FROM Users WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", currentUserId);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    txtName.Text = reader["Name"].ToString();
                    txtEmail.Text = reader["Useremail"].ToString();
                    txtPhone.Text = reader["Phone"].ToString();
                    txtPassword.Text = reader["Password"].ToString();
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please fill all fields.");
                return;
            }

            if (txtPhone.Text.Length != 11 || !txtPhone.Text.All(char.IsDigit))
            {
                MessageBox.Show("Phone must be exactly 11 digits.");
                return;
            }

            
            using (SqlConnection conn = new SqlConnection(@"Server=localhost\SQLEXPRESS;Database=TechMartDB;Trusted_Connection=True;"))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE (Useremail = @Email OR Phone = @Phone) AND UserID != @UserID";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                checkCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                checkCmd.Parameters.AddWithValue("@UserID", currentUserId);

                int count = (int)checkCmd.ExecuteScalar();
                if (count > 0)
                {
                    MessageBox.Show("Email or Phone already exists.");
                    return;
                }

                
                string updateQuery = @"UPDATE Users 
                               SET Name=@Name, Useremail=@Email, Phone=@Phone, Password=@Password
                               WHERE UserID=@UserID";

                SqlCommand updateCmd = new SqlCommand(updateQuery, conn);
                updateCmd.Parameters.AddWithValue("@Name", txtName.Text);
                updateCmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                updateCmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                updateCmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                updateCmd.Parameters.AddWithValue("@UserID", currentUserId);

                updateCmd.ExecuteNonQuery();

                MessageBox.Show("Profile updated successfully!");
                this.Close();
            }
        }

        private void EditProfileForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }
    }
}
