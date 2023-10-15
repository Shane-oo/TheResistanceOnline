using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddGoogleUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStatistics_Users_UserId1",
                table: "PlayerStatistics");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStatistics_UserId1",
                table: "PlayerStatistics");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "PlayerStatistics");

            migrationBuilder.CreateTable(
                name: "GoogleUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoogleUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoogleUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoogleUsers_Subject_UserId",
                table: "GoogleUsers",
                columns: new[] { "Subject", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GoogleUsers_UserId",
                table: "GoogleUsers",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoogleUsers");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "PlayerStatistics",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStatistics_UserId1",
                table: "PlayerStatistics",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStatistics_Users_UserId1",
                table: "PlayerStatistics",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
