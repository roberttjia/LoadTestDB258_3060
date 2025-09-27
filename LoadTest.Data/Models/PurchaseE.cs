using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class PurchaseE
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = string.Empty;
        
        public virtual VendorE Vendor { get; set; } = null!;
        public virtual RegistrationE Registration { get; set; } = null!;
        public virtual ICollection<PurchaseListE> PurchaseList { get; set; }
        public virtual ICollection<PurchasePaymentListE> PurchasePaymentList { get; set; }
        public virtual ICollection<ProductLogE> ProductLog { get; set; }
        public virtual ICollection<SellingE> Selling { get; set; }
        public virtual ICollection<PurchasePaymentReturnRecordE> PurchasePaymentReturnRecord { get; set; }
    }
}