using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPT_BOOKMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
				   name: "Carts",
				   columns: table => new
				   {
					   CartId = table.Column<int>(type: "int", nullable: false)
						   .Annotation("SqlServer:Identity", "1, 1"),
					   BookId = table.Column<int>(type: "int", nullable: false),
					   Quantity = table.Column<int>(type: "int", nullable: false),
					   Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
				   },
				   constraints: table =>
				   {
					   table.PrimaryKey("PK_Carts", x => x.CartId);
				   });

			migrationBuilder.CreateIndex(
				name: "IX_Carts_BookId",
				table: "Carts",
				column: "BookId");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropTable(
			name: "Carts");
		}
    }
}
