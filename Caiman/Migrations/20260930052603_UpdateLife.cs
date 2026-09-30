using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Caiman.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLife : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Life",
                table: "Items",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(2026, 9, 30, 14, 26, 3, 78, DateTimeKind.Unspecified).AddTicks(2792), new TimeSpan(0, 9, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "Life",
                table: "Items",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "TEXT",
                oldDefaultValue: new DateTimeOffset(new DateTime(2026, 9, 30, 14, 26, 3, 78, DateTimeKind.Unspecified).AddTicks(2792), new TimeSpan(0, 9, 0, 0, 0)));
        }
    }
}
