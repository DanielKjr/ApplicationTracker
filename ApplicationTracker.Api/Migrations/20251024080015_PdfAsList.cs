using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicationTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class PdfAsList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PdfFiles_Applications_JobApplicationId1",
                table: "PdfFiles");

            migrationBuilder.DropIndex(
                name: "IX_PdfFiles_JobApplicationId",
                table: "PdfFiles");

            migrationBuilder.DropIndex(
                name: "IX_PdfFiles_JobApplicationId1",
                table: "PdfFiles");

            migrationBuilder.DropColumn(
                name: "JobApplicationId1",
                table: "PdfFiles");

            migrationBuilder.AddColumn<string>(
                name: "PdfType",
                table: "PdfFiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Applications",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PdfFiles_JobApplicationId",
                table: "PdfFiles",
                column: "JobApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PdfFiles_JobApplicationId",
                table: "PdfFiles");

            migrationBuilder.DropColumn(
                name: "PdfType",
                table: "PdfFiles");

            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Applications");

            migrationBuilder.AddColumn<Guid>(
                name: "JobApplicationId1",
                table: "PdfFiles",
                type: "uuid",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_PdfFiles_Applications_JobApplicationId1",
                table: "PdfFiles",
                column: "JobApplicationId1",
                principalTable: "Applications",
                principalColumn: "JobApplicationId");
        }
    }
}
