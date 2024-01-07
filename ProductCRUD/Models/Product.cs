using System.ComponentModel.DataAnnotations;

namespace ProductCRUD.Models
{
    // Models/Product.cs
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public decimal UnitPrice { get; set; }
    }

}

