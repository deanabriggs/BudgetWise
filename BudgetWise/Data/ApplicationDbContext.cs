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

        // Categories table (default and custom user categories)
        public DbSet<Category> Categories { get; set; } = default!;

        // NEW: Accounts table
        public DbSet<Account> Accounts { get; set; } = default!;

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

            // ----------- CATEGORY RELATIONSHIP -----------
            builder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Note: Uniqueness is enforced in application code
            // For default categories (UserId = null): unique by Name
            // For user categories: unique by UserId + Name
        }
    }
}
