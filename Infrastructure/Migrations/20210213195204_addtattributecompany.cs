using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addtattributecompany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "CompanyName",
            //    table: "PortfolioItems");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "PortfolioItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "PortfolioItems");

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "PortfolioItems",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
