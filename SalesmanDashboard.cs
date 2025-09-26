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
    public partial class SalesmanDashboard : Form
    {
        public SalesmanDashboard()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            LoginForm login = new LoginForm();
            login.Show();
        }


        
        private void OpenFormInPanel(Form form)
        {
            MainPanel.Controls.Clear(); 
            form.TopLevel = false;         
            form.FormBorderStyle = FormBorderStyle.None; 
            form.Dock = DockStyle.Fill;    
            MainPanel.Controls.Add(form);
            MainPanel.Tag = form;
            form.Show();
        }
        

        private void LoadUserControlInPanel(UserControl control)
        {
            MainPanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            MainPanel.Controls.Add(control);
            control.BringToFront();
        }
        private void btnNewSale_Click(object sender, EventArgs e)
        {
            var newSaleControl = new TechMart.UserControls.NewSaleControl();
            LoadUserControlInPanel(newSaleControl);
        }

    

        private void btnSalesHistory_Click(object sender, EventArgs e)
        {
            var salesControl = new SalesHistoryControl();
            LoadUserControlInPanel(salesControl);
            salesControl.RefreshSales(); 
        }

     

        private void SalesmanDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
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

        private void btnProfileMenu_Click(object sender, EventArgs e)
        {
            contextMenuProfile.Show(btnProfileMenu, 0, btnProfileMenu.Height);
        }

        private void OpenEditProfileForm()
        {
            EditProfileForm editForm = new EditProfileForm(LoggedInUser.UserID);
            editForm.ShowDialog();
        }

    }

}
