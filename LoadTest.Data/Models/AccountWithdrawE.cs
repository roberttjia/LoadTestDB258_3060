using System.ComponentModel.DataAnnotations;

namespace LoadTest.Data.Models
{
    public partial class AccountWithdrawE
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AccountType { get; set; } = string.Empty;
        
        public virtual ICollection<ExpenseE> Expense { get; set; }
        public virtual ICollection<SellingExpenseE> SellingExpense { get; set; }
        public virtual ICollection<SellingPaymentReturnRecordE> SellingPaymentReturnRecord { get; set; }
        public virtual ICollection<PurchasePaymentReturnRecordE> PurchasePaymentReturnRecord { get; set; }
    }
}