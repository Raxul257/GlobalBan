using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace fr34kyn01535.GlobalBan.Migrations
{
    public partial class GlobalBan1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bans",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<string>(type: "varchar(32)", nullable: true),
                    PlayerName = table.Column<string>(type: "varchar(255)", nullable: true),
                    AdminId = table.Column<string>(type: "varchar(32)", nullable: true),
                    AdminName = table.Column<string>(type: "varchar(255)", nullable: true),
                    Reason = table.Column<string>(type: "varchar(255)", nullable: true),
                    Duration = table.Column<int>(nullable: false),
                    BanTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bans", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bans");
        }
    }
}
