using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    public partial class UpdateUserSettingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserSettings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
