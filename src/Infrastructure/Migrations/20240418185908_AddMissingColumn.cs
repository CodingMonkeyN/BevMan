using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BevMan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMissingColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Created",
                table: "user_roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "user_roles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModified",
                table: "user_roles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "user_roles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "user_roles");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "user_roles");
        }
    }
}
