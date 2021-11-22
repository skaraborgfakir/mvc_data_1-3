using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Kartotek.Migrations
{
    public partial class databasensschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "people");

            migrationBuilder.CreateTable(
                name: "Person",
                schema: "people",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Namn = table.Column<string>(nullable: false),
                    Bostadsort = table.Column<string>(nullable: false),
                    Telefonnummer = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "people",
                table: "Person",
                columns: new[] { "Id", "Bostadsort", "Namn", "Telefonnummer" },
                values: new object[,]
                {
                    { 1, "Solberga", "Michael Carlsson", "0433" },
                    { 2, "Göteborg", "Ulf Smedbo", "031" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Person",
                schema: "people");
        }
    }
}
