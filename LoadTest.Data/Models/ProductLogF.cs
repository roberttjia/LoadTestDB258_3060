using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class ProductLogF
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        
    }
}