using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class CustomerPhoneE
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        
        public virtual ICollection<ServiceE> Service { get; set; }
        public virtual ICollection<ServiceDeviceE> ServiceDevice { get; set; }
        public virtual ICollection<CustomerPhoneE> CustomerPhone { get; set; }
    }
}