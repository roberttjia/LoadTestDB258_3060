using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class ExpenseTransportationC
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
        
    }
}