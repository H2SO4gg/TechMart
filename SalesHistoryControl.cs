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
using TechMart.Services;
using TechMart.Common;

namespace TechMart.UserControls
{
    public partial class SalesHistoryControl : UserControl
    {
        private readonly SaleService saleService = new SaleService();
        private DataTable salesTable;
        public bool ShowAllSales { get; set; } = false;

        public SalesHistoryControl()
        {
            InitializeComponent();
            InitializeControls();
         
        }
        public void RefreshSales()
        {
            LoadSales();
        }

        private void InitializeControls()
        {
            // Default date range: last 30 days
            dtpTo.Value = DateTime.Today.AddDays(1).AddSeconds(-1);
            dtpFrom.Value = DateTime.Today.AddDays(-30);

         
            dgvSales.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSales.MultiSelect = false;
            dgvSales.ReadOnly = true;
            dgvSales.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvSaleItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvSaleItems.MultiSelect = false;
            dgvSaleItems.ReadOnly = true;
            dgvSaleItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

           
            btnFilter.Click += btnFilter_Click;
            btnRefresh.Click += btnRefresh_Click;
            txtSearch.TextChanged += txtSearch_TextChanged;
            dgvSales.SelectionChanged += dgvSales_SelectionChanged;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            LoadSales();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ApplySearchFilter();
        }
        public void LoadSales()
        {
            int? userId = ShowAllSales ? (int?)null : LoggedInUser.UserID;
            DateTime from = dtpFrom.Value.Date;
            DateTime to = dtpTo.Value.Date.AddDays(1).AddSeconds(-1);

            salesTable = saleService.GetSales(userId, from, to);
            dgvSales.DataSource = salesTable;

       
            if (dgvSales.Columns.Contains("SaleDate"))
                dgvSales.Columns["SaleDate"].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm";
            if (dgvSales.Columns.Contains("Subtotal"))
                dgvSales.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
            if (dgvSales.Columns.Contains("Total"))
                dgvSales.Columns["Total"].DefaultCellStyle.Format = "N2";
            if (dgvSales.Columns.Contains("Salesman"))
                dgvSales.Columns["Salesman"].HeaderText = "Salesman";

            dgvSaleItems.DataSource = null;

           
            dgvSales.ClearSelection();
            dgvSales.CurrentCell = null;

           
            if (salesTable.Rows.Count > 0 && dgvSales.Columns.Contains("SaleID"))
            {
                var firstRow = dgvSales.Rows[0];
                firstRow.Selected = true;

                var cell = firstRow.Cells["SaleID"];
                if (cell != null)
                    dgvSales.CurrentCell = cell; 
            }
        }

        private void dgvSales_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSales.SelectedRows.Count == 0 || !dgvSales.Columns.Contains("SaleID"))
            {
                dgvSaleItems.DataSource = null;
                return;
            }

            var row = dgvSales.SelectedRows[0];
            if (row?.Cells["SaleID"]?.Value == null) return;

            if (!int.TryParse(row.Cells["SaleID"].Value.ToString(), out int saleId)) return;
            LoadSaleItems(saleId);
        }
      

        private void LoadSaleItems(int saleId)
        {
            DataTable dtItems = saleService.GetSaleItemsBySaleId(saleId);
            dgvSaleItems.DataSource = dtItems;
            dgvSaleItems.ClearSelection();
            dgvSaleItems.CurrentCell = null;
        }

        
        private void ApplySearchFilter()
        {
            if (salesTable == null) return;
            string text = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(text))
            {
                salesTable.DefaultView.RowFilter = "";
                return;
            }

            // Try parse as sale id
            if (int.TryParse(text, out int id))
            {
                salesTable.DefaultView.RowFilter = $"SaleID = {id}";
            }
            else
            {
                // Optionally allow searching by date string or total: leave empty or implement more complex filter
                salesTable.DefaultView.RowFilter = $"CONVERT(SaleDate, System.String) LIKE '%{text}%'";
            }
        }
    }
}
   