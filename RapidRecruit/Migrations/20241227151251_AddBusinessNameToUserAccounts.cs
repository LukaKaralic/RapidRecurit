using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidRecruit.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessNameToUserAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "AspNetUsers");
        }
    }
}
