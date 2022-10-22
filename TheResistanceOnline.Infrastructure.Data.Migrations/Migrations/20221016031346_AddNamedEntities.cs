using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    public partial class AddNamedEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "DiscordUsers",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "DiscordUsers",
                newName: "UserName");
        }
    }
}
