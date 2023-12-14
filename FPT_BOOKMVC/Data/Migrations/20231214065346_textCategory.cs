using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPT_BOOKMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class textCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "BookId", "Author", "CategoryId", "Description", "Image", "Name", "Price", "PublishCompanyId", "Quantity", "UpdateDate" },
                values: new object[] { 1, "ltn", 1, "Title", "123", "Title", 1m, 1, 1, new DateTime(2023, 12, 14, 13, 53, 45, 956, DateTimeKind.Local).AddTicks(7611) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1);
        }
    }
}
