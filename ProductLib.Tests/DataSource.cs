using ProductLib;
using System;
using System.Collections.Generic;

namespace ProductLib.Tests
{
    internal static class DataSource
    {
        public static readonly List<Product> Products = new List<Product>
        {
            new Product
            {
                Name = "Lenovo",
                Description = "Home Computer, Notebook",
                Weight = 10.5m,
                Length = 20m,
                Height = 5m,
                Width = 8m
            },
            new Product
            {
                Name = "Samsung",
                Description = "TV-set",
                Weight = 17m,
                Length = 500m,
                Height = 200m,
                Width = 80m
            },
            new Product
            {
                Name = "Apple IPhone",
                Description = "Smartphone",
                Weight = 215m,
                Length = 10m,
                Height = 75m,
                Width = 176m
            }
        };

        public static readonly List<Order> Orders = new List<Order>
        {
            new Order
            {
                Status = "NotStarted",
                CreatedDate = new DateTime(2022, 1, 7),
                UpdatedDate = new DateTime(2022, 12, 7),
                ProductId = 1
            },
            new Order
            {
                Status = "Done",
                CreatedDate = new DateTime(2023, 1, 17),
                UpdatedDate = new DateTime(2023, 2, 25),
                ProductId = 2
            },
            new Order
            {
                Status = "Arrived",
                CreatedDate = new DateTime(2025, 5, 3),
                UpdatedDate = new DateTime(2025, 8, 7),
                ProductId = 1
            },
            new Order
            {
                Status = "Loading",
                CreatedDate = new DateTime(2024, 1, 17),
                UpdatedDate = new DateTime(2024, 6, 25),
                ProductId = 2
            }
        };
    }
}