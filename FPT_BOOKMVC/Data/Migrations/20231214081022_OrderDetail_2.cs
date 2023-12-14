using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPT_BOOKMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderDetail_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                column: "UpdateDate",
                value: new DateTime(2023, 12, 14, 15, 10, 22, 452, DateTimeKind.Local).AddTicks(9981));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Books",
                keyColumn: "BookId",
                keyValue: 1,
                column: "UpdateDate",
                value: new DateTime(2023, 12, 14, 14, 47, 28, 260, DateTimeKind.Local).AddTicks(313));
        }
    }
}
