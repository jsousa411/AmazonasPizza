using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterProject.Context.Migrations
{
    public partial class renamedConfigurationtoUserConfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Configuration",
                table: "Configuration");

            migrationBuilder.RenameTable(
                name: "Configuration",
                newName: "UserConfiguration");

            migrationBuilder.RenameIndex(
                name: "IX_Configuration_Name",
                table: "UserConfiguration",
                newName: "IX_UserConfiguration_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserConfiguration",
                table: "UserConfiguration",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserConfiguration",
                table: "UserConfiguration");

            migrationBuilder.RenameTable(
                name: "UserConfiguration",
                newName: "Configuration");

            migrationBuilder.RenameIndex(
                name: "IX_UserConfiguration_Name",
                table: "Configuration",
                newName: "IX_Configuration_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Configuration",
                table: "Configuration",
                column: "Id");
        }
    }
}
