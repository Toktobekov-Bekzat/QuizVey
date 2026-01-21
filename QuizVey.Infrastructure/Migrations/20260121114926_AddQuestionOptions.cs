using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizVey.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQuestionOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "_options",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "_options",
                table: "Questions");
        }
    }
}
