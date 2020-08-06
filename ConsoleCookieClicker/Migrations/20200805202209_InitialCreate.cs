using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleCookieClicker.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cookies = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Upgrades",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CookieClickerDataId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    BasePrice = table.Column<int>(nullable: false),
                    BaseCookiesPerTick = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upgrades", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Data");

            migrationBuilder.DropTable(
                name: "Upgrades");
        }
    }
}
