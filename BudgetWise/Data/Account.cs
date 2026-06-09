// File: Data/Account.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Data
{
    public enum AccountType
    {
        Checking,
        Savings,
        Cash,
        Retirement,
        CreditCard,
        Loan
    }

    public class Account
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public AccountType Type { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal StartingBalance { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentBalance { get; set; } = 0;

        // True for rows created by the demo-data seeder, so they can be bulk-removed.
        public bool IsDemo { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        public virtual ICollection<UserTransaction> Transactions { get; set; } = new List<UserTransaction>();
    }
}

