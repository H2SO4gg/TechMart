using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechMart.Common;
using TechMart.Services;

namespace TechMart.UserControls
{
    public partial class EmployeeManagementControl : UserControl
    {
        private readonly UserService userService = new UserService();

        public EmployeeManagementControl()
        {
            InitializeComponent();

           
            btnAddEmployee.Click -= btnAddEmployee_Click;
            btnAddEmployee.Click += btnAddEmployee_Click;

            btnClearForm.Click -= btnClearForm_Click;
            btnClearForm.Click += btnClearForm_Click;

            txtPhone.KeyPress -= txtPhone_KeyPress;
            txtPhone.KeyPress += txtPhone_KeyPress;

            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.MultiSelect = false;
            dgvEmployees.ReadOnly = true;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            LoadEmployees();
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
        private void LoadEmployees()
        {
            try
            {
                DataTable dt = userService.GetAllEmployees();
                dgvEmployees.DataSource = dt;
                dgvEmployees.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employees: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ClearForm();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text; 

           
            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(phone) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

         
            if (!Regex.IsMatch(phone, @"^\d{11}$"))
            {
                MessageBox.Show("Phone number must be exactly 11 digits.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                if (addr.Address != email)
                {
                    MessageBox.Show("Please enter a valid email address.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Please enter a valid email address.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }

            try
            {
                if (userService.EmailExists(email))
                {
                    MessageBox.Show("This email is already registered. Choose a different email.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtEmail.Focus();
                    return;
                }
                if (userService.PhoneExists(phone))
                {
                    MessageBox.Show("This phone number is already used. Use a different phone number.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPhone.Focus();
                    return;
                }

              

                bool added = userService.AddEmployee(name, phone, email, password);
                if (added)
                {
                    MessageBox.Show("Employee added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployees(); 
                }
                else
                {
                    MessageBox.Show("Failed to add employee. Email or phone may already exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClearForm_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            txtName.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            txtName.Focus();
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvEmployees.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Please select an employee to delete.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var row = dgvEmployees.SelectedRows[0];
                if (row == null) return;

                int userId = Convert.ToInt32(row.Cells["UserID"].Value);
                string userEmail = row.Cells["Useremail"]?.Value?.ToString() ?? "(unknown)";

                if (userId == LoggedInUser.UserID)
                {
                    MessageBox.Show("You cannot delete your own account while logged in.", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

              
                var confirm = MessageBox.Show($"Are you sure you want to delete employee '{userEmail}' (ID: {userId})? This cannot be undone.",
                                              "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm != DialogResult.Yes) return;

               
                if (userService.HasSales(userId))
                {
                    MessageBox.Show("This employee has recorded sales and cannot be deleted. If you must remove them, consider disabling the account instead.",
                                    "Delete Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (userService.DeleteEmployee(userId, out string errorMessage))
                {
                    MessageBox.Show("Employee deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadEmployees();
                }
                else
                {
                    MessageBox.Show("Employee not deleted: " + errorMessage, "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting employee: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

   
    
    }
}
