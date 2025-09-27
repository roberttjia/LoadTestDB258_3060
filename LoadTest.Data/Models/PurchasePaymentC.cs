using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class PurchasePaymentC
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        
    }
}