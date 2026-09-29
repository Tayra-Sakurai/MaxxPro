using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cougar.Migrations
{
    /// <inheritdoc />
    public partial class AddedUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SessionId",
                table: "Sessions",
                column: "SessionId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sessions_SessionId",
                table: "Sessions");
        }
    }
}
