using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNullUserIdFromPlayerStatitics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistics_AspNetUsers_UserId",
                table: "PlayerStatistics");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "PlayerStatistics");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PlayerStatistics",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistics_AspNetUsers_UserId",
                table: "PlayerStatistics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistics_AspNetUsers_UserId",
                table: "PlayerStatistics");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "PlayerStatistics",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldMaxLength: 450);

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "PlayerStatistics",
                type: "nvarchar(450)",
                maxLength: 450,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistics_AspNetUsers_UserId",
                table: "PlayerStatistics",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
