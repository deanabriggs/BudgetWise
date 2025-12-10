// File: Data/UserTransaction.cs
// This class represents a single money movement for a specific user.
// We store both income and expenses here.
// Convention in this app:
//   - Positive Amount = income
//   - Negative Amount = expense

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Data
{
    public class UserTransaction
    {
        // Primary key for the transaction row.
        public int Id { get; set; }

        // Foreign key back to the Identity User table (AspNetUsers).
        // Every transaction belongs to exactly one user.
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Foreign key to Account - every transaction is linked to an account
        [Required]
        public int AccountId { get; set; }

        // The calendar date for this transaction (no time of day needed).
        [Required]
        [DataType(DataType.Date)]
        public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Today);

        // A short description so the user remembers what this was.
        [Required]
        [StringLength(100)]
        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;

        // Money amount for this transaction.
        // We use decimal for currency (more precise than double).
        // Positive = income, Negative = expense.
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "-1000000", "1000000",
            ErrorMessage = "Amount must be between -1,000,000 and 1,000,000.")]
        public decimal Amount { get; set; }

        // Navigation property back to the User entity.
        // "virtual" allows EF Core to create proxies if needed.
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        // Navigation property to Account
        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; } = null!;
    }
}
