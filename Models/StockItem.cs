using System.ComponentModel.DataAnnotations;

namespace StoreStock.Models
{
    public class StockItem
    {
        // Composite primary key
        public int StoreId { get; set; } // Foreign key property for Store
        public int ProductId { get; set; } // Foreign key property for Product

        // Quantity of the product in stock at the store
        public int Quantity { get; set; }

        // Navigation properties
        public Store Store { get; set; }
        public Product Product { get; set; }
    }
}
