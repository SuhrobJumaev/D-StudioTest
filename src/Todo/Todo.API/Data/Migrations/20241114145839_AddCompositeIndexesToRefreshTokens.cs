using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCompositeIndexesToRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "RefreshToken_UserId_IsRevoked",
                table: "RefreshTokens",
                columns: new[] { "UserId", "IsRevoked" });

            migrationBuilder.CreateIndex(
                name: "RefreshToken_UserId_Token",
                table: "RefreshTokens",
                columns: new[] { "UserId", "Token" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "RefreshToken_UserId_IsRevoked",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "RefreshToken_UserId_Token",
                table: "RefreshTokens");
        }
    }
}
