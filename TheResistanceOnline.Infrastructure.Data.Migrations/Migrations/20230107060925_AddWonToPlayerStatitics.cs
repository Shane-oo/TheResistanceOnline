using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddWonToPlayerStatitics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Won",
                table: "PlayerStatistics",
                type: "bit",
                nullable: false,
                defaultValue: false);
          migrationBuilder.RunSqlScript("20230107060925_AddWonToPlayerStatitics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Won",
                table: "PlayerStatistics");
        }
    }
}
