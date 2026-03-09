using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileImage",
                table: "ItemProducts");

            migrationBuilder.AddColumn<List<string>>(
                name: "FileImage",
                table: "Products",
                type: "text[]",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileImage",
                table: "Products");

            migrationBuilder.AddColumn<List<string>>(
                name: "FileImage",
                table: "ItemProducts",
                type: "text[]",
                nullable: false);
        }
    }
}
