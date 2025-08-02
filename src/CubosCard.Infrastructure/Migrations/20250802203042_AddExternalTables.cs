using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CubosCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExternalAuthentication",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    auth_code = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_external_authentication", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ExternalAuthenticationToken",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_authentication_id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_token_id = table.Column<string>(type: "text", nullable: false),
                    external_access_token = table.Column<string>(type: "text", nullable: false),
                    external_refresh_token = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: false),
                    deleted_at = table.Column<DateTime>(type: "timestamp(6) without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_external_authentication_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_external_authentication_token_external_authentication_external",
                        column: x => x.external_authentication_id,
                        principalTable: "ExternalAuthentication",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_external_authentication_email",
                table: "ExternalAuthentication",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_external_authentication_token_external_authentication_id",
                table: "ExternalAuthenticationToken",
                column: "external_authentication_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExternalAuthenticationToken");

            migrationBuilder.DropTable(
                name: "ExternalAuthentication");
        }
    }
}
