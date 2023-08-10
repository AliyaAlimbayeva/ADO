using System.Data;
using Microsoft.Data.SqlClient;
namespace ProductLib
{
    public class ConnectedMod
    {
        private readonly SqlConnection _connection;

        public ConnectedMod(SqlConnection connection)
        {
            _connection = connection;
        }
        public bool CreateProduct(Product product)
        {
            using SqlCommand command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "INSERT INTO Products (Name, Description, Weight, Length, Height, Width) VALUES (@Name, @Description, @Weight, @Length, @Height, @Width)";
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Weight", product.Weight);
            command.Parameters.AddWithValue("@Length", product.Length);
            command.Parameters.AddWithValue("@Height", product.Height);
            command.Parameters.AddWithValue("@Width", product.Width);
            return command.ExecuteNonQuery() == 1;
        }
        public bool UpdateProduct(Product product)
        {
            using var command = new SqlCommand();
            command.Connection = _connection;

            command.CommandText = "UPDATE Products SET Name = @Name, Description = @Description, " +
                           "Weight = @Weight, Length = @Length, Height = @Height, Width = @Width " +
                           "WHERE Id = @Id";
            command.Parameters.AddWithValue("@Id", product.Id);
            command.Parameters.AddWithValue("@Name", product.Name);
            command.Parameters.AddWithValue("@Description", product.Description);
            command.Parameters.AddWithValue("@Weight", product.Weight);
            command.Parameters.AddWithValue("@Length", product.Length);
            command.Parameters.AddWithValue("@Height", product.Height);
            command.Parameters.AddWithValue("@Width", product.Width);
            return command.ExecuteNonQuery() == 1;
        }
        public bool DeleteProduct(int productId)
        {
            using var command = new SqlCommand();
            command.Connection = _connection;
            command.CommandText = "DELETE FROM products WHERE id = @id";
            command.Parameters.AddWithValue("@Id", productId);
            return command.ExecuteNonQuery() == 1;
        }
        public Product GetProductById(int productId)
        {
            var products = new List<Product>();

            using var command = new SqlCommand();
            command.Connection = _connection;

            command.CommandText = "SELECT * FROM products WHERE id = @id;";
            command.Parameters.AddWithValue("@Id", productId);
            var reader = command.ExecuteReader();

            while (reader.Read())
                products.Add(new Product
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Name = reader["name"].ToString(),
                    Description = reader["description"].ToString(),
                    Weight = Convert.ToDecimal(reader["Weight"]),
                    Length = Convert.ToDecimal(reader["Length"]),
                    Height = Convert.ToDecimal(reader["Height"]),
                    Width = Convert.ToDecimal(reader["Width"])
                });
            reader.Close();
            return products.SingleOrDefault();
        }
        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();

            using SqlCommand command = new SqlCommand("SELECT * FROM Products;", _connection);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Weight = Convert.ToDecimal(reader["Weight"]),
                        Length = Convert.ToDecimal(reader["Length"]),
                        Height = Convert.ToDecimal(reader["Height"]),
                        Width = Convert.ToDecimal(reader["Width"])
                    });
                }
            }
            reader.Close();
            return products;
        }
        public bool CreateOrder(Order order)
        {
            using SqlCommand command = new SqlCommand("INSERT INTO Orders (Status, CreatedDate, UpdatedDate, ProductId) " +
                                                      "VALUES (@Status, @CreatedDate, @UpdatedDate, @ProductId)", _connection);
            command.Parameters.AddWithValue("@Status", order.Status);
            command.Parameters.AddWithValue("@CreatedDate", order.CreatedDate);
            command.Parameters.AddWithValue("@UpdatedDate", order.UpdatedDate);
            command.Parameters.AddWithValue("@ProductId", order.ProductId);
            return command.ExecuteNonQuery() == 1;
        }
        public Order GetOrderById(int orderId)
        {
            var orders = new List<Order>();
            using SqlCommand command = new SqlCommand("SELECT * FROM Orders WHERE Id = @Id", _connection);
            command.Parameters.AddWithValue("@Id", orderId);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
                orders.Add(new Order
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Status = reader["status"].ToString(),
                    CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                    UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]),
                    ProductId = Convert.ToInt32(reader["ProductId"])
                });
            reader.Close();
            return orders.SingleOrDefault();
        }
        public bool UpdateOrder(Order order)
        {
            using SqlCommand command = new SqlCommand("UPDATE Orders SET Status = @Status, CreatedDate = @CreatedDate, " +
                                                    "UpdatedDate = @UpdatedDate, ProductId = @ProductId WHERE Id = @Id", _connection);
            command.Parameters.AddWithValue("@Id", order.Id);
            command.Parameters.AddWithValue("@Status", order.Status);
            command.Parameters.AddWithValue("@CreatedDate", order.CreatedDate);
            command.Parameters.AddWithValue("@UpdatedDate", order.UpdatedDate);
            command.Parameters.AddWithValue("@ProductId", order.ProductId);
            return command.ExecuteNonQuery() == 1;
        }
        public bool DeleteOrder(int orderId)
        {
            using SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE Id = @Id", _connection);
            command.Parameters.AddWithValue("@Id", orderId);
            return command.ExecuteNonQuery() == 1;
        }
        public List<Order> GetAllOrders()
        {
            List<Order> orders = new List<Order>();
            using SqlCommand command = new SqlCommand("spGetAllOrders", _connection);
            command.CommandType = CommandType.StoredProcedure;
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Order order = new Order
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Status = reader["Status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]),
                        ProductId = Convert.ToInt32(reader["ProductId"])
                    };
                    orders.Add(order);
                }
            }
            reader.Close();
            return orders;
        }
        public List<Order> GetOrdersByFilter(
            int? month = null, 
            int? year = null,
            string status = null, 
            int? productId = null)
        {
            List<Order> orders = new List<Order>();
            using SqlCommand command = new SqlCommand("spGetOrdersByFilter", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Month", month != null ? (object)month : DBNull.Value);
            command.Parameters.AddWithValue("@Year", year != null ? (object)year : DBNull.Value);
            command.Parameters.AddWithValue("@Status", !string.IsNullOrEmpty(status) ? (object)status : DBNull.Value);
            command.Parameters.AddWithValue("@ProductId", productId != null ? (object)productId : DBNull.Value);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Order order = new Order
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Status = reader["Status"].ToString(),
                        CreatedDate = Convert.ToDateTime(reader["CreatedDate"]),
                        UpdatedDate = Convert.ToDateTime(reader["UpdatedDate"]),
                        ProductId = Convert.ToInt32(reader["ProductId"])
                    };
                    orders.Add(order);
                }
            }
            reader.Close();
            return orders;
        }
        public bool DeleteOrders(
            int? year = null,
            int? month = null,
            string status = null,
            int? productId = null)
        {
            using SqlCommand command = new SqlCommand("spDeleteOrders", _connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Month", month != null ? (object)month : DBNull.Value);
            command.Parameters.AddWithValue("@Year", year != null ? (object)year : DBNull.Value);
            command.Parameters.AddWithValue("@Status", !string.IsNullOrEmpty(status) ? (object)status : DBNull.Value);
            command.Parameters.AddWithValue("@Product", productId != null ? (object)productId : DBNull.Value);
            return command.ExecuteNonQuery() == 1;
        }
        public void ClearAllData()
        {
            using var cmd = new SqlCommand();
            cmd.Connection = _connection;

            cmd.CommandText = "EXEC spClearDb;";
            cmd.ExecuteNonQuery();
        }
    }
}
