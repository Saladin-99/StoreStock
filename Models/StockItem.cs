using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreStock.Models
{
    public class StockItem
    {
        // Composite primary key
        [Key]
        [Column(Order = 1)]
        public int StoreId { get; set; } // Foreign key property for Store
        [Key]
        [Column(Order = 2)]
        public int ProductId { get; set; } // Foreign key property for Product

        // Quantity of the product in stock at the store
        public int Quantity { get; set; }

        // Navigation properties
        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
