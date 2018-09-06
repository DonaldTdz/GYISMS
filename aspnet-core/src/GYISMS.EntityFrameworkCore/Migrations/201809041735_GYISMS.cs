using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GYISMS.Migrations
{
    public partial class GYISMS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    DepartmentName = table.Column<string>(maxLength: 100, nullable: false),
                    ParentId = table.Column<long>(nullable: true),
                    Order = table.Column<long>(nullable: true),
                    DeptHiding = table.Column<bool>(nullable: true),
                    OrgDeptOwner = table.Column<string>(maxLength: 100, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Organizations");
        }
    }
}
