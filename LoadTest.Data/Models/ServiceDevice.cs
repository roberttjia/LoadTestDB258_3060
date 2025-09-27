using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class ServiceDevice
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal ServiceFee { get; set; }
        public DateTime ServiceDate { get; set; }
        public string Status { get; set; } = string.Empty;
        
    }
}