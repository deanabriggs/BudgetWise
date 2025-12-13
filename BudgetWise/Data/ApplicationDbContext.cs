// File: Data/ApplicationDbContext.cs
// This class is the bridge between our C# classes and the database.

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetWise.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Table for all user transactions (expenses)
        public DbSet<UserTransaction> UserTransactions { get; set; } = default!;

        // Table for monthly budgets
        public DbSet<Budget> Budgets { get; set; } = default!;

        // Income table
        public DbSet<Income> Incomes { get; set; } = default!;

        // NEW: Debts table
        public DbSet<Debt> Debts { get; set; } = default!;

        // NEW: Savings goals table
        public DbSet<SavingsGoal> SavingsGoals { get; set; } = default!;

        // Categories table (default and custom user categories)
        public DbSet<Category> Categories { get; set; } = default!;

        // NEW: Accounts table
        public DbSet<Account> Accounts { get; set; } = default!;

        // Sources table (reusable source names for transactions)
        public DbSet<Source> Sources { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ----------- ACCOUNT RELATIONSHIP -----------
            builder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----------- TRANSACTIONS RELATIONSHIP -----------
            builder.Entity<UserTransaction>()
                .HasOne(t => t.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Transaction to Account relationship
            builder.Entity<UserTransaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting account with transactions

            // ----------- BUDGET RELATIONSHIP -----------
            builder.Entity<Budget>()
                .HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate budgets for same user + category + month + year.
            builder.Entity<Budget>()
                .HasIndex(b => new { b.UserId, b.CategoryId, b.Month, b.Year })
                .IsUnique();

            // ----------- INCOME RELATIONSHIP -----------
            builder.Entity<Income>()
                .HasOne< User >()
                .WithMany(u => u.Incomes)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----------- DEBT RELATIONSHIP -----------
            builder.Entity<Debt>()
                .HasOne(d => d.User)
                .WithMany(u => u.Debts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----------- SAVINGS GOAL RELATIONSHIP -----------
            builder.Entity<SavingsGoal>()
                .HasOne(g => g.User)
                .WithMany(u => u.SavingsGoals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ----------- CATEGORY RELATIONSHIP -----------
            builder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Note: Uniqueness is enforced in application code
            // For default categories (UserId = null): unique by Name
            // For user categories: unique by UserId + Name

            // ----------- SOURCE RELATIONSHIP -----------
            builder.Entity<Source>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Prevent duplicate sources for same user + name
            builder.Entity<Source>()
                .HasIndex(s => new { s.UserId, s.Name })
                .IsUnique();
        }
    }
}
