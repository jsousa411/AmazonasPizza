using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarterProject.Context.Migrations
{
    public partial class creatingproductclass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(360)", maxLength: 360, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    AdvertisingContext = table.Column<string>(type: "nvarchar(360)", maxLength: 360, nullable: true),
                    MenuText = table.Column<string>(type: "nvarchar(360)", maxLength: 360, nullable: true),
                    price = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Product");
        }
    }
}
