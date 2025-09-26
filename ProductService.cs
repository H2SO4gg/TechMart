using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TechMart.Data;
using TechMart.Models;

namespace TechMart.Services
{
    public class ProductService
    {
        
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT ProductID, Name, Category, Brand, Price, Quantity, Warranty " +
                "FROM Products WHERE IsDeleted = 0";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID = (int)reader["ProductID"],
                            Name = reader["Name"].ToString(),
                            Category = reader["Category"].ToString(),
                            Brand = reader["Brand"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            Warranty = reader["Warranty"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error fetching products: " + ex.Message);
            }
            return products;
        }

       
        public int AddProduct(Product product)
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    if (conn == null)
                        throw new Exception("DbConnection.GetConnection() returned null.");

                    conn.Open();

                    string query = "INSERT INTO Products (Name, Category, Brand, Price, Quantity, Warranty, IsDeleted) " +
                                   "OUTPUT INSERTED.ProductID " +
                                   "VALUES (@Name,@Category,@Brand,@Price,@Quantity,@Warranty, 0)";

                    SqlCommand cmd = new SqlCommand(query, conn);

               
                    if (product.Name == null) throw new Exception("Product.Name is null!");
                    if (product.Category == null) throw new Exception("Product.Category is null!");
                    if (product.Brand == null) throw new Exception("Product.Brand is null!");
                    if (product.Warranty == null) throw new Exception("Product.Warranty is null!");

                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Category", product.Category);
                    cmd.Parameters.AddWithValue("@Brand", product.Brand);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@Warranty", product.Warranty);

                    object result = cmd.ExecuteScalar();

                    if (result == null)
                        throw new Exception("Insert failed.");

                    return (int)result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product: " + ex.Message);
            }
        }


      
        public void UpdateProduct(Product product)
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Products SET Name=@Name, Category=@Category, Brand=@Brand, " +
                                   "Price=@Price, Quantity=@Quantity, Warranty=@Warranty WHERE ProductID=@ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Category", product.Category);
                    cmd.Parameters.AddWithValue("@Brand", product.Brand);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
                    cmd.Parameters.AddWithValue("@Warranty", product.Warranty);
                    cmd.Parameters.AddWithValue("@ProductID", product.ProductID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating product: " + ex.Message);
            }
        }

        public void DeleteProduct(int productId)
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Products SET IsDeleted = 1 WHERE ProductID=@ProductID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting product: " + ex.Message);
            }
        }


    
        public List<Product> SearchProducts(string keyword)
        {
            List<Product> products = new List<Product>();
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT ProductID, Name, Category, Brand, Price, Quantity, Warranty " +
                "FROM Products WHERE IsDeleted = 0 AND Name LIKE @keyword";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            ProductID = (int)reader["ProductID"],
                            Name = reader["Name"].ToString(),
                            Category = reader["Category"].ToString(),
                            Brand = reader["Brand"].ToString(),
                            Price = (decimal)reader["Price"],
                            Quantity = (int)reader["Quantity"],
                            Warranty = reader["Warranty"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching products: " + ex.Message);
            }
            return products;
        }
       
        public void UpdateStock(int productId, int newQuantity)
        {
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "UPDATE Products SET Quantity = @Quantity WHERE ProductID = @ProductID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Quantity", newQuantity);
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        int affected = cmd.ExecuteNonQuery();
                        if (affected == 0)
                        {
                            throw new Exception("Update failed — product may not exist.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error updating stock: " + ex.Message);
            }
        }

        public DataTable getLowStockProductTable()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection conn = DbConnection.GetConnection())
                {
                    conn.Open();
                    string query = "SELECT [ProductID],[Name],[Category],[Brand],[Quantity] FROM Products where Quantity < 5";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching products: " + ex.Message);
            }
        }
    }
}
