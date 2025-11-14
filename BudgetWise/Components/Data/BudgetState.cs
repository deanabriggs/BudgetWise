using System.Collections.Generic;

namespace BudgetWise.Data;

public class BudgetState
{
    public List<Transaction> Transactions { get; set; } = new List<Transaction>();
}