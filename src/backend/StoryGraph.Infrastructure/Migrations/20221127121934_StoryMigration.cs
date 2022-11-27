using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryGraph.Infrastructure.Migrations
{
    public partial class StoryMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "Stories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOnUtc",
                table: "Stories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Stories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "CreatedOnUtc",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "ModifiedOnUtc",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Stories");
        }
    }
}
