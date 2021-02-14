using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class addtattributetypeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "typeId",
                table: "PortfolioItems",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItems_typeId",
                table: "PortfolioItems",
                column: "typeId");

            migrationBuilder.AddForeignKey(
                name: "FK_PortfolioItems_Typecat_typeId",
                table: "PortfolioItems",
                column: "typeId",
                principalTable: "Typecat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PortfolioItems_Typecat_typeId",
                table: "PortfolioItems");

            migrationBuilder.DropIndex(
                name: "IX_PortfolioItems_typeId",
                table: "PortfolioItems");

            migrationBuilder.DropColumn(
                name: "typeId",
                table: "PortfolioItems");
        }
    }
}
