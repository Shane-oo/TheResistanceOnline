using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDiscordEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordUsers");

            migrationBuilder.DropTable(
                name: "DiscordRoles");

            migrationBuilder.DropTable(
                name: "DiscordChannels");

            migrationBuilder.DropColumn(
                name: "UserWantsToUseDiscord",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "UserWantsToUseDiscordRecord",
                table: "UserSettings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserWantsToUseDiscord",
                table: "UserSettings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "UserWantsToUseDiscordRecord",
                table: "UserSettings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscordChannels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordChannels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordChannelId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordRoles_DiscordChannels_DiscordChannelId",
                        column: x => x.DiscordChannelId,
                        principalTable: "DiscordChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DiscordUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordRoleId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscordTag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscordUsers_DiscordRoles_DiscordRoleId",
                        column: x => x.DiscordRoleId,
                        principalTable: "DiscordRoles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "DiscordChannels",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "game-1" },
                    { 2, "game-2" },
                    { 3, "game-3" },
                    { 4, "game-4" },
                    { 5, "game-5" },
                    { 6, "game-6" },
                    { 7, "game-7" },
                    { 8, "game-8" },
                    { 9, "game-9" },
                    { 10, "game-10" }
                });

            migrationBuilder.InsertData(
                table: "DiscordRoles",
                columns: new[] { "Id", "DiscordChannelId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Can Join Game-1" },
                    { 2, 2, "Can Join Game-2" },
                    { 3, 3, "Can Join Game-3" },
                    { 4, 4, "Can Join Game-4" },
                    { 5, 5, "Can Join Game-5" },
                    { 6, 6, "Can Join Game-6" },
                    { 7, 7, "Can Join Game-7" },
                    { 8, 8, "Can Join Game-8" },
                    { 9, 9, "Can Join Game-9" },
                    { 10, 10, "Can Join Game-10" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRoles_DiscordChannelId",
                table: "DiscordRoles",
                column: "DiscordChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUsers_DiscordRoleId",
                table: "DiscordUsers",
                column: "DiscordRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscordUsers_UserId",
                table: "DiscordUsers",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }
    }
}
