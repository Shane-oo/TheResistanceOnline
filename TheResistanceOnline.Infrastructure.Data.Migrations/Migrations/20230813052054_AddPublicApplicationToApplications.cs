using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheResistanceOnline.Infrastructure.Data.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicApplicationToApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
              migrationBuilder.RunSqlScript("20230813052054_AddPublicApplicationToApplications.up.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.RunSqlScript("20230813052054_AddPublicApplicationToApplications.down.sql");
        }
    }
}
