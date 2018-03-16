using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SenaiEnergia.Migrations
{
    public partial class MakeCompanyAEntityAndRelactCompanyToTimeInterval : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "TimeInterval");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "TimeInterval",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    CompanyID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.CompanyID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeInterval_CompanyId",
                table: "TimeInterval",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeInterval_Company_CompanyId",
                table: "TimeInterval",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeInterval_Company_CompanyId",
                table: "TimeInterval");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_TimeInterval_CompanyId",
                table: "TimeInterval");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "TimeInterval");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "TimeInterval",
                nullable: true);
        }
    }
}
