using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderEngineTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // [FIX] Delete orphaned OrderItems so SQL Server allows us to build the Foreign Key!
            migrationBuilder.Sql("DELETE FROM rms.OrderItems;");

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                schema: "rms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    InventoryItemId = table.Column<int>(type: "int", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityChanged = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                schema: "rms",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "rms",
                table: "OrderItems",
                column: "OrderId",
                principalSchema: "rms",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                schema: "rms",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "InventoryTransactions",
                schema: "rms");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_OrderId",
                schema: "rms",
                table: "OrderItems");
        }
    }
}
