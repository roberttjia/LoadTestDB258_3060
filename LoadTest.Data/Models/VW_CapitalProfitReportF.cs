using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class VW_CapitalProfitReportF
    {
        public int Id { get; set; }
        public string ReportData { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime ReportDate { get; set; }
        
    }
}