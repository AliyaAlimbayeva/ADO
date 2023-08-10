using Microsoft.Data.SqlClient;
using System.Data;

namespace ProductLib
{
    public class DisConnectedMod
    {
        private readonly DataSet _dataSet;

        private readonly SqlDataAdapter _productDataAdapter;
        private readonly DataTable _productTable;

        private readonly SqlDataAdapter _orderDataAdapter;
        private readonly DataTable _orderTable;


        public DisConnectedMod(SqlConnection connection)
        {
            _dataSet = new DataSet();
            _productDataAdapter = new SqlDataAdapter("select * from Products;", connection);

            _productDataAdapter.FillSchema(_dataSet, SchemaType.Source, "Products");
            _productDataAdapter.Fill(_dataSet, "Products");
            _dataSet.Tables["Products"].Columns["Id"].AutoIncrement = true;
            _dataSet.Tables["Products"].Columns["Id"].AutoIncrementSeed = 1;
            _productTable = _dataSet.Tables["Products"];

            _orderDataAdapter = new SqlDataAdapter("select * from orders;", connection);
            _orderDataAdapter.FillSchema(_dataSet, SchemaType.Source, "Orders");
            _orderDataAdapter.Fill(_dataSet, "Orders");
            _dataSet.Tables["Orders"].Columns["Id"].AutoIncrement = true;
            _dataSet.Tables["Orders"].Columns["Id"].AutoIncrementSeed = 1;
            _orderTable = _dataSet.Tables["Orders"];

            var ordersProductsFk = new ForeignKeyConstraint("FK_Orders_Products",
                _dataSet.Tables["Products"].Columns["Id"],
                _dataSet.Tables["Orders"].Columns["ProductId"]);
            ordersProductsFk.DeleteRule = Rule.None;
            _dataSet.Tables["Orders"].Constraints.Add(ordersProductsFk);

            _ = new SqlCommandBuilder(_orderDataAdapter);
            _ = new SqlCommandBuilder(_productDataAdapter);
        }

        public bool CreateProduct(Product product)
        {
            var newProduct = _dataSet.Tables["Products"].NewRow();

            newProduct["Name"] = product.Name;
            newProduct["Description"] = product.Description;
            newProduct["Weight"] = product.Weight;
            newProduct["Height"] = product.Height;
            newProduct["Width"] = product.Width;
            newProduct["Length"] = product.Length;

            _productTable.Rows.Add(newProduct);

            return _productDataAdapter.Update(_dataSet, "Products") == 1;
        }
        public Product GetProductById(int id)
        {
            var row = _productTable
                .AsEnumerable()
                .FirstOrDefault(x => x.Field<int>("Id") == id);

            Product product = new Product
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                Weight = Convert.ToDecimal(row["Weight"]),
                Height = Convert.ToDecimal(row["Height"]),
                Width = Convert.ToDecimal(row["Width"]),
                Length = Convert.ToDecimal(row["Length"])
            };

            return product;
        }
        public bool UpdateProduct(Product product)
        {
            var productToUpdate = _productTable.Rows.Find(product.Id);
            productToUpdate["name"] = product.Name;
            productToUpdate["description"] = product.Description;
            productToUpdate["weight"] = product.Weight;
            productToUpdate["height"] = product.Height;
            productToUpdate["width"] = product.Width;
            productToUpdate["length"] = product.Length;
            return _productDataAdapter.Update(_dataSet, "Products") == 1;
        }
        public bool DeleteProduct(int id)
        {
            _productTable.Rows.Find(id).Delete();
            return _productDataAdapter.Update(_dataSet, "Products") == 1;
        }
        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();
            foreach (DataRow row in _productTable.Rows)
            {
                products.Add(new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    Weight = Convert.ToDecimal(row["Weight"]),
                    Height = Convert.ToDecimal(row["Height"]),
                    Width = Convert.ToDecimal(row["Width"]),
                    Length = Convert.ToDecimal(row["Length"])
                });
            }
            return products;
        }
        public bool CreateOrder(Order order)
        {
            var newOrder = _dataSet.Tables["Orders"].NewRow();

            newOrder["Status"] = order.Status;
            newOrder["CreatedDate"] = order.CreatedDate;
            newOrder["UpdatedDate"] = order.UpdatedDate;
            newOrder["ProductId"] = order.ProductId;

            _orderTable.Rows.Add(newOrder);

            return _orderDataAdapter.Update(_dataSet, "Orders") == 1;
        }

        public Order GetOrderById(int id)
        {
            var row = _orderTable
                .AsEnumerable()
                .FirstOrDefault(x => x.Field<int>("Id") == id);

            var order = new Order
            {
                Id = Convert.ToInt32(row["Id"]),
                Status = row["Status"].ToString(),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                ProductId = Convert.ToInt32(row["ProductId"])
            };

            return order;
        }

        public bool UpdateOrder(Order order)
        {
            var orderToUpdate = _orderTable.Rows.Find(order.Id);

            orderToUpdate["Status"] = order.Status;
            orderToUpdate["CreatedDate"] = order.CreatedDate;
            orderToUpdate["UpdatedDate"] = order.UpdatedDate;
            orderToUpdate["ProductId"] = order.ProductId;

            return _orderDataAdapter.Update(_dataSet, "Orders") == 1;
        }

        public bool DeleteOrder(int id)
        {
            _orderTable.Rows.Find(id).Delete();

            return _orderDataAdapter.Update(_dataSet, "Orders") == 1;
        }

        public List<Order> GetAllOrders()
        {
            var orders = new List<Order>();
            foreach (DataRow row in _orderTable.Rows)
            {
                orders.Add(new Order
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Status = row["Status"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                    ProductId = Convert.ToInt32(row["ProductId"])
                });
            }
            return orders;
        }

        public List<Order> GetFilteredOrders(
            int? year = null,
            int? month = null,
            string status = null,
            int? product = null)
        {
            var orders = new List<Order>();
            var rows = _orderTable.AsEnumerable();
            if (year != null)
            {
                rows = rows.Where(x =>
                    x.Field<DateTime>("CreatedDate").Year == year);
            }
            if (month != null)
            {
                rows = rows.Where(x =>
                    x.Field<DateTime>("CreatedDate").Month == month);
            }
            if (status != null)
            {
                rows = rows.Where(x =>
                    x.Field<string>("Status") == status.ToString());
            }
            if (product != null)
            {
                rows = rows.Where(x =>
                    x.Field<int>("ProductId") == product);
            }
            foreach (DataRow row in rows)
            {
                orders.Add(new Order
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Status = row["Status"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    UpdatedDate = Convert.ToDateTime(row["UpdatedDate"]),
                    ProductId = Convert.ToInt32(row["ProductId"])
                });
            }
            return orders;
        }

        public bool DeleteOrders(
            int? year = null,
            int? month = null,
            string status = null,
            int? product = null)
        {
            var rows = _orderTable.AsEnumerable();
            if (year != null)
            {
                rows = rows.Where(x =>
                    x.Field<DateTime>("CreatedDate").Year == year);
            }
            if (month != null)
            {
                rows = rows.Where(x =>
                    x.Field<DateTime>("CreatedDate").Month == month);
            }
            if (!string.IsNullOrEmpty(status))
            {
                rows = rows.Where(x =>
                    x.Field<string>("Status") == status.ToString());
            }
            if (product != null)
            {
                rows = rows.Where(x =>
                    x.Field<int>("ProductId") == product);
            }
            var numOfRowsToDelete = rows.Count();
            foreach (var row in rows)
            {
                row.Delete();
            }
            return _orderDataAdapter.Update(_dataSet, "Orders") == numOfRowsToDelete;
        }
    }
}
