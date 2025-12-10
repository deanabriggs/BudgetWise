using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetWise.Migrations
{
    /// <inheritdoc />
    public partial class Updateddatawherecategorieswerestringsandnottheclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Budgets_UserId_Category_Month_Year",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "UserTransactions");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Budgets");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "UserTransactions",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Budgets",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTransactions_CategoryId",
                table: "UserTransactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CategoryId",
                table: "Budgets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId_CategoryId_Month_Year",
                table: "Budgets",
                columns: new[] { "UserId", "CategoryId", "Month", "Year" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                table: "Budgets",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTransactions_Categories_CategoryId",
                table: "UserTransactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Categories_CategoryId",
                table: "Budgets");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTransactions_Categories_CategoryId",
                table: "UserTransactions");

            migrationBuilder.DropIndex(
                name: "IX_UserTransactions_CategoryId",
                table: "UserTransactions");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_CategoryId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_UserId_CategoryId_Month_Year",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "UserTransactions");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Budgets");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "UserTransactions",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Budgets",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId_Category_Month_Year",
                table: "Budgets",
                columns: new[] { "UserId", "Category", "Month", "Year" },
                unique: true);
        }
    }
}
