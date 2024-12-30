using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidRecruit.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIndexToJobApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_UserId_JobPostingId",
                table: "JobApplication",
                columns: new[] { "UserId", "JobPostingId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JobApplication_UserId_JobPostingId",
                table: "JobApplication");
        }
    }
}