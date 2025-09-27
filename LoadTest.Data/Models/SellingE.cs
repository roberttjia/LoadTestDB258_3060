using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class SellingE
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        
        public virtual ICollection<SellingListE> SellingList { get; set; }
        public virtual ICollection<SellingPaymentListE> SellingPaymentList { get; set; }
        public virtual ICollection<SellingExpenseE> SellingExpense { get; set; }
        public virtual ICollection<WarrantyE> Warranty { get; set; }
        public virtual ICollection<ProductLogE> ProductLog { get; set; }
    }
}