using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addtattributetypegender : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "Gender",
            //    table: "PortfolioItems");

            migrationBuilder.AddColumn<string>(
                name: "gendertype",
                table: "PortfolioItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gendertype",
                table: "PortfolioItems");

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
