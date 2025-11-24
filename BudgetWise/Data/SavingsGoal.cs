using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetWise.Data;

public class SavingsGoal
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    // Optional navigation property
    public User? User { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TargetAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal CurrentAmount { get; set; }

    public DateTime? TargetDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    // ⭐ REQUIRED by SavingsGoals.razor
    [NotMapped]
    public decimal ProgressPercent =>
        TargetAmount == 0 ? 0 :
        Math.Min(100m, (CurrentAmount / TargetAmount) * 100m);
}
