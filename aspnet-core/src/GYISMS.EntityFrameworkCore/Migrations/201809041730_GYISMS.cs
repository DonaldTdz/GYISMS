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

            migrationBuilder.CreateTable(
                name: "SystemDatas",
                columns: table => new
                { Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModelId = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: false),
                    Desc = table.Column<string>(maxLength: 500, nullable: false),
                    Seq = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemDatas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 200, nullable: false),
                    OpenId = table.Column<string>(maxLength: 200, nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: true),
                    Mobile = table.Column<string>(maxLength: 11, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Active = table.Column<bool>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: true),
                    IsBoss = table.Column<bool>(nullable: true),
                    Department = table.Column<string>(maxLength: 300, nullable: true),
                    Position = table.Column<string>(maxLength: 100, nullable: true),
                    Avatar = table.Column<string>(maxLength: 200, nullable: true),
                    HiredDate = table.Column<string>(maxLength: 100, nullable: true),
                    Roles = table.Column<string>(maxLength: 300, nullable: true),
                    RoleId = table.Column<long>(nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "Organizations");
            migrationBuilder.DropTable(
            name: "SystemDatas");
            migrationBuilder.DropTable(
            name: "Employees");
        }
    }
}
