using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPT_BOOKMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigratonPushlishCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
			   name: "PushlishCompanies",
			   columns: table => new
			   {
				   PublishingCompanyId = table.Column<int>(type: "int", nullable: false)
					   .Annotation("SqlServer:Identity", "1, 1"),
				   Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
				   Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
			   },
			   constraints: table =>
			   {
				   table.PrimaryKey("PK_PushlishCompanies", x => x.PublishingCompanyId);
			   });
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
			migrationBuilder.DropTable(
				name: "PushlishCompanies");
		}
    }
}
