using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthApiPg.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeToCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cycles_CycleId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CycleId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CycleId",
                table: "Transactions");

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Cycles",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "Cycles");

            migrationBuilder.AddColumn<int>(
                name: "CycleId",
                table: "Transactions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CycleId",
                table: "Transactions",
                column: "CycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cycles_CycleId",
                table: "Transactions",
                column: "CycleId",
                principalTable: "Cycles",
                principalColumn: "CycleId");
        }
    }
}
