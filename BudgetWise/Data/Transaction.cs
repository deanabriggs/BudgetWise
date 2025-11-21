using System.ComponentModel.DataAnnotations.Schema;
using BudgetWise.Data;
using Microsoft.AspNetCore.Identity;

public class UserTransaction
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public double Amount { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }
}