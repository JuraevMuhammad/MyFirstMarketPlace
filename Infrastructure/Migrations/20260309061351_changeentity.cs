using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Color",
                table: "Orders",
                newName: "ColorProduct");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ItemProducts",
                newName: "ColorProduct");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorProduct",
                table: "Orders",
                newName: "Color");

            migrationBuilder.RenameColumn(
                name: "ColorProduct",
                table: "ItemProducts",
                newName: "Color");
        }
    }
}
