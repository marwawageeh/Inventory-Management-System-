using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace test_inventory.Migrations
{
    /// <inheritdoc />
    public partial class supplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "Purchas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "supplier",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_supplier", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchas_SupplierId",
                table: "Purchas",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchas_supplier_SupplierId",
                table: "Purchas",
                column: "SupplierId",
                principalTable: "supplier",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchas_supplier_SupplierId",
                table: "Purchas");

            migrationBuilder.DropTable(
                name: "supplier");

            migrationBuilder.DropIndex(
                name: "IX_Purchas_SupplierId",
                table: "Purchas");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "Purchas");
        }
    }
}
