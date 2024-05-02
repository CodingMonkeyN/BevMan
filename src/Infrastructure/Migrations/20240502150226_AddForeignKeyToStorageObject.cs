using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BevMan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyToStorageObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_path",
                table: "products");

            migrationBuilder.AddColumn<Guid>(
                name: "storage_object_id",
                table: "products",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_storage_object_id",
                table: "products",
                column: "storage_object_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_products_storage_objects_storage_object_id",
                table: "products",
                column: "storage_object_id",
                principalSchema: "storage",
                principalTable: "objects",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_products_storage_objects_storage_object_id",
                table: "products");

            migrationBuilder.DropIndex(
                name: "ix_products_storage_object_id",
                table: "products");

            migrationBuilder.DropColumn(
                name: "storage_object_id",
                table: "products");

            migrationBuilder.AddColumn<string>(
                name: "image_path",
                table: "products",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
