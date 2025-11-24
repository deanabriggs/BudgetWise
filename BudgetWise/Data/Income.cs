using System;
using System.ComponentModel.DataAnnotations;

namespace BudgetWise.Data
{
    public class Income
    {
        [Key]
        public int Id { get; set; }

        // User who owns this income entry
        public string UserId { get; set; } = string.Empty;

        // Date the income was received
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        // What the income was for
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;

        // The amount of money received
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
    }
}
