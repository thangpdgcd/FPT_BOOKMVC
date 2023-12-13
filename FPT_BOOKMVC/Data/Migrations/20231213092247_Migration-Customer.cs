using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPT_BOOKMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class MigrationCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
               name: "Customers",
               columns: table => new
               {
                   UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Customers", x => x.UserId);
                   table.ForeignKey(
                       name: "FK_Customers_AspNetUsers_UserId",
                       column: x => x.UserId,
                       principalTable: "AspNetUsers",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
               });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
             name: "Customers");
        }
    }
}
