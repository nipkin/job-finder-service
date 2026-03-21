using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobFinderService.Migrations
{
    /// <inheritdoc />
    public partial class AddUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobPostings_WebpageUrl",
                table: "JobPostings");

            migrationBuilder.AlterColumn<string>(
                name: "WebpageUrl",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Headline",
                table: "JobPostings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId",
                table: "JobPostings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_UserProfileId",
                table: "JobPostings",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_UserProfiles_UserProfileId",
                table: "JobPostings",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_UserProfiles_UserProfileId",
                table: "JobPostings");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_JobPostings_UserProfileId",
                table: "JobPostings");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "JobPostings");

            migrationBuilder.AlterColumn<string>(
                name: "WebpageUrl",
                table: "JobPostings",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Headline",
                table: "JobPostings",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostings_WebpageUrl",
                table: "JobPostings",
                column: "WebpageUrl",
                unique: true);
        }
    }
}
