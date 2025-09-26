
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using TechMart.Data;
using TechMart.Models;

namespace TechMart.Services
{
    public class SaleService
    {
     
        public int AddSale(int userId, List<CartItem> cartItems, decimal subtotal, decimal discountPercent, decimal total)
        {
            if (cartItems == null || cartItems.Count == 0)
                throw new ArgumentException("No items to save for sale.");

            int newSaleId = 0;

            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                using (var transaction = conn.BeginTransaction())
                {
                    try
                    {
                       
                        string insertSaleSql = @"
                            INSERT INTO Sales (SaleDate, UserID, Subtotal, Discount, Total)
                            OUTPUT INSERTED.SaleID
                            VALUES (@SaleDate, @UserID, @Subtotal, @Discount, @Total)";
                        using (var cmdSale = new SqlCommand(insertSaleSql, conn, transaction))
                        {
                            cmdSale.Parameters.AddWithValue("@SaleDate", DateTime.Now);
                            cmdSale.Parameters.AddWithValue("@UserID", userId);
                            cmdSale.Parameters.AddWithValue("@Subtotal", subtotal);
                            cmdSale.Parameters.AddWithValue("@Discount", discountPercent);
                            cmdSale.Parameters.AddWithValue("@Total", total);

                            newSaleId = (int)cmdSale.ExecuteScalar();
                        }

                        
                        foreach (var item in cartItems)
                        {
                          
                            int available = 0;
                            using (var cmdCheck = new SqlCommand("SELECT Quantity FROM Products WHERE ProductID = @ProductID", conn, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@ProductID", item.ProductID);
                                var result = cmdCheck.ExecuteScalar();
                                if (result == null) throw new Exception($"Product (ID {item.ProductID}) not found.");
                                available = Convert.ToInt32(result);
                            }

                            if (available < item.Quantity)
                                throw new Exception($"Insufficient stock for '{item.ProductName}'. Available: {available}, Requested: {item.Quantity}.");

                            string insertItemSql = @"
                                INSERT INTO SaleItems (SaleID, ProductID, Quantity, Price, Subtotal)
                                VALUES (@SaleID, @ProductID, @Quantity, @Price, @Subtotal)";
                            using (var cmdItem = new SqlCommand(insertItemSql, conn, transaction))
                            {
                                cmdItem.Parameters.AddWithValue("@SaleID", newSaleId);
                                cmdItem.Parameters.AddWithValue("@ProductID", item.ProductID);
                                cmdItem.Parameters.AddWithValue("@Quantity", item.Quantity);
                                cmdItem.Parameters.AddWithValue("@Price", item.Price);
                                cmdItem.Parameters.AddWithValue("@Subtotal", item.Subtotal);
                                cmdItem.ExecuteNonQuery();
                            }

                            using (var cmdUpdate = new SqlCommand("UPDATE Products SET Quantity = Quantity - @Qty WHERE ProductID = @ProductID", conn, transaction))
                            {
                                cmdUpdate.Parameters.AddWithValue("@Qty", item.Quantity);
                                cmdUpdate.Parameters.AddWithValue("@ProductID", item.ProductID);
                                cmdUpdate.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return newSaleId;
                    }
                    catch
                    {
                        try { transaction.Rollback(); } catch { /* ignore rollback errors */ }
                        throw;
                    }
                }
            }
        }
      
        public DataTable GetSales(int? userId, DateTime from, DateTime to)
        {
            var dt = new DataTable();
            using (var conn = DbConnection.GetConnection())
            {
                conn.Open();
                string query = userId.HasValue
                    ? "SELECT SaleID, SaleDate, Subtotal, Discount, Total, (SELECT Name FROM Users WHERE UserID = Sales.UserID) AS Salesman FROM Sales WHERE UserID=@UserID AND SaleDate BETWEEN @From AND @To ORDER BY SaleDate DESC"
                    : "SELECT SaleID, SaleDate, Subtotal, Discount, Total, (SELECT Name FROM Users WHERE UserID = Sales.UserID) AS Salesman FROM Sales WHERE SaleDate BETWEEN @From AND @To ORDER BY SaleDate DESC";

                using (var cmd = new SqlCommand(query, conn))
                {
                    if (userId.HasValue)
                        cmd.Parameters.AddWithValue("@UserID", userId.Value);
                    cmd.Parameters.AddWithValue("@From", from);
                    cmd.Parameters.AddWithValue("@To", to);

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt ?? new DataTable(); // never null
        }

        
        public DataTable GetSaleItemsBySaleId(int saleId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string sql = @"
                    SELECT si.SaleItemID, si.ProductID, p.Name AS ProductName, si.Quantity, si.Price, si.Subtotal
                    FROM SaleItems si
                    LEFT JOIN Products p ON si.ProductID = p.ProductID
                    WHERE si.SaleID = @SaleID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@SaleID", saleId);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
        public DataTable GetSaleByDate(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();
                string sql = @"
                        SELECT s.SaleID, s.SaleDate, s.Subtotal, s.Discount, s.Total, u.UserEmail AS Salesman
                        FROM Sales s
                        INNER JOIN Users u ON s.UserID = u.UserID
                        WHERE CAST(s.SaleDate AS DATE) Between @fromDate And @toDate";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Date);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public DataTable GetWeeklySalesSummary(DateTime weekStartDate)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = DbConnection.GetConnection())
            {
                conn.Open();

                DateTime fromDate = weekStartDate.Date;
                DateTime toDate = weekStartDate.Date.AddDays(7);

                string sql = @"
                        SELECT 
                            CAST(s.SaleDate AS date) AS [Day],
                            SUM(s.Total) AS DailySales
                        FROM Sales s
                        WHERE s.SaleDate >= @FromDate AND s.SaleDate < @ToDate
                        GROUP BY CAST(s.SaleDate AS date)
                        ORDER BY [Day];";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
