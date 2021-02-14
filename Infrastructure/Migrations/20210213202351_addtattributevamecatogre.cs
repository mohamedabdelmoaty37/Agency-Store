using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addtattributevamecatogre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "PortfolioItems");

            migrationBuilder.AddColumn<string>(
                name: "Namecatogry",
                table: "PortfolioItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Namecatogry",
                table: "PortfolioItems");

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
