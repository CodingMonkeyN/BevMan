using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BevMan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class User_UserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_user_roles_users_UserId",
                table: "user_roles",
                column: "UserId",
                principalSchema: "auth",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_roles_users_UserId",
                table: "user_roles");
        }
    }
}
