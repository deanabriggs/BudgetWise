// File: Data/DemoDataSeeder.cs
// Seeds (and removes) a realistic, self-contained set of demo data for a single
// user so the app can be explored end-to-end without hand-entering anything.
// Every row it creates is tagged IsDemo = true so it can be bulk-removed cleanly.
//
// Conventions followed (see Accounts.razor / Home.razor):
//   - Transaction amounts are stored POSITIVE; Category.IsIncome decides direction.
//   - Asset accounts:  balance = Start + income - expenses
//   - Debt accounts:   balance = Start - income + expenses  (CreditCard, Loan)
//   - Income is modeled as income-category transactions, matching how the app's
//     UI actually records income (there is no separate income entry point).

using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Data;

public static class DemoDataSeeder
{
    /// <summary>
    /// Returns true if the user currently has any demo-tagged rows.
    /// </summary>
    public static async Task<bool> HasDemoDataAsync(ApplicationDbContext context, string userId)
    {
        return await context.Accounts.AnyAsync(a => a.UserId == userId && a.IsDemo)
            || await context.UserTransactions.AnyAsync(t => t.UserId == userId && t.IsDemo)
            || await context.Budgets.AnyAsync(b => b.UserId == userId && b.IsDemo);
    }

    /// <summary>
    /// Populates demo data for the given user. No-ops and returns false if the user
    /// already has demo data (or any accounts), so it is safe to call repeatedly.
    /// Data is created in dependency order: categories -> accounts -> transactions -> budgets.
    /// </summary>
    public static async Task<bool> SeedAsync(ApplicationDbContext context, string userId)
    {
        // Don't seed on top of existing data (demo or real).
        if (await context.Accounts.AnyAsync(a => a.UserId == userId))
        {
            return false;
        }

        // ---------------- Categories (shared defaults, never tagged demo) ----------------
        await EnsureDefaultCategoriesAsync(context);

        var categories = await context.Categories
            .Where(c => c.UserId == null || c.UserId == userId)
            .ToListAsync();

        // Resolve (or create, if a default was previously deleted) each category we need.
        async Task<Category> CategoryAsync(string name, bool isIncome)
        {
            var found = categories.FirstOrDefault(c => c.Name == name);
            if (found is not null) return found;

            found = new Category { Name = name, IsDefault = true, IsIncome = isIncome, UserId = null };
            context.Categories.Add(found);
            await context.SaveChangesAsync();
            categories.Add(found);
            return found;
        }

        var salary = await CategoryAsync("Salary", true);
        var sideHustle = await CategoryAsync("Side Hustle", true);
        var rent = await CategoryAsync("Rent/Mortgage", false);
        var utilities = await CategoryAsync("Utilities", false);
        var groceries = await CategoryAsync("Groceries", false);
        var gas = await CategoryAsync("Gas", false);
        var dining = await CategoryAsync("Dining Out", false);
        var subscriptions = await CategoryAsync("Subscriptions", false);
        var entertainment = await CategoryAsync("Entertainment", false);
        var healthcare = await CategoryAsync("Healthcare", false);
        var personalCare = await CategoryAsync("Personal Care", false);

        // ---------------- Accounts (one of every type) ----------------
        var checking = new Account { UserId = userId, Name = "Everyday Checking", Type = AccountType.Checking, StartingBalance = 1800m, IsDemo = true };
        var savings = new Account { UserId = userId, Name = "Emergency Savings", Type = AccountType.Savings, StartingBalance = 8200m, IsDemo = true };
        var cash = new Account { UserId = userId, Name = "Wallet Cash", Type = AccountType.Cash, StartingBalance = 150m, IsDemo = true };
        var retirement = new Account { UserId = userId, Name = "401(k)", Type = AccountType.Retirement, StartingBalance = 26500m, IsDemo = true };
        var creditCard = new Account { UserId = userId, Name = "Visa Credit Card", Type = AccountType.CreditCard, StartingBalance = 0m, IsDemo = true };
        var carLoan = new Account { UserId = userId, Name = "Car Loan", Type = AccountType.Loan, StartingBalance = 12400m, IsDemo = true };

        var accounts = new[] { checking, savings, cash, retirement, creditCard, carLoan };
        context.Accounts.AddRange(accounts);
        await context.SaveChangesAsync(); // assign account Ids (transactions reference them)

        // ---------------- Transactions ----------------
        // Dates are relative to today so the demo always spans the current and previous
        // month (drives the "vs last month" KPIs and the daily-trend chart).
        var today = DateTime.Today;
        var txns = new List<UserTransaction>();

        void Tx(Account account, int offsetDays, Category category, decimal amount, string source, string? description = null)
        {
            txns.Add(new UserTransaction
            {
                UserId = userId,
                AccountId = account.Id,
                CategoryId = category.Id,
                Date = DateOnly.FromDateTime(today.AddDays(offsetDays)),
                Amount = amount,
                Source = source,
                Description = description,
                IsDemo = true
            });
        }

        // This month: income
        Tx(checking, -8, salary, 2200m, "Acme Corp", "Bi-weekly paycheck");
        Tx(checking, -1, sideHustle, 240m, "Etsy", "Craft sales");

        // This month: expenses from checking
        Tx(checking, -8, rent, 1200m, "Landlord LLC", "Monthly rent");
        Tx(checking, -7, utilities, 142m, "City Utilities");
        Tx(checking, -6, groceries, 86m, "Whole Foods");
        Tx(checking, -5, gas, 48m, "Shell");
        Tx(checking, -4, dining, 32m, "Olive Garden");
        Tx(checking, -3, groceries, 73m, "Trader Joe's");
        Tx(checking, -2, subscriptions, 15.99m, "Netflix");
        Tx(checking, 0, entertainment, 28m, "AMC Theatres");

        // This month: credit card charges
        Tx(creditCard, -6, dining, 54m, "Chipotle");
        Tx(creditCard, -4, groceries, 98m, "Costco");
        Tx(creditCard, -2, gas, 41m, "Chevron");

        // Last month: income
        Tx(checking, -35, salary, 2200m, "Acme Corp", "Bi-weekly paycheck");
        Tx(checking, -21, salary, 2200m, "Acme Corp", "Bi-weekly paycheck");

        // Last month: expenses
        Tx(checking, -36, rent, 1200m, "Landlord LLC", "Monthly rent");
        Tx(checking, -34, utilities, 138m, "City Utilities");
        Tx(checking, -30, groceries, 112m, "Whole Foods");
        Tx(checking, -27, gas, 51m, "Shell");
        Tx(checking, -24, dining, 64m, "Local Bistro");
        Tx(checking, -22, healthcare, 45m, "CVS Pharmacy");
        Tx(checking, -20, personalCare, 38m, "Supercuts");

        context.UserTransactions.AddRange(txns);
        await context.SaveChangesAsync();

        // ---------------- Budgets (planned values for the current month) ----------------
        var budgetLimits = new (Category Category, decimal Limit)[]
        {
            (salary, 4400m),
            (rent, 1200m),
            (groceries, 400m),
            (gas, 150m),
            (dining, 120m),
            (utilities, 160m),
            (entertainment, 80m),
            (subscriptions, 30m),
        };

        foreach (var (category, limit) in budgetLimits)
        {
            context.Budgets.Add(new Budget
            {
                UserId = userId,
                CategoryId = category.Id,
                MonthlyLimit = limit,
                Month = today.Month,
                Year = today.Year,
                IsDemo = true
            });
        }
        await context.SaveChangesAsync();

        // ---------------- Recalculate account balances from seeded transactions ----------------
        var isIncomeById = categories.ToDictionary(c => c.Id, c => c.IsIncome);
        foreach (var account in accounts)
        {
            var accountTxns = txns.Where(t => t.AccountId == account.Id).ToList();
            var income = accountTxns.Where(t => isIncomeById[t.CategoryId]).Sum(t => t.Amount);
            var expense = accountTxns.Where(t => !isIncomeById[t.CategoryId]).Sum(t => t.Amount);

            bool isDebtAccount = account.Type is AccountType.CreditCard or AccountType.Loan;
            account.CurrentBalance = isDebtAccount
                ? account.StartingBalance - income + expense
                : account.StartingBalance + income - expense;
        }
        await context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Removes every demo-tagged row for the user. Deletes in FK-safe order:
    /// transactions (which restrict account deletion) -> budgets -> accounts ->
    /// any demo-tagged custom categories.
    /// </summary>
    public static async Task RemoveAsync(ApplicationDbContext context, string userId)
    {
        var demoTransactions = await context.UserTransactions
            .Where(t => t.UserId == userId && t.IsDemo)
            .ToListAsync();
        context.UserTransactions.RemoveRange(demoTransactions);
        await context.SaveChangesAsync();

        var demoBudgets = await context.Budgets
            .Where(b => b.UserId == userId && b.IsDemo)
            .ToListAsync();
        context.Budgets.RemoveRange(demoBudgets);
        await context.SaveChangesAsync();

        var demoAccounts = await context.Accounts
            .Where(a => a.UserId == userId && a.IsDemo)
            .ToListAsync();
        context.Accounts.RemoveRange(demoAccounts);
        await context.SaveChangesAsync();

        // Only ever removes user-owned demo categories; shared defaults (UserId == null)
        // are never tagged IsDemo and are therefore untouched.
        var demoCategories = await context.Categories
            .Where(c => c.UserId == userId && c.IsDemo)
            .ToListAsync();
        context.Categories.RemoveRange(demoCategories);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Creates the full set of default categories (shared by all users) if none exist yet.
    /// Mirrors the logic on the Categories page so the demo and that page agree.
    /// </summary>
    private static async Task EnsureDefaultCategoriesAsync(ApplicationDbContext context)
    {
        if (await context.Categories.AnyAsync(c => c.IsDefault && c.UserId == null))
        {
            return;
        }

        foreach (var name in DefaultCategories.ExpenseCategoryNames)
        {
            context.Categories.Add(new Category { Name = name, IsDefault = true, IsIncome = false, UserId = null });
        }

        foreach (var name in DefaultCategories.IncomeCategoryNames)
        {
            context.Categories.Add(new Category { Name = name, IsDefault = true, IsIncome = true, UserId = null });
        }

        await context.SaveChangesAsync();
    }
}
