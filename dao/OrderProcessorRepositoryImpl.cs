﻿using E_Commerce_App.Entity;
using E_Commerce_App.Exception;
using E_Commerce_App.userException;
using E_Commerce_App.Utility;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_App.dao
{
    public class OrderProcessorRepositoryImpl : IOrderProcessorRepository
    {
        
        Products product = new Products();
        Customers customer = new Customers();
        Orders order = new Orders();
        OrderItems orderItems = new OrderItems();

        public string connectionString;
        SqlCommand cmd = null;

        public OrderProcessorRepositoryImpl()
        {
            connectionString = DBConnection.GetConnectionString();
            cmd= new SqlCommand();
        }

        public bool AddToCart(Customers customer, Products product, int quantity)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    cmd.CommandText = "insert into cart values(@customerId,@productId,@quantity)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@productId", product.ProductId);
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nProduct added to cart Successfully............\n");
                        return true;
                    }
                }
            }
            catch(System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return false;
        }

        public bool CreateCustomer(Customers customer)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    cmd.CommandText = "insert into customers values(@name,@email,@password)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@name", customer.Name);
                    cmd.Parameters.AddWithValue("@email", customer.Email);
                    cmd.Parameters.AddWithValue("@password", customer.Password);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nCustomer Added Successfully............\n");
                        return true;
                    }
                }
            }
            catch(System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return false;
        }

        public bool CreateProduct(Products product)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    cmd.CommandText = "Insert into products values(@name,@price,@description,@quantity)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@name", product.Name);
                    cmd.Parameters.AddWithValue("@price", product.Price);
                    cmd.Parameters.AddWithValue("@description", product.Description);
                    cmd.Parameters.AddWithValue("@quantity", product.StockQuantity);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nProduct Added Successfully............\n");
                        return true;
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
                        
            return false;
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    cmd.CommandText = "delete from customers where customer_id=@customerId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nCustomer Deleted Successfully...................\n");
                        return true;
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return false;
        }

        public bool DeleteProduct(int productId)
        {
            bool productExist = ProductExists(productId);
            if(productExist) {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        cmd.CommandText = "delete from products where product_id=@productId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@productId", productId);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("\nProduct Deleted Successfully............\n");
                            return true;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
            
            return false;
        }

        public List<Products> GetAllFromCart(Customers customer)
        {
            List<Products> products = new List<Products>();
            
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    cmd.CommandText = "select products.name,products.price,products.stock_quantity from products " +
                        "join cart on \r\nproducts.product_id=cart.product_id where cart.customer_id=@customerId";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Products product = new Products();
                        product.Name = (string)reader["name"];
                        product.Price = (float)(decimal)reader["price"];
                        product.StockQuantity = (int)reader["stock_quantity"];
                        products.Add(product);
                    }
                    sqlConnection.Close();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            

            return products;
        }

        public List<Products> GetOrdersByCustomer(int customerId)
        {
            List<Products> customerProduct = new List<Products>();
            bool customerExists = CustomerExists(customerId);

            if(customerExists)
            {
                try
                {
                    using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                    {
                        cmd.CommandText = "select products.* from products join order_items on products.product_id=" +
                            "order_items.product_id\r\njoin orders on orders.order_id=order_items.order_id join customers " +
                            "on customers.customer_id=\r\norders.customer_id where customers.customer_id=@customerId";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@customerId", customerId);
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Products product = new Products();
                            product.ProductId = (int)reader["product_id"];
                            product.Name = (string)reader["name"];
                            product.Price = (float)(decimal)reader["price"];
                            product.StockQuantity = (int)reader["stock_quantity"];
                            product.Description = (string)reader["description"];
                            product.StockQuantity = (int)reader["stock_quantity"];
                            customerProduct.Add(product);
                        }
                        sqlConnection.Close();
                    }
                }
                catch (System.Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                Console.WriteLine("Customer does not exists.");
            }
            
            


            return customerProduct;
        }

        public bool PlaceOrder(Customers customer, Dictionary<Products, int> dict, string shippingAddress)
        {

            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    double price = 0;
                    int quantity = 0;
                    int productId = 0;
                    int orderId = 0;

                    
                    foreach (var items in dict)
                    {
                        orderId = 0;
                        productId = items.Key.ProductId;
                        quantity = items.Value;

                        bool productExist = ProductExists(productId);
                        if(productExist)
                        {
                            cmd.CommandText = "select price*@quantity from products where product_id=@productId";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@quantity", quantity);
                            cmd.Parameters.AddWithValue("@productId", productId);
                            cmd.Connection = sqlConnection;
                            sqlConnection.Open();
                            price = Convert.ToSingle(cmd.ExecuteScalar());
                            sqlConnection.Close();

                            cmd.CommandText = "insert into orders OUTPUT INSERTED.order_id values(@customerId,@orderDate,@totalPrice,@shippingAddress)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                            cmd.Parameters.AddWithValue("@orderDate", DateTime.Now.ToString("yyyy-MM-dd"));
                            cmd.Parameters.AddWithValue("@totalPrice", price);
                            cmd.Parameters.AddWithValue("@shippingAddress", shippingAddress);
                            cmd.Connection = sqlConnection;
                            sqlConnection.Open();
                            orderId = Convert.ToInt32(cmd.ExecuteScalar());
                            sqlConnection.Close();


                            cmd.CommandText = "insert into order_items values(@orderId,@productId,@quantity)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@orderId", orderId);
                            cmd.Parameters.AddWithValue("@productId", productId);
                            cmd.Parameters.AddWithValue("@quantity", quantity);
                            cmd.Connection = sqlConnection;
                            sqlConnection.Open();
                            cmd.ExecuteNonQuery();                            
                        }                        
                    }
                    if (orderId > 0)
                    {
                        Console.WriteLine($"Order Placed successfully...........");
                        return true;
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return false;
        }

        public bool RemoveFromCart(Customers customers, Products product)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "delete from cart where customer_id=@customerId and product_id=@productId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@customerId", customers.CustomerId);
                cmd.Parameters.AddWithValue("@productId", product.ProductId);
                cmd.Connection = sqlConnection;
                sqlConnection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    Console.WriteLine("Product removed from cart.");
                    return true;
                }
            }
            return false;
        }


        public bool CustomerExists(int custId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    cmd.CommandText = "select * from customers where customer_id=@customerId";
                    cmd.Parameters.AddWithValue("@customerId", custId);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return true;
                    }
                    throw new ProductNotFoundException($"CustomerId {custId} does not exists");
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool ProductExists(int prodId)
        {
            try
            {
                using(SqlConnection sqlConnection=new SqlConnection(connectionString))
                {
                    cmd.CommandText = "select * from products where product_id=@productId";
                    cmd.Parameters.AddWithValue("@productId", prodId);
                    cmd.Connection=sqlConnection;
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read())
                    {
                        return true;
                    }
                    throw new ProductNotFoundException($"ProductId {prodId} does not exists");
                }
            }
            catch(System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        public bool OrderExists(int orderId)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    cmd.CommandText = "select * from orders where order_id=@productId";
                    cmd.Parameters.AddWithValue("@orderId", orderId);
                    cmd.Connection = sqlConnection;
                    sqlConnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return true;
                    }
                    throw new ProductNotFoundException($"OrderId {orderId} does not exists");
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }
    }
}
