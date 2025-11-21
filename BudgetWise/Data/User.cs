using Microsoft.AspNetCore.Identity;

namespace BudgetWise.Data;

public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public virtual ICollection<UserTransaction> Transactions { get; set; } = new List<UserTransaction>();
}
