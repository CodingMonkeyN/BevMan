using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BevMan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_balances_user_id",
                table: "balances");

            migrationBuilder.CreateTable(
                name: "user_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    display_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    storage_object_id = table.Column<Guid>(type: "uuid", nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    last_modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_modified_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_profiles_storage_objects_storage_object_id",
                        column: x => x.storage_object_id,
                        principalSchema: "storage",
                        principalTable: "objects",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_user_profiles_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_balances_user_id",
                table: "balances",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_storage_object_id",
                table: "user_profiles",
                column: "storage_object_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_user_profiles_user_id",
                table: "user_profiles",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_profiles");

            migrationBuilder.DropIndex(
                name: "ix_balances_user_id",
                table: "balances");

            migrationBuilder.CreateIndex(
                name: "ix_balances_user_id",
                table: "balances",
                column: "user_id");
        }
    }
}
