// File: Data/Source.cs
// This class represents a source (merchant, vendor, employer, etc.) that can be reused by a user.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Data
{
    public class Source
    {
        // Primary key for the source row.
        public int Id { get; set; }

        // Foreign key back to the Identity User table (AspNetUsers).
        // Every source belongs to exactly one user.
        [Required]
        public string UserId { get; set; } = string.Empty;

        // The name of the source (merchant, vendor, employer, etc.)
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        // Navigation property back to the User entity.
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
    }
}
