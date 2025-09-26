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
    public partial class ProductManagement : UserControl
    {
        ProductService productService = new ProductService();
        private int? selectedProductId = null;
        private List<Product> allProducts = new List<Product>();

        public ProductManagement()
        {
            InitializeComponent();
            LoadProducts();
          
            txtSearch.TextChanged += txtSearch_TextChanged;
        }
        private void LoadProducts()
        {
            try
            {
                
                allProducts = productService.GetAllProducts();

                dgvProducts.DataSource = new List<Product>(allProducts);
                dgvProducts.ClearSelection();
                dgvProducts.CurrentCell = null;
                selectedProductId = null;
              
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

                
                int caret = txtSearch.SelectionStart;

                
                dgvProducts.DataSource = list;
                dgvProducts.ClearSelection();
                dgvProducts.CurrentCell = null;
                selectedProductId = null;

                
                BeginInvoke(new Action(() =>
                {
                    txtSearch.Focus();
                   
                    if (caret <= txtSearch.Text.Length) txtSearch.SelectionStart = caret;
                    else txtSearch.SelectionStart = txtSearch.Text.Length;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering products: " + ex.Message);
            }
        }


        private void ClearFields()
        {
            txtName.Clear();
            txtCategory.Text = "";
            txtBrand.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
            txtWarranty.Clear();
            selectedProductId = null; 

            dgvProducts.ClearSelection();
        }

        private bool ValidateInputs(out decimal price, out int quantity)
        {
            price = 0m;
            quantity = 0;

            // 1) Name required
            string name = txtName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Product name is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            // 2) Price required + numeric + non-negative
            string priceText = txtPrice.Text.Trim();
            if (string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Price is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }
            if (!decimal.TryParse(priceText, out price) || price < 0m)
            {
                MessageBox.Show("Please enter a valid non-negative price (example: 1200.50).", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            // 3) Quantity required + integer + non-negative
            string qtyText = txtQuantity.Text.Trim();
            if (string.IsNullOrEmpty(qtyText))
            {
                MessageBox.Show("Quantity is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }
            if (!int.TryParse(qtyText, out quantity) || quantity < 0)
            {
                MessageBox.Show("Please enter a valid non-negative integer for quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs(out decimal price, out int quantity)) return;

                Product p = new Product
                {
                    Name = txtName.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Brand = txtBrand.Text.Trim(),
                    Price = price,
                    Quantity = quantity,
                    Warranty = txtWarranty.Text.Trim()
                };
                int newProductId = productService.AddProduct(p);
                LoadProducts();

           
                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    if ((int)row.Cells["ProductID"].Value == newProductId)
                    {
                        row.Selected = true;
                        dgvProducts.CurrentCell = row.Cells[0];
                        dgvProducts_CellClick(this, new DataGridViewCellEventArgs(0, row.Index)); 
                        break;
                    }
                }


                MessageBox.Show("Product added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductId == null)
                {
                    MessageBox.Show("Please select a product to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidateInputs(out decimal price, out int quantity)) return;

                Product p = new Product
                {
                    ProductID = selectedProductId.Value,
                    Name = txtName.Text.Trim(),
                    Category = txtCategory.Text.Trim(),
                    Brand = txtBrand.Text.Trim(),
                    Price = price,
                    Quantity = quantity,
                    Warranty = txtWarranty.Text.Trim()
                };

                productService.UpdateProduct(p);
                LoadProducts();
                ClearFields();

                MessageBox.Show("Product updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedProductId == null)
                {
                    MessageBox.Show("Please select a product to delete.");
                    return;
                }

                DialogResult result = MessageBox.Show(
                    "Are you sure you want to delete this product?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    productService.DeleteProduct(selectedProductId.Value);
                    LoadProducts();
                    ClearFields();

                    MessageBox.Show("Product Deleted Successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

       

        private void ProductManagement_Load(object sender, EventArgs e)
        {
            dgvProducts.ClearSelection();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadFilteredProducts(txtSearch.Text.Trim());
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    selectedProductId = (int)dgvProducts.Rows[e.RowIndex].Cells["ProductID"].Value;

                    txtName.Text = dgvProducts.Rows[e.RowIndex].Cells["Name"].Value.ToString();
                    txtCategory.Text = dgvProducts.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                    txtBrand.Text = dgvProducts.Rows[e.RowIndex].Cells["Brand"].Value.ToString();
                    txtPrice.Text = dgvProducts.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                    txtQuantity.Text = dgvProducts.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                    txtWarranty.Text = dgvProducts.Rows[e.RowIndex].Cells["Warranty"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error selecting product: " + ex.Message);
            }
        }

        private void btnInstructions_Click(object sender, EventArgs e)
        {
            string instructions =
        "📌 How to Use Product Management Panel:\n\n" +
        "1️. Fill in the product details on the left.\n" +
        "   - Product Name: Enter product name.\n" +
        "   - Category: Select or type category.\n" +
        "   - Brand: Enter brand name.\n" +
        "   - Price: Enter numeric value only.\n" +
        "   - Quantity: Enter how many units are available.\n" +
        "   - Warranty: Enter warranty in months.\n\n" +
        "2️. Click 'Add' to save a new product.\n" +
        "3️. Select a product from the list to Update or Delete it.\n" +
        "4️. Use 'Reset' to clear all input fields.\n" +
        "5️. Search bar lets you filter products by name.\n\n" +
        "✅ Follow these steps to keep your product list clean and updated.";

            MessageBox.Show(instructions, "Instructions", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
