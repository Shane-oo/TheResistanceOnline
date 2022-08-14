using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    public partial class AddProfilePictures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3b0f80c8-1d82-42dc-835d-c6f3d0df73f8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fb9528c5-e30c-4919-8f1c-403b2d9156dd");

            migrationBuilder.AddColumn<int>(
                name: "ProfilePictureId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfilePictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilePictures", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProfilePictures",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Black man cyberpunk", "1.png" },
                    { 2, "White woman cyberpunk", "2.png" },
                    { 3, "White man spy", "3.png" },
                    { 4, "White woman spy", "4.png" },
                    { 5, "White man cyber warrior", "5.png" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfilePictures");

            migrationBuilder.DropColumn(
                name: "ProfilePictureId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3b0f80c8-1d82-42dc-835d-c6f3d0df73f8", "5c60cc2b-5c5e-4ea7-8337-f153088690d7", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fb9528c5-e30c-4919-8f1c-403b2d9156dd", "5387cdf1-8fe9-4c5e-ac8e-b7c658060bbf", "User", "USER" });
        }
    }
}
