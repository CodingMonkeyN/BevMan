using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BevMan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "public_url",
                table: "products",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "public_url",
                table: "products");
        }
    }
}
