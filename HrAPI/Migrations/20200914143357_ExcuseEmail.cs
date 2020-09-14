using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HrAPI.Migrations
{
    public partial class ExcuseEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Arrival",
                table: "Excuses");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Excuses",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Hours",
                table: "Excuses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Time",
                table: "Excuses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Excuses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Excuses");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Excuses");

            migrationBuilder.DropColumn(
                name: "Time",
                table: "Excuses");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Excuses");

            migrationBuilder.AddColumn<DateTime>(
                name: "Arrival",
                table: "Excuses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
