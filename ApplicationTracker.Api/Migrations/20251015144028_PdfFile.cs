using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class PdfFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EncryptedApplicationPdf",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "EncryptedResumePdf",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "PdfFiles",
                columns: table => new
                {
                    PdfFileId = table.Column<Guid>(type: "uuid", nullable: false),
                    JobApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    JobApplicationId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PdfFiles", x => x.PdfFileId);
                    table.ForeignKey(
                        name: "FK_PdfFiles_Applications_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "Applications",
                        principalColumn: "JobApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PdfFiles_Applications_JobApplicationId1",
                        column: x => x.JobApplicationId1,
                        principalTable: "Applications",
                        principalColumn: "JobApplicationId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PdfFiles_JobApplicationId",
                table: "PdfFiles",
                column: "JobApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PdfFiles_JobApplicationId1",
                table: "PdfFiles",
                column: "JobApplicationId1",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PdfFiles");

            migrationBuilder.AddColumn<byte[]>(
                name: "EncryptedApplicationPdf",
                table: "Applications",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "EncryptedResumePdf",
                table: "Applications",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
