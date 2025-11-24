using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetWise.Migrations
{
    /// <inheritdoc />
    public partial class AddIncomeColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Source",
                table: "Incomes");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Incomes",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Incomes");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "Incomes",
                type: "TEXT",
                nullable: true);
        }
    }
}
