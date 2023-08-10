using System;
namespace ProductLib
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int ProductId { get; set; }
    }
}
