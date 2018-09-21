using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GYISMS.Migrations
{
    public partial class GYISMS_TobaccoService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                 name: "Growers",
                 columns: table => new
                 {
                     Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                     Year = table.Column<int>(nullable: true),
                     UnitCode = table.Column<string>(maxLength: 20, nullable: true),
                     UnitName = table.Column<string>(maxLength: 50, nullable: true),
                     Name = table.Column<string>(maxLength: 50, nullable: false),
                     CountyCode = table.Column<int>(nullable: true),
                     EmployeeId = table.Column<string>(maxLength: 200, nullable: true),
                     EmployeeName = table.Column<string>(maxLength: 200, nullable: true),
                     ContractNo = table.Column<string>(maxLength: 50, nullable: true),
                     VillageGroup = table.Column<string>(maxLength: 50, nullable: true),
                     Tel = table.Column<string>(maxLength: 20, nullable: true),
                     Address = table.Column<string>(maxLength: 500, nullable: true),
                     Type = table.Column<int>(nullable: true),
                     PlantingArea = table.Column<decimal>(nullable: true),
                     Longitude = table.Column<decimal>(nullable: true),
                     Latitude = table.Column<decimal>(nullable: true),
                     ContractTime = table.Column<DateTime>(nullable: true),
                     IsDeleted = table.Column<bool>(nullable: true),
                     CreationTime = table.Column<DateTime>(nullable: true),
                     CreatorUserId = table.Column<long>(nullable: true),
                     LastModificationTime = table.Column<DateTime>(nullable: true),
                     LastModifierUserId = table.Column<long>(nullable: true),
                     DeletionTime = table.Column<DateTime>(nullable: true),
                     DeleterUserId = table.Column<long>(nullable: true)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_Growers", x => x.Id);
                 });

            migrationBuilder.CreateTable(
                name: "Schedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Desc = table.Column<string>(maxLength: 500, nullable: true),
                    Type = table.Column<int>(nullable: false),
                    BeginTime = table.Column<DateTime>(nullable: true),
                    EndTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    PublishTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskId = table.Column<int>(nullable: false),
                    ScheduleId = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: false, maxLength:50),
                    GrowerId = table.Column<int>(nullable: false),
                    VisitNum = table.Column<int>(nullable: true),
                    CompleteNum = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int?>(nullable: true),
                    ScheduleTaskId = table.Column<Guid>(nullable: false),
                    EmployeeName = table.Column<string>(nullable: true),
                    GrowerName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TaskId = table.Column<int>(nullable: false),
                    TaskName = table.Column<string>(maxLength: 200, nullable: true),
                    ScheduleId = table.Column<Guid>(nullable: false),
                    VisitNum = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    IsExamine = table.Column<bool>(nullable: true),
                    Desc = table.Column<string>(maxLength: 500, nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskExamines",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TaskId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Desc = table.Column<string>(maxLength: 500, nullable: true),
                    Seq = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true),
                    CreatorUserId = table.Column<long>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierUserId = table.Column<long>(nullable: true),
                    DeletionTime = table.Column<DateTime>(nullable: true),
                    DeleterUserId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskExamines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitExamines",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    VisitRecordId = table.Column<Guid>(nullable: true),
                    EmployeeId = table.Column<string>(nullable: true),
                    GrowerId = table.Column<int>(nullable: true),
                    TaskExamineId = table.Column<int>(nullable: true),
                    Score = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitExamines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ScheduleDetailId = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column <string>(nullable: true),
                    GrowerId = table.Column<int>(nullable: true),
                    SignTime = table.Column<DateTime>(nullable: true),
                    Location = table.Column<string>(maxLength: 200, nullable: true),
                    Longitude = table.Column<decimal>(nullable: true),
                    Latitude = table.Column<decimal>(nullable: true),
                    Desc = table.Column<string>(maxLength: 500, nullable: true),
                    ImgPath = table.Column<string>(maxLength: 200, nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitRecords", x => x.Id);
                });
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Growers");
            migrationBuilder.DropTable(
                name: "Schedules");
            migrationBuilder.DropTable(
                name: "ScheduleDetails");
            migrationBuilder.DropTable(
                name: "ScheduleTasks");
            migrationBuilder.DropTable(
                name: "VisitTasks");
            migrationBuilder.DropTable(
                name: "TaskExamines");
            migrationBuilder.DropTable(
                name: "VisitExamine");
            migrationBuilder.DropTable(
                name: "VisitRecord");
        }
    }
}
