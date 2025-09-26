using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TechMart.Models;
using TechMart.Services;

namespace TechMart.UserControls
{
    public partial class InventoryManagement : UserControl
    {
        private ProductService productService = new ProductService();
        private List<Product> allProducts = new List<Product>();
        private int? selectedProductId = null;
        private const int LowStockThreshold = 5; 
        

        public InventoryManagement()
        {
            InitializeComponent();

            this.txtSearchInventory.TextChanged += txtSearchInventory_TextChanged;
            this.dgvInventory.CellClick += dgvInventory_CellClick;
            this.btnRefresh.Click += btnRefresh_Click;
            this.btnLowStock.Click += btnLowStock_Click;

            this.dgvInventory.DataBindingComplete += dgvInventory_DataBindingComplete;

            LoadInventory();
        }

        

        private void LoadInventory()
        {
            try
            {
                allProducts = productService.GetAllProducts();
                BindInventory(allProducts);
                lblInventoryStatus.Text = $"Total products: {allProducts.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BindInventory(List<Product> list)
        {
            dgvInventory.DataSource = null;
            dgvInventory.DataSource = list;
            dgvInventory.ClearSelection();
            dgvInventory.CurrentCell = null;
            selectedProductId = null;

        

            lblInventoryStatus.Text = $"Showing: {dgvInventory.Rows.Count} products (Low stock threshold: {LowStockThreshold})";
        }
        private void txtSearchInventory_TextChanged(object sender, EventArgs e)
        {
            string term = txtSearchInventory.Text.Trim();
            LoadFilteredInventory(term);
        }
        
        

        private void LoadFilteredInventory(string searchText)
        {
            try
            {
                if (allProducts == null || allProducts.Count == 0) allProducts = productService.GetAllProducts();

                List<Product> list;
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    list = new List<Product>(allProducts);
                }
                else
                {
                    list = allProducts
                        .Where(p => !string.IsNullOrEmpty(p.Name) &&
                                    p.Name.StartsWith(searchText, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                
                int caret = txtSearchInventory.SelectionStart;

                BindInventory(list);

                BeginInvoke(new Action(() =>
                {
                    txtSearchInventory.Focus();
                    if (caret <= txtSearchInventory.Text.Length) txtSearchInventory.SelectionStart = caret;
                    else txtSearchInventory.SelectionStart = txtSearchInventory.Text.Length;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering inventory: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvInventory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    var row = dgvInventory.Rows[e.RowIndex];
                    selectedProductId = Convert.ToInt32(row.Cells["ProductID"].Value);
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadInventory();
            ClearSearchAndSelection();
        }
        

        private void btnLowStock_Click(object sender, EventArgs e)
        {
            try
            {
                var low = (allProducts ?? productService.GetAllProducts())
                    .Where(p => p.Quantity <= LowStockThreshold)
                    .ToList();
                BindInventory(low);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing low stock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdateStock_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductId == null)
                {
                    MessageBox.Show("Please select a product row to update stock.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                
                var prod = allProducts.FirstOrDefault(p => p.ProductID == selectedProductId.Value);
                if (prod == null)
                {
                    MessageBox.Show("Selected product not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int currentQty = prod.Quantity;
                int newQty = AskForQuantity(currentQty);
                if (newQty < 0) return; 

                
                productService.UpdateStock(prod.ProductID, newQty);

                
                LoadInventory();
                MessageBox.Show("Stock updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvInventory.ClearSelection();
                dgvInventory.CurrentCell = null;
                selectedProductId = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating stock: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        
        private int AskForQuantity(int current)
        {
            using (Form f = new Form())
            {
                f.StartPosition = FormStartPosition.CenterParent;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.MinimizeBox = false;
                f.MaximizeBox = false;
                f.ShowIcon = false;
                f.ShowInTaskbar = false;
                f.Text = "Update Stock";

                Label lbl = new Label() { Left = 10, Top = 15, Text = $"Current quantity: {current}", AutoSize = true };
                NumericUpDown nud = new NumericUpDown() { Left = 10, Top = 40, Width = 200, Minimum = 0, Maximum = 1000000, Value = current };
                Button ok = new Button() { Text = "OK", Left = 10, Width = 80, Top = 80, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 120, Width = 80, Top = 80, DialogResult = DialogResult.Cancel };

                f.Controls.Add(lbl);
                f.Controls.Add(nud);
                f.Controls.Add(ok);
                f.Controls.Add(cancel);
                f.AcceptButton = ok;
                f.CancelButton = cancel;
                var dr = f.ShowDialog();

                if (dr == DialogResult.OK) return Convert.ToInt32(nud.Value);
                return -1;
            }
        }

        private void ClearSearchAndSelection()
        {
            txtSearchInventory.Text = "";
            dgvInventory.ClearSelection();
            dgvInventory.CurrentCell = null;
            selectedProductId = null;
        }
        private void ApplyLowStockStyling()
        {
            foreach (DataGridViewRow row in dgvInventory.Rows)
            {
                try
                {
                    int qty = Convert.ToInt32(row.Cells["Quantity"].Value);
                    if (qty <= LowStockThreshold)
                    {
                        row.DefaultCellStyle.BackColor = Color.LightCoral;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = Color.White;
                        row.DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                catch
                {
                    // Ignore invalid rows
                }
            }
        }

        private void dgvInventory_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ApplyLowStockStyling();
        }
    }
}
