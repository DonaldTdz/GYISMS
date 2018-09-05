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
                { Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", 
                SqlServerValueGenerationStrategy.IdentityColumn),
                    DepartmentName = table.Column<string>(maxLength: 100, nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: true),
                    DeptHiding = table.Column<bool>(nullable: true),
                    OrgDeptOwner = table.Column<string>(maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false) },
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
