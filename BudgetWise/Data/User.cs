using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BudgetWise.Data
{
    // Our application user. Inherits from IdentityUser so we still get
    // all the normal Identity fields (Email, PasswordHash, etc).
    public class User : IdentityUser
    {
        // Optional names (these may already exist in your project)
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // Navigation properties so EF Core knows how things relate

        // All expense transactions for this user
        public List<UserTransaction> Transactions { get; set; } = new();

        // All budgets for this user
        public List<Budget> Budgets { get; set; } = new();
    }
}
