using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SenaiEnergia.Migrations
{
    public partial class criacao : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeInterval_Company_CompanyId",
                table: "TimeInterval");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "TimeInterval",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeInterval_Company_CompanyId",
                table: "TimeInterval",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeInterval_Company_CompanyId",
                table: "TimeInterval");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "TimeInterval",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_TimeInterval_Company_CompanyId",
                table: "TimeInterval",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "CompanyID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
