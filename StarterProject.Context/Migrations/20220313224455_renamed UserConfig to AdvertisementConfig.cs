using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterProject.Context.Migrations
{
    public partial class renamedUserConfigtoAdvertisementConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserConfiguration");

            migrationBuilder.CreateTable(
                name: "AdvertisementConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(360)", maxLength: 360, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertisementConfig", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisementConfig_Name",
                table: "AdvertisementConfig",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvertisementConfig");

            migrationBuilder.CreateTable(
                name: "UserConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(360)", maxLength: 360, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserConfiguration", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserConfiguration_Name",
                table: "UserConfiguration",
                column: "Name",
                unique: true);
        }
    }
}
