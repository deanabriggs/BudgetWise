using BudgetWise.Data;
using BudgetWise.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Set default culture to US (en-US) for consistent currency formatting
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Database - supports both local development and production
var connectionString = builder.Configuration.GetConnectionString("Database");
// Allow override via environment variable (useful for Fly.io or other deployments)
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING") 
        ?? "Data Source=BudgetWise.db"; // Default fallback for local development
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.EnableSensitiveDataLogging(true);
    options.UseSqlite(connectionString);
});

// Identity
builder.Services.AddDefaultIdentity<User>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// NOTE: No custom AuthenticationStateProvider registration needed here.
// AddServerSideBlazor wires up an AuthenticationStateProvider that reads from ASP.NET Core Identity.

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Only use HTTPS redirection in development or when not behind a proxy
// Fly.io handles HTTPS at the proxy level
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Apply database migrations
app.ApplyMigrations();

app.MapRazorPages();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

// Configure port from environment variable (Fly.io sets PORT)
// If PORT is set, use it; otherwise use default 8080
var port = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(port))
{
    app.Urls.Clear();
    app.Urls.Add($"http://0.0.0.0:{port}");
}

app.Run();
