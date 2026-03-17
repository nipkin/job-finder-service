using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFinderService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CvScore",
                table: "JobPostings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OptimizedCv",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CvScore",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "OptimizedCv",
                table: "JobPostings");
        }
    }
}
