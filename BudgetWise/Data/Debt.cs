using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Data;

public class Debt
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    // Navigation property required by EF
    public User? User { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal OriginalAmount { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentBalance { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlyPayment { get; set; }

    public decimal? InterestRate { get; set; }

    public DateTime? StartDate { get; set; }

    // ✔ Used in Razor page
    [NotMapped]
    public decimal ProgressPercent =>
        OriginalAmount == 0 ? 0 :
        Math.Min(100m, (1 - (CurrentBalance / OriginalAmount)) * 100m);
}
