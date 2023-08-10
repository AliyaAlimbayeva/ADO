namespace ProductLib.Tests
{
    public partial class Tests : TestBase
    {
        [Fact]
        public void Should_Add_Product_Using_Connected_Model()
        {
            var product = DataSource.Products[0];

            connectedMod.CreateProduct(product).Should().BeTrue();

            connectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Get_All_Products_Using_Connected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];

            connectedMod.CreateProduct(product1);
            connectedMod.CreateProduct(product2);

            connectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product1, product2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Update_Product_Using_Connected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];
            product2.Id = 1;

            connectedMod.CreateProduct(product1);
            connectedMod.UpdateProduct(product2);

            connectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product2 });
        }

        [Fact]
        public void Should_Get_Product_By_Id_Using_Connected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];

            connectedMod.CreateProduct(product1);
            connectedMod.CreateProduct(product2);

            connectedMod.GetProductById(1).Should()
                .BeEquivalentTo(product1, config =>
                    config.Excluding(p => p.Id));

            connectedMod.GetProductById(2).Should()
                .BeEquivalentTo(product2, config =>
                    config.Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Delete_Product_Using_Connected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];

            connectedMod.CreateProduct(product1);
            connectedMod.CreateProduct(product2);

            connectedMod.DeleteProduct(1).Should().BeTrue();

            connectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Add_Order_Using_Connected_Model()
        {
            CreateProductsConnectedModel();

            var order = DataSource.Orders[0];

            connectedMod.CreateOrder(order).Should().BeTrue();

            connectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Get_All_Orders_Using_Connected_Model()
        {
            CreateProductsConnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];

            connectedMod.CreateOrder(order1);
            connectedMod.CreateOrder(order2);

            connectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order1, order2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Update_Order_Using_Connected_Model()
        {
            CreateProductsConnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];
            connectedMod.CreateOrder(order1);
            connectedMod.CreateOrder(order2);

            order1.Id = 1;
            order1.Status = "InProgress";
            order1.UpdatedDate = order1.UpdatedDate.AddDays(10);

            connectedMod.UpdateOrder(order1);

            connectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order1, order2 },
                    config => config
                        .Excluding(o => o.Id));
        }

        [Fact]
        public void Should_Get_Order_By_Id_Using_Connected_Model()
        {
            CreateProductsConnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];
            connectedMod.CreateOrder(order1);
            connectedMod.CreateOrder(order2);

            connectedMod.GetOrderById(1).Should()
                .BeEquivalentTo(order1, config =>
                    config.Excluding(p => p.Id));

            connectedMod.GetOrderById(2).Should()
                .BeEquivalentTo(order2, config =>
                    config.Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Delete_Order_Using_Connected_Model()
        {
            CreateProductsConnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];
            connectedMod.CreateOrder(order1);
            connectedMod.CreateOrder(order2);

            connectedMod.DeleteOrder(1).Should().BeTrue();

            connectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Theory]
        [MemberData(nameof(GetFilteredOrdersTestData))]
        public void Should_Get_Filtered_Orders_Using_Connected_Model(
            int? year,
            int? month,
            string? status,
            int? product,
            List<Order> expected)
        {
            CreateProductsConnectedModel();

            connectedMod.CreateOrder(DataSource.Orders[0]);
            connectedMod.CreateOrder(DataSource.Orders[1]);
            connectedMod.CreateOrder(DataSource.Orders[2]);
            connectedMod.CreateOrder(DataSource.Orders[3]);

            var result = connectedMod.GetOrdersByFilter(
                year: year,
                month: month,
                status: status,
                productId: product
                );

            result.Should()
                .BeEquivalentTo(expected, config => config
                            .Excluding(p => p.Id));
        }

        [Theory]
        [MemberData(nameof(DeleteOrdersTestData))]
        public void Should_Delete_Orders_Using_Connected_Model(
            int? year,
            int? month,
            string? status,
            int? product,
            List<Order> expected)
        {
            CreateProductsConnectedModel();

            connectedMod.CreateOrder(DataSource.Orders[0]);
            connectedMod.CreateOrder(DataSource.Orders[1]);
            connectedMod.CreateOrder(DataSource.Orders[2]);
            connectedMod.CreateOrder(DataSource.Orders[3]);

            connectedMod.DeleteOrders(
                year: year,
                month: month,
                status: status,
                productId: product
            );

            var result = connectedMod.GetAllOrders();

            result.Should()
                .BeEquivalentTo(expected, config => config
                    .Excluding(p => p.Id));
        }

        private void CreateProductsConnectedModel()
        {
            connectedMod.CreateProduct(DataSource.Products[0]);
            connectedMod.CreateProduct(DataSource.Products[1]);
        }

        private void CreateProductsDisconnectedModel()
        {
            disconnectedMod.CreateProduct(DataSource.Products[0]);
            disconnectedMod.CreateProduct(DataSource.Products[1]);
        }

        public static IEnumerable<object[]> GetFilteredOrdersTestData()
        {
            var data = new List<object[]>
        {
            new object[] { null, null, null, null, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[1],
                DataSource.Orders[2],
                DataSource.Orders[3]
            }},
            new object[] { 2023, null, null, null, new List<Order>
            {
                DataSource.Orders[1]
            }},
            new object[] { null, 5, null, null, new List<Order>
            {
                DataSource.Orders[2]
            }},
            new object[] { null, null, "Loading", null, new List<Order>
            {
                DataSource.Orders[3]
            }},
            new object[] { null, null, null, 2, new List<Order>
            {
                DataSource.Orders[1],
                DataSource.Orders[3]
            }},
            new object[] { 2024, 1, null, null, new List<Order>
            {
                DataSource.Orders[3]
            }},
            new object[] { null, 1, "Loading", null, new List<Order>
            {
                DataSource.Orders[3]
            }},
            new object[] { 2023, 1, "Done", 2, new List<Order>
            {
                DataSource.Orders[1]
            }}
        };

            return data;
        }

        public static IEnumerable<object[]> DeleteOrdersTestData()
        {
            var data = new List<object[]>
        {
            new object[] { null, null, null, null, new List<Order>()},
            new object[] { 2023, null, null, null, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[2],
                DataSource.Orders[3]
            }},
            new object[] { null, 5, null, null, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[1],
                DataSource.Orders[3]
            }},
            new object[] { null, null, "Loading", null, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[1],
                DataSource.Orders[2]
            }},
            new object[] { null, null, null, 2, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[2]
            }},
            new object[] { 2024, 1, null, null, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[1],
                DataSource.Orders[2]
            }},
            new object[] { null, 1, "Loading", null, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[1],
                DataSource.Orders[2]
            }},
            new object[] { 2023, 1, "Done", 2, new List<Order>
            {
                DataSource.Orders[0],
                DataSource.Orders[2],
                DataSource.Orders[3]
            }}
        };

            return data;
        }
    }
    
}