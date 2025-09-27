using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class ServiceE
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal ServiceFee { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Status { get; set; } = string.Empty;
        
        public virtual CustomerE Customer { get; set; } = null!;
        public virtual RegistrationE Registration { get; set; } = null!;
        public virtual ICollection<ServiceListE> ServiceList { get; set; }
        public virtual ICollection<ServicePaymentListE> ServicePaymentList { get; set; }
    }
}