using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CubosCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_transaction_card_card_id",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "ix_transaction_card_id",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "card_id",
                table: "Transaction");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "card_id",
                table: "Transaction",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_transaction_card_id",
                table: "Transaction",
                column: "card_id");

            migrationBuilder.AddForeignKey(
                name: "fk_transaction_card_card_id",
                table: "Transaction",
                column: "card_id",
                principalTable: "Card",
                principalColumn: "id");
        }
    }
}
