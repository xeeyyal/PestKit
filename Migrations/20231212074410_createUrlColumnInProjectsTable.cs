﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PestKitAB104.Migrations
{
    public partial class createUrlColumnInProjectsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Url",
                table: "Projects");
        }
    }
}
