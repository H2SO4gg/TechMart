using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechMart.Services;

namespace TechMart.UserControls
{
    public partial class ReportsUC : UserControl
    {
        private readonly SaleService saleService = new SaleService();
        private readonly ProductService productService = new ProductService();

        public ReportsUC()
        {
            InitializeComponent();
        }



        private void lblSales_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            loadFilter();
        }

        private void loadFilter()
        {
            DataTable dt = saleService.GetSaleByDate(dtpFrom.Value, dtpTo.Value);

            decimal grandTotal = 0;

            if (dt.Rows.Count > 0)
            {
                object sumObj = dt.Compute("SUM(Total)", string.Empty);
                grandTotal = sumObj != DBNull.Value ? Convert.ToDecimal(sumObj) : 0;
            }

            lblSalesAmount.Text = grandTotal.ToString("C2");

            lblProfitAmount.Text = (grandTotal - (grandTotal * 0.92m)).ToString("C2");
        }

        private void LoadLowStockItems()
        {
            DataTable dt2 = productService.getLowStockProductTable();
            dgvLowStockItem.DataSource = dt2;
        }

        private void LoadWeeklySalesChart()
        {
            DateTime weekstart = DateTime.Now.AddDays(-6);
            DataTable dt = saleService.GetWeeklySalesSummary(weekstart);

            chartSales.DataSource = dt;
            chartSales.DataBind();
        }

        private void ReportsUC_Load(object sender, EventArgs e)
        {
            LoadWeeklySalesChart();
            LoadLowStockItems();
            dtpFrom.Value = DateTime.Today.AddDays(-7);
            dtpTo.Value = DateTime.Today;
            loadFilter();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
