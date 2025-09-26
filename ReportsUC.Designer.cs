namespace TechMart.UserControls
{
    partial class ReportsUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.pnlGrossSales = new System.Windows.Forms.Panel();
            this.lblSalesAmount = new System.Windows.Forms.Label();
            this.lblSales = new System.Windows.Forms.Label();
            this.pnlGraphSales = new System.Windows.Forms.Panel();
            this.chartSales = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlGrossProfit = new System.Windows.Forms.Panel();
            this.lblProfitAmount = new System.Windows.Forms.Label();
            this.lblProfit = new System.Windows.Forms.Label();
            this.pnlLowStockItem = new System.Windows.Forms.Panel();
            this.dgvLowStockItem = new System.Windows.Forms.DataGridView();
            this.lblLowStock = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.ProductID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductBrand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblChart = new System.Windows.Forms.Label();
            this.pnlGrossSales.SuspendLayout();
            this.pnlGraphSales.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartSales)).BeginInit();
            this.pnlGrossProfit.SuspendLayout();
            this.pnlLowStockItem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStockItem)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlGrossSales
            // 
            this.pnlGrossSales.BackColor = System.Drawing.Color.White;
            this.pnlGrossSales.Controls.Add(this.lblSalesAmount);
            this.pnlGrossSales.Controls.Add(this.lblSales);
            this.pnlGrossSales.Location = new System.Drawing.Point(14, 71);
            this.pnlGrossSales.Name = "pnlGrossSales";
            this.pnlGrossSales.Size = new System.Drawing.Size(271, 197);
            this.pnlGrossSales.TabIndex = 0;
            // 
            // lblSalesAmount
            // 
            this.lblSalesAmount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalesAmount.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblSalesAmount.Location = new System.Drawing.Point(10, 99);
            this.lblSalesAmount.Name = "lblSalesAmount";
            this.lblSalesAmount.Size = new System.Drawing.Size(247, 50);
            this.lblSalesAmount.TabIndex = 0;
            this.lblSalesAmount.Text = "$";
            // 
            // lblSales
            // 
            this.lblSales.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSales.Location = new System.Drawing.Point(16, 30);
            this.lblSales.Name = "lblSales";
            this.lblSales.Size = new System.Drawing.Size(76, 38);
            this.lblSales.TabIndex = 0;
            this.lblSales.Text = "Sales";
            this.lblSales.Click += new System.EventHandler(this.lblSales_Click);
            // 
            // pnlGraphSales
            // 
            this.pnlGraphSales.BackColor = System.Drawing.Color.White;
            this.pnlGraphSales.Controls.Add(this.chartSales);
            this.pnlGraphSales.Controls.Add(this.lblChart);
            this.pnlGraphSales.Location = new System.Drawing.Point(14, 288);
            this.pnlGraphSales.Name = "pnlGraphSales";
            this.pnlGraphSales.Size = new System.Drawing.Size(992, 428);
            this.pnlGraphSales.TabIndex = 0;
            // 
            // chartSales
            // 
            chartArea3.AxisX.Interval = 1D;
            chartArea3.AxisX.LabelStyle.Format = "dd-MMM";
            chartArea3.AxisX.Title = "Date";
            chartArea3.AxisY.Title = "Sales";
            chartArea3.Name = "Main";
            this.chartSales.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartSales.Legends.Add(legend3);
            this.chartSales.Location = new System.Drawing.Point(3, 48);
            this.chartSales.Name = "chartSales";
            series3.BorderWidth = 2;
            series3.ChartArea = "Main";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            series3.IsValueShownAsLabel = true;
            series3.IsXValueIndexed = true;
            series3.Legend = "Legend1";
            series3.Name = "sales";
            series3.XValueMember = "Day";
            series3.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series3.YValueMembers = "DailySales";
            this.chartSales.Series.Add(series3);
            this.chartSales.Size = new System.Drawing.Size(986, 377);
            this.chartSales.TabIndex = 0;
            this.chartSales.Text = "Sales";
            // 
            // pnlGrossProfit
            // 
            this.pnlGrossProfit.BackColor = System.Drawing.Color.White;
            this.pnlGrossProfit.Controls.Add(this.lblProfitAmount);
            this.pnlGrossProfit.Controls.Add(this.lblProfit);
            this.pnlGrossProfit.Location = new System.Drawing.Point(304, 71);
            this.pnlGrossProfit.Name = "pnlGrossProfit";
            this.pnlGrossProfit.Size = new System.Drawing.Size(272, 197);
            this.pnlGrossProfit.TabIndex = 0;
            // 
            // lblProfitAmount
            // 
            this.lblProfitAmount.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfitAmount.ForeColor = System.Drawing.Color.DodgerBlue;
            this.lblProfitAmount.Location = new System.Drawing.Point(15, 99);
            this.lblProfitAmount.Name = "lblProfitAmount";
            this.lblProfitAmount.Size = new System.Drawing.Size(243, 50);
            this.lblProfitAmount.TabIndex = 0;
            this.lblProfitAmount.Text = "$";
            // 
            // lblProfit
            // 
            this.lblProfit.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfit.Location = new System.Drawing.Point(20, 30);
            this.lblProfit.Name = "lblProfit";
            this.lblProfit.Size = new System.Drawing.Size(76, 38);
            this.lblProfit.TabIndex = 0;
            this.lblProfit.Text = "Profit";
            // 
            // pnlLowStockItem
            // 
            this.pnlLowStockItem.BackColor = System.Drawing.Color.White;
            this.pnlLowStockItem.Controls.Add(this.dgvLowStockItem);
            this.pnlLowStockItem.Controls.Add(this.lblLowStock);
            this.pnlLowStockItem.Location = new System.Drawing.Point(595, 71);
            this.pnlLowStockItem.Name = "pnlLowStockItem";
            this.pnlLowStockItem.Size = new System.Drawing.Size(414, 197);
            this.pnlLowStockItem.TabIndex = 0;
            // 
            // dgvLowStockItem
            // 
            this.dgvLowStockItem.AllowUserToAddRows = false;
            this.dgvLowStockItem.AllowUserToDeleteRows = false;
            this.dgvLowStockItem.AllowUserToResizeColumns = false;
            this.dgvLowStockItem.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLowStockItem.BackgroundColor = System.Drawing.Color.White;
            this.dgvLowStockItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLowStockItem.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvLowStockItem.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLowStockItem.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProductID,
            this.ProductName,
            this.ProductBrand,
            this.ProductCategory,
            this.ProductQuantity});
            this.dgvLowStockItem.GridColor = System.Drawing.Color.White;
            this.dgvLowStockItem.Location = new System.Drawing.Point(3, 47);
            this.dgvLowStockItem.Name = "dgvLowStockItem";
            this.dgvLowStockItem.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvLowStockItem.RowHeadersVisible = false;
            this.dgvLowStockItem.Size = new System.Drawing.Size(408, 147);
            this.dgvLowStockItem.TabIndex = 1;
            // 
            // lblLowStock
            // 
            this.lblLowStock.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowStock.Location = new System.Drawing.Point(3, 6);
            this.lblLowStock.Name = "lblLowStock";
            this.lblLowStock.Size = new System.Drawing.Size(176, 38);
            this.lblLowStock.TabIndex = 0;
            this.lblLowStock.Text = "Low Stock Items:";
            this.lblLowStock.Click += new System.EventHandler(this.lblSales_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(248, 28);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpFrom.TabIndex = 1;
            this.dtpFrom.Value = new System.DateTime(2025, 9, 18, 2, 22, 53, 0);
            this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(526, 28);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 20);
            this.dtpTo.TabIndex = 2;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFrom.Location = new System.Drawing.Point(197, 26);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(48, 21);
            this.lblFrom.TabIndex = 3;
            this.lblFrom.Text = "From";
            this.lblFrom.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblTo.Location = new System.Drawing.Point(496, 28);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(27, 21);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = "To";
            this.lblTo.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.Location = new System.Drawing.Point(752, 26);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(75, 23);
            this.btnFilter.TabIndex = 4;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // ProductID
            // 
            this.ProductID.DataPropertyName = "ProductID";
            this.ProductID.HeaderText = "ID";
            this.ProductID.Name = "ProductID";
            this.ProductID.Visible = false;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "Name";
            this.ProductName.FillWeight = 40F;
            this.ProductName.HeaderText = "Name";
            this.ProductName.Name = "ProductName";
            // 
            // ProductBrand
            // 
            this.ProductBrand.DataPropertyName = "Brand";
            this.ProductBrand.FillWeight = 20F;
            this.ProductBrand.HeaderText = "Brand";
            this.ProductBrand.Name = "ProductBrand";
            // 
            // ProductCategory
            // 
            this.ProductCategory.DataPropertyName = "Category";
            this.ProductCategory.FillWeight = 20F;
            this.ProductCategory.HeaderText = "Category";
            this.ProductCategory.Name = "ProductCategory";
            // 
            // ProductQuantity
            // 
            this.ProductQuantity.DataPropertyName = "Quantity";
            this.ProductQuantity.FillWeight = 10F;
            this.ProductQuantity.HeaderText = "Stock";
            this.ProductQuantity.Name = "ProductQuantity";
            // 
            // lblChart
            // 
            this.lblChart.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblChart.Location = new System.Drawing.Point(15, 11);
            this.lblChart.Name = "lblChart";
            this.lblChart.Size = new System.Drawing.Size(269, 38);
            this.lblChart.TabIndex = 0;
            this.lblChart.Text = "Sales Trend Weekly";
            this.lblChart.Click += new System.EventHandler(this.lblSales_Click);
            // 
            // ReportsUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.btnFilter);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.dtpTo);
            this.Controls.Add(this.dtpFrom);
            this.Controls.Add(this.pnlLowStockItem);
            this.Controls.Add(this.pnlGrossProfit);
            this.Controls.Add(this.pnlGraphSales);
            this.Controls.Add(this.pnlGrossSales);
            this.Name = "ReportsUC";
            this.Size = new System.Drawing.Size(1029, 744);
            this.Load += new System.EventHandler(this.ReportsUC_Load);
            this.pnlGrossSales.ResumeLayout(false);
            this.pnlGraphSales.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartSales)).EndInit();
            this.pnlGrossProfit.ResumeLayout(false);
            this.pnlLowStockItem.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLowStockItem)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlGrossSales;
        private System.Windows.Forms.Panel pnlGraphSales;
        private System.Windows.Forms.Panel pnlGrossProfit;
        private System.Windows.Forms.Panel pnlLowStockItem;
        private System.Windows.Forms.Label lblSales;
        private System.Windows.Forms.Label lblSalesAmount;
        private System.Windows.Forms.Label lblProfitAmount;
        private System.Windows.Forms.Label lblProfit;
        private System.Windows.Forms.DataGridView dgvLowStockItem;
        private System.Windows.Forms.Label lblLowStock;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSales;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductBrand;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductQuantity;
        private System.Windows.Forms.Label lblChart;
    }
}
