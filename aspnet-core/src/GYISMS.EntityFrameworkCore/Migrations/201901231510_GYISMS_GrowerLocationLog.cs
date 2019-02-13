using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Text;

namespace GYISMS.Migrations
{
    public partial class GYISMS_GrowerLocationLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GrowerLocationLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<string>(maxLength: 200, nullable: true),
                    GrowerId = table.Column<int>(nullable: false),
                    Longitude = table.Column<decimal>( nullable: true),
                    Latitude = table.Column<decimal>( nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrowerLocationLogs", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
              name: "CollectNum",
              table: "Growers",
              nullable: false,
              defaultValue: 0);
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrowerLocationLogs");

            migrationBuilder.DropColumn(
                name: "CollectNum",
                table: "Growers");
        }
    }
}
