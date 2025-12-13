// File: Data/Budget.cs
// This class represents a monthly budget limit for a specific user
// and category (for example: "Groceries, April 2025, $400").
//
// We will use this to:
//  - Show budget vs actual spending on the dashboard.
//  - Display friendly alerts when the user is near or over the limit.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Data
{
    public class Budget
    {
        // Primary key for the budget row.
        public int Id { get; set; }

        // Foreign key back to the Identity user (AspNetUsers table).
        [Required]
        public string UserId { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; } = null!;
        // Monthly spending limit for this category.
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(typeof(decimal), "0", "1000000",
            ErrorMessage = "Budget amount must be a positive value.")]
        public decimal MonthlyLimit { get; set; }

        // Month number (1–12). This makes it easy to filter by month.
        [Range(1, 12)]
        public int Month { get; set; }

        // Year (e.g., 2025). Combined with Month, gives us a calendar period.
        public int Year { get; set; }

        // Navigation property back to the user.
        public virtual User User { get; set; } = null!;
    }
}
