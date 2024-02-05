using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineStoreServer.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsFeatured",
                table: "Products",
                newName: "Featured");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Featured",
                table: "Products",
                newName: "IsFeatured");
        }
    }
}
