namespace BudgetWise.Data;

/// <summary>
/// Provides a centralized list of default categories that are available to all users.
/// These categories are automatically created when the first user registers or when
/// the Categories page is loaded for the first time.
/// </summary>
public static class DefaultCategories
{
    /// <summary>
    /// Gets the list of default expense category names that should be available to all users.
    /// </summary>
    public static IReadOnlyList<string> ExpenseCategoryNames { get; } = new List<string>
    {
        "Car Payments",
        "Clothing",
        "Credit Card Payments",
        "Dining Out",
        "Donations",
        "Education",
        "Entertainment",
        "Gas",
        "Groceries",
        "Healthcare",       
        "Insurance",
        "Personal Care",
        "Savings Contributions",
        "Subscriptions",
        "Rent/Mortgage",
        "Utilities"
    }.AsReadOnly();

    /// <summary>
    /// Gets the list of default income category names that should be available to all users.
    /// </summary>
    public static IReadOnlyList<string> IncomeCategoryNames { get; } = new List<string>
    {
        "Salary",
        "Side Hustle",
        "Bonus",
        "Investment Income",
        "Other Income"
    }.AsReadOnly();

    /// <summary>
    /// Gets all default category names (expenses and income combined).
    /// </summary>
    public static IReadOnlyList<string> CategoryNames { get; } = ExpenseCategoryNames
        .Concat(IncomeCategoryNames)
        .ToList()
        .AsReadOnly();
}
