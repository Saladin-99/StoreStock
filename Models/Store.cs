using System.ComponentModel.DataAnnotations;

namespace StoreStock.Models
{
    public class Store
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Address { get; set; } = "Online Store";

        public List<StockItem>? StockItems { get; set; }
    }
}
