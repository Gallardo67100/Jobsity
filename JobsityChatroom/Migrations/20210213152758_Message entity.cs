using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JobsityChatroom.Migrations
{
    public partial class Messageentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Timestamp = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    MessageText = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => new { x.Timestamp, x.MessageText });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");
        }
    }
}
