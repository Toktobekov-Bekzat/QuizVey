using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizVey.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "AssessmentVersions");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Assessments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Assessments");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AssessmentVersions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
