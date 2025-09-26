using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using TechMart.Common;
using TechMart.Models;
using TechMart.Services; 

namespace TechMart.UserControls
{
    public partial class NewSaleControl : UserControl
    {
        private List<CartItem> cartItems = new List<CartItem>();
        private BindingSource cartBindingSource = new BindingSource();
        private ProductService productService = new ProductService();
        private List<Product> allProducts = new List<Product>();
        private SaleService saleService = new SaleService();
        private int CurrentUserId => (LoggedInUser.UserID != 0) ? LoggedInUser.UserID : 2;

       
        private Product selectedProduct = null;

        public NewSaleControl()
        {
            InitializeComponent();

            SetupCartGrid();
            SetupProductGrid();
            LoadProducts();

            nudDiscount.ValueChanged += (s, e) => UpdateSummary();
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private void SetupCartGrid()
        {
            dgvCart.AutoGenerateColumns = false;
            dgvCart.Columns.Clear();

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Product",
                HeaderText = "Product",
                DataPropertyName = "ProductName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Price",
                HeaderText = "Price",
                DataPropertyName = "Price",
                ReadOnly = true
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Qty",
                HeaderText = "Qty",
                DataPropertyName = "Quantity"
            });

            dgvCart.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "Subtotal",
                HeaderText = "Subtotal",
                DataPropertyName = "Subtotal",
                ReadOnly = true
            });

            cartBindingSource.DataSource = cartItems;
            dgvCart.DataSource = cartBindingSource;

            dgvCart.CellEndEdit += dgvCart_CellEndEdit;

           
            dgvCart.ClearSelection();
        }

        private void SetupProductGrid()
        {
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false; 
            dgvProducts.CellClick += dgvProducts_CellClick;
        }

       
        private void LoadProducts()
        {
            try
            {
                allProducts = productService.GetAllProducts();
                dgvProducts.DataSource = new List<Product>(allProducts);

                dgvProducts.BeginInvoke(new Action(() =>
                {
                    dgvProducts.ClearSelection();
                    dgvProducts.CurrentCell = null;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void LoadFilteredProducts(string searchText)
        {
            try
            {
                if (allProducts == null || allProducts.Count == 0)
                    allProducts = productService.GetAllProducts();

                List<Product> filtered;
                if (string.IsNullOrWhiteSpace(searchText))
                    filtered = new List<Product>(allProducts);
                else
                    filtered = allProducts
                        .Where(p => !string.IsNullOrEmpty(p.Name) &&
                                    p.Name.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        .ToList();

                dgvProducts.DataSource = filtered;

                ClearProductSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering products: " + ex.Message);
            }
        }

        private void ClearProductSelection()
        {
            dgvProducts.ClearSelection();
            dgvProducts.CurrentCell = null;
            selectedProduct = null;
        }
        
        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            selectedProduct = dgvProducts.Rows[e.RowIndex].DataBoundItem as Product;
        }
     
        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Please select a product first.", "Add to Cart", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CartItem existing = cartItems.FirstOrDefault(c => c.ProductID == selectedProduct.ProductID);
            if (existing != null)
                existing.Quantity++;
            else
                cartItems.Add(new CartItem()
                {
                    ProductID = selectedProduct.ProductID,
                    ProductName = selectedProduct.Name,
                    Price = selectedProduct.Price,
                    Quantity = 1
                });

            RefreshCart();
            ClearProductSelection();
        }

        private void dgvCart_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvCart.Columns["Qty"].Index)
            {
                int qty;
                if (!int.TryParse(dgvCart.Rows[e.RowIndex].Cells["Qty"].Value?.ToString(), out qty) || qty < 1)
                    qty = 1;

                dgvCart.Rows[e.RowIndex].Cells["Qty"].Value = qty;
                cartItems[e.RowIndex].Quantity = qty;
            }

            RefreshCart();
        }
    
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {

            if (dgvCart.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to remove.", "Remove Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<CartItem> itemsToRemove = new List<CartItem>();
            foreach (DataGridViewRow row in dgvCart.SelectedRows)
            {
                string name = row.Cells["Product"].Value.ToString();
                CartItem item = cartItems.FirstOrDefault(c => c.ProductName == name);
                if (item != null)
                    itemsToRemove.Add(item);
            }

            foreach (var item in itemsToRemove)
                cartItems.Remove(item);

            RefreshCart();
        }
     
        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if (!cartItems.Any())
            {
                MessageBox.Show("Cart is empty!", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

         
            decimal subtotal = cartItems.Sum(c => c.Subtotal);
            decimal discountPercent = nudDiscount.Value;
            decimal discountAmount = subtotal * discountPercent / 100m;
            decimal total = Math.Max(0, subtotal - discountAmount);

            try
            {
                int userId = CurrentUserId; 
                int saleId = saleService.AddSale(userId, cartItems, subtotal, discountPercent, total);

                MessageBox.Show($"Checkout successful!\nSale ID: {saleId}\nTotal: {total:N2}", "Checkout", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cartItems.Clear();
                RefreshCart();

               
                nudDiscount.Value = 0;

               
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during checkout: " + ex.Message, "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
  
        private void RefreshCart()
        {
            cartBindingSource.ResetBindings(false);
            UpdateSummary();
            dgvCart.ClearSelection();
        }

        private void UpdateSummary()
        {
            decimal subtotal = cartItems.Sum(c => c.Subtotal);

            decimal discountPercent = nudDiscount.Value;
            discountPercent = Math.Max(0, Math.Min(100, discountPercent));

            decimal discountAmount = subtotal * discountPercent / 100;
            decimal total = Math.Max(0, subtotal - discountAmount);

            lblSubtotal.Text = $"Subtotal: {subtotal:N2}";
            lblDiscount.Text = $"Discount: {discountPercent}%";
            lblTotal.Text = $"Total: {total:N2}";
        }

       
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredProducts(txtSearch.Text.Trim());
        }

      
    }
}

        
    




