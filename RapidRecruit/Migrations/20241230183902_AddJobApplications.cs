using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidRecruit.Migrations
{
    /// <inheritdoc />
    public partial class AddJobApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobApplication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    JobPostingId = table.Column<int>(type: "int", nullable: false),
                    CandidateNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewerNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReviewDescision = table.Column<int>(type: "int", nullable: true),
                    ResumeFileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResumeFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobApplication_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobApplication_JobPosting_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPosting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_JobPostingId",
                table: "JobApplication",
                column: "JobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_JobApplication_UserId",
                table: "JobApplication",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplication");
        }
    }
}
