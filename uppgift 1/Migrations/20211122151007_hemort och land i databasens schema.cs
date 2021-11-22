using Microsoft.EntityFrameworkCore.Migrations;

namespace Kartotek.Migrations
{
    public partial class hemortochlandidatabasensschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "medlemskartotek");

            migrationBuilder.RenameTable(
                name: "Person",
                schema: "orter",
                newName: "Person",
                newSchema: "medlemskartotek");

            migrationBuilder.RenameTable(
                name: "Orter",
                schema: "orter",
                newName: "Orter",
                newSchema: "medlemskartotek");

            migrationBuilder.RenameTable(
                name: "Land",
                schema: "orter",
                newName: "Land",
                newSchema: "medlemskartotek");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "orter");

            migrationBuilder.RenameTable(
                name: "Person",
                schema: "medlemskartotek",
                newName: "Person",
                newSchema: "orter");

            migrationBuilder.RenameTable(
                name: "Orter",
                schema: "medlemskartotek",
                newName: "Orter",
                newSchema: "orter");

            migrationBuilder.RenameTable(
                name: "Land",
                schema: "medlemskartotek",
                newName: "Land",
                newSchema: "orter");
        }
    }
}
