using System.ComponentModel.DataAnnotations;

namespace StoreStock.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; } = "No description available";

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    
    }
}
