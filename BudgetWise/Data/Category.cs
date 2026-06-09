// File: Data/Category.cs
// This class represents a budget category that can be used in transactions and budgets.
// Categories can be default/common ones or custom user-created ones.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Data
{
    public class Category
    {
        // Primary key for the category row.
        public int Id { get; set; }

        // Foreign key back to the Identity user (AspNetUsers table).
        // Nullable for default categories that are available to all users.
        public string? UserId { get; set; }

        // Category name (e.g., "Groceries", "Rent", "Entertainment").
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        // Indicates if this is a default/common category (true) or custom user category (false).
        public bool IsDefault { get; set; }

        // Indicates if this is an income category (true) or expense category (false).
        public bool IsIncome { get; set; }

        // True for custom categories created by the demo-data seeder, so they can be
        // bulk-removed. Shared default categories (UserId == null) are never demo rows.
        public bool IsDemo { get; set; }

        // Navigation property back to the user (nullable for default categories).
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}






