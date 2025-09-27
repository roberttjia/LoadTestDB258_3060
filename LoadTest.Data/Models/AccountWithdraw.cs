using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class AccountWithdraw
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountType { get; set; } = string.Empty;
        
    }
}