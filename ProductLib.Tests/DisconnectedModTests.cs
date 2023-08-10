namespace ProductLib.Tests
{
    public partial class Tests
    {
        [Fact]
        public void Should_Add_Product_Using_Disconnected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[0];

            disconnectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product>());

            disconnectedMod.CreateProduct(product1).Should().BeTrue();
            disconnectedMod.CreateProduct(product2).Should().BeTrue();

            disconnectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product1, product2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Get_All_Products_Using_Disconnected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];

            disconnectedMod.CreateProduct(product1);
            disconnectedMod.CreateProduct(product2);

            disconnectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product1, product2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Update_Product_Using_Disconnected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];
            product2.Id = 1;

            disconnectedMod.CreateProduct(product1);
            disconnectedMod.UpdateProduct(product2);

            disconnectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product2 });
        }

        [Fact]
        public void Should_Get_Product_By_Id_Using_Disconnected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];

            disconnectedMod.CreateProduct(product1);
            disconnectedMod.CreateProduct(product2);

            disconnectedMod.GetProductById(1).Should()
                .BeEquivalentTo(product1, config =>
                    config.Excluding(p => p.Id));

            disconnectedMod.GetProductById(2).Should()
                .BeEquivalentTo(product2, config =>
                    config.Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Delete_Product_Using_Disconnected_Model()
        {
            var product1 = DataSource.Products[0];
            var product2 = DataSource.Products[1];

            disconnectedMod.CreateProduct(product1);
            disconnectedMod.CreateProduct(product2);

            disconnectedMod.DeleteProduct(1).Should().BeTrue();

            disconnectedMod.GetAllProducts().Should()
                .BeEquivalentTo(new List<Product> { product2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Add_Order_Using_Disconnected_Model()
        {
            CreateProductsDisconnectedModel();

            var order = DataSource.Orders[0];

            disconnectedMod.CreateOrder(order).Should().BeTrue();

            disconnectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Get_All_Orders_Using_Disconnected_Model()
        {
            CreateProductsDisconnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];

            disconnectedMod.CreateOrder(order1);
            disconnectedMod.CreateOrder(order2);

            disconnectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order1, order2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Update_Order_Using_Disconnected_Model()
        {
            CreateProductsDisconnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];
            disconnectedMod.CreateOrder(order1);
            disconnectedMod.CreateOrder(order2);

            order1.Id = 1;
            order1.Status = "InProgress";
            order1.UpdatedDate = order1.UpdatedDate.AddDays(10);

            disconnectedMod.UpdateOrder(order1);

            disconnectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order1, order2 },
                    config => config
                        .Excluding(o => o.Id));
        }

        [Fact]
        public void Should_Get_Order_By_Id_Using_Disconnected_Model()
        {
            CreateProductsDisconnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];
            disconnectedMod.CreateOrder(order1);
            disconnectedMod.CreateOrder(order2);

            disconnectedMod.GetOrderById(1).Should()
                .BeEquivalentTo(order1, config =>
                    config.Excluding(p => p.Id));

            disconnectedMod.GetOrderById(2).Should()
                .BeEquivalentTo(order2, config =>
                    config.Excluding(p => p.Id));
        }

        [Fact]
        public void Should_Delete_Order_Using_Disconnected_Model()
        {
            CreateProductsDisconnectedModel();

            var order1 = DataSource.Orders[0];
            var order2 = DataSource.Orders[1];

            disconnectedMod.CreateOrder(order1);
            disconnectedMod.CreateOrder(order2);

            disconnectedMod.DeleteOrder(1).Should().BeTrue();

            disconnectedMod.GetAllOrders().Should()
                .BeEquivalentTo(new List<Order> { order2 },
                    config => config
                        .Excluding(p => p.Id));
        }

        [Theory]
        [MemberData(nameof(GetFilteredOrdersTestData))]
        public void Should_Get_Filtered_Orders_Using_Disconnected_Model(
            int? year,
            int? month,
            string? status,
            int? product,
            List<Order> expected)
        {
            CreateProductsDisconnectedModel();

            disconnectedMod.CreateOrder(DataSource.Orders[0]);
            disconnectedMod.CreateOrder(DataSource.Orders[1]);
            disconnectedMod.CreateOrder(DataSource.Orders[2]);
            disconnectedMod.CreateOrder(DataSource.Orders[3]);

            var result = disconnectedMod.GetFilteredOrders(
                year: year,
                month: month,
                status: status,
                product: product
                );

            result.Should()
                .BeEquivalentTo(expected, config => config
                         .Excluding(p => p.Id));
        }

        [Theory]
        [MemberData(nameof(DeleteOrdersTestData))]
        public void Should_Delete_Orders_Using_Disconnected_Model(
            int? year,
            int? month,
            string? status,
            int? product,
            List<Order> expected)
        {
            CreateProductsDisconnectedModel();

            disconnectedMod.CreateOrder(DataSource.Orders[0]);
            disconnectedMod.CreateOrder(DataSource.Orders[1]);
            disconnectedMod.CreateOrder(DataSource.Orders[2]);
            disconnectedMod.CreateOrder(DataSource.Orders[3]);

            disconnectedMod.DeleteOrders(
                year: year,
                month: month,
                status: status,
                product: product
            ).Should().BeTrue();

            var result = disconnectedMod.GetAllOrders();

            result.Should()
                .BeEquivalentTo(expected, config => config
                    .Excluding(p => p.Id));
        }
    }
}
