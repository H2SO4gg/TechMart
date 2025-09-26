using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechMart.Common;


using TechMart.UserControls;

namespace TechMart.Forms
{
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
        }
        private void OpenUserControlInPanel(UserControl uc)
        {
            pnlMain.Controls.Clear(); 

            uc.Dock = DockStyle.Fill; 
            pnlMain.Controls.Add(uc); 
            uc.BringToFront();         
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            ProductManagement productManagementUC = new ProductManagement();
            OpenUserControlInPanel(productManagementUC);
        }

       private void btnInventory_Click(object sender, EventArgs e)
        {
            InventoryManagement uc = new InventoryManagement();
            OpenUserControlInPanel(uc);

        }

        private void btnSales_Click(object sender, EventArgs e)
        {
            var salesControl = new SalesHistoryControl();
            salesControl.ShowAllSales = true;  
            OpenUserControlInPanel(salesControl);
            salesControl.RefreshSales();
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            var reportsUC = new ReportsUC();
            OpenUserControlInPanel(reportsUC);
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            var employeeManagementUC = new EmployeeManagementControl();
            OpenUserControlInPanel(employeeManagementUC);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();

        }

        private void AdminDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btnProfileMenu_Click(object sender, EventArgs e)
        {
            contextMenuProfile.Show(btnProfileMenu, 0, btnProfileMenu.Height);
        }

        private void contextMenuProfile_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "Logout")
            {
                this.Hide();
                LoginForm login = new LoginForm();
                login.Show();
            }
            else if (e.ClickedItem.Text == "Edit Profile")
            {
                OpenEditProfileForm();
            }
        }
        private void OpenEditProfileForm()
        {
            
            EditProfileForm editForm = new EditProfileForm(LoggedInUser.UserID);
            editForm.ShowDialog();
        }

       
    }
}
