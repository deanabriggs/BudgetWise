# BudgetWise

> A full-stack personal-finance web app for planning budgets, recording income and expenses across accounts, and seeing it all come together on a live dashboard.

**🔗 Live demo:** https://budgetwise.fly.dev &nbsp;•&nbsp; **💻 Source:** https://github.com/deanabriggs/BudgetWise

<!--
  ORIGINAL (team Azure DevOps repo, access lost after the course ended):
  https://dev.azure.com/CSE-325-BudgetWise-CAE/_git/ctrl-alt-elite-project
  Replaced with the GitHub repo above, which Deana owns, for the portfolio copy.
-->

---

## Overview

BudgetWise is a .NET 8 **Blazor Server** application for managing personal finances. The core idea is a simple budgeting loop: **plan** what you intend to earn and spend (budgets), **record** what actually happens (transactions across your accounts), and **review** the cumulative results on a reporting dashboard.

It is designed for individuals and students who want a clean, organized way to track their money without a spreadsheet.

On the engineering side, it uses **ASP.NET Core Identity** for authentication (cookie-based login with hashed passwords and per-user data scoping), validates input with **data annotations** surfaced through Blazor’s `EditForm`, and handles errors gracefully with `try`/`catch` and a global exception handler so invalid input or failures never crash the app.

## Try it in 30 seconds

1. Open the **[live demo](https://budgetwise.fly.dev)** and register an account (no email confirmation required).
2. On the dashboard, click **“Load demo data.”**
3. Instantly explore a fully populated app — sample accounts, a month of transactions, budgets, and charts — then click **“Remove demo data”** to clear it.

> The one-click demo seeder lets a first-time visitor see every feature working without entering any data by hand.

## Features

**Accounts & balances**
- Six account types — Checking, Savings, Cash, Retirement, Credit Card, and Loan — grouped into assets vs. debts.
- Balances are recalculated automatically from each account’s transactions.

**Transactions**
- Add, edit, and delete transactions tied to a specific account and category, with a reusable “source” (merchant/employer) autocomplete.
- Bulk **import** transactions through an import dialog.

**Budgets (the “plan”)**
- Set monthly spending limits per category, for both income and expense categories.
- Compare planned vs. actual to see what’s remaining.

**Categories**
- A set of sensible default categories plus your own custom income/expense categories.

**Dashboard (the “report”)**
- Income, expense, and profit/loss KPIs with month-over-month comparison.
- Budget progress, an estimated net-worth summary, spending-by-category breakdown, and a daily income/expense trend — rendered with **Chart.js**.
- Searchable recent-transaction list and a quick-add form.

**Accounts, auth & data**
- Secure registration, login, and account management via ASP.NET Core Identity.
- Server-side validation and safe error handling throughout.
- Responsive Bootstrap layout with accessibility-minded labels and focus styles.

## Technology

| Area | Stack |
| --- | --- |
| Language / runtime | C#, .NET 8 |
| Web framework | Blazor Server (interactive server components), ASP.NET Core, Razor Pages |
| Authentication | ASP.NET Core Identity |
| Data | Entity Framework Core 8 (code-first migrations), SQLite |
| Front end | Bootstrap 5, custom CSS, Chart.js via JS interop |
| Containerization | Docker (multi-stage build) |
| Hosting | Fly.io (persistent volume for the SQLite database) |
| CI/CD | GitHub Actions → automatic deploy to Fly.io on every push |
| Source control | Git / GitHub (mirrored to Azure DevOps) |

## Architecture at a glance

- **Blazor Server** keeps UI logic on the server; the browser holds a lightweight connection and receives DOM updates over a websocket.
- **EF Core** maps the domain model (`Account`, `UserTransaction`, `Budget`, `Category`) to SQLite, and **migrations are applied automatically on startup**, so the schema is always current.
- All data is **scoped per user** via ASP.NET Core Identity.
- The app is packaged with a **multi-stage Dockerfile** and deployed to **Fly.io**, where the SQLite database lives on a persistent volume so it survives restarts and redeploys.

## Running locally

**Requirements:** .NET SDK 8.0 and Visual Studio or VS Code.

```bash
git clone https://github.com/deanabriggs/BudgetWise.git
cd BudgetWise
dotnet run --project BudgetWise
```

Then open the URL shown in the console (e.g. `http://localhost:5163`). The database is created and migrated automatically on first run — no manual `dotnet ef database update` step is required.

## Deployment & CI/CD

<!--
  ORIGINAL (team README):
  BudgetWise is deployed using Microsoft Azure App Service. Azure was selected
  because it fully supports ASP.NET Core and Blazor Server applications.

  Corrected below: the app is actually deployed on Fly.io (see Dockerfile and
  fly.toml). The "Azure" references in the team project were the Azure DevOps
  repo and Trello board (collaboration tools), not the app's runtime host.
-->

BudgetWise runs on **Fly.io** as a Docker container built from the included `Dockerfile`, configured via `fly.toml`, with its SQLite database on a persistent volume. A **GitHub Actions** workflow (`.github/workflows/fly-deploy.yml`) builds and deploys automatically on every push to `master`, so the live site stays in sync with the repository.

## My role

I took a **leading role across the full lifecycle** of this project:

- **Concept** — originated the idea and shaped the budgeting/transaction/dashboard approach.
- **Development** — built and integrated core features across the data model, Blazor UI, and EF Core data layer.
- **Troubleshooting & testing** — diagnosed and resolved bugs, data-flow issues, and deployment failures, and validated behavior end-to-end.
- **Editing & polishing** — refined the UI, cleaned up the data model (removing unused tables), and improved error handling and usability.
- **Deployment** — containerized the app and set up hosting on Fly.io with an automated GitHub Actions CI/CD pipeline.
- **Collaboration** — worked within a team using **Azure DevOps**, with Azure Repos for version control and Azure Boards for project management and task tracking.

---

<details>
<summary>Project background &amp; credits</summary>

<br>

BudgetWise began as a group project for a .NET course (team **ctrl-alt-elite**) and is maintained here as a personal portfolio copy.

**Team members:** Deana Briggs, Zachary Humphreys, Elora Mathias, Cam Woodward

**Demonstration video:** https://youtu.be/AS2cN6RGXbk

<!-- ORIGINAL team links retained for reference:
     Trello board (Azure DevOps): https://dev.azure.com/CSE-325-BudgetWise-CAE/ctrl-alt-elite-project/_boards/board/t/ctrl-alt-elite-project%20Team/Issues
     Website field was left blank in the original team README. -->

</details>
