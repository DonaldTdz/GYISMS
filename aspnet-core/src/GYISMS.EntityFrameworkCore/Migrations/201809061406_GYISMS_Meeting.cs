using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace GYISMS.Migrations
{
    public partial class GYISMS_Meeting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false),
                    MeetingRoomId = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Issues = table.Column<string>(maxLength: 2000, nullable: true),
                    Desc = table.Column<string>(maxLength: 500, nullable: true),
                    BeginTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    HostId = table.Column<string>(nullable: true),
                    HostName = table.Column<string>(nullable: true),
                    NoticeWay = table.Column<int>(nullable: true),
                    RemindingWay = table.Column<int>(nullable: true),
                    RemindingTime = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    AuditId = table.Column<string>(nullable: true),
                    AuditName = table.Column<string>(nullable: true),
                    AuditTime = table.Column<DateTime>(nullable: true),
                    CancelUserId = table.Column<string>(nullable: true),
                    CancelUserName = table.Column<string>(maxLength: 50, nullable: true),
                    CancelTime = table.Column<DateTime>(nullable: true),
                    ResponsibleId = table.Column<string>(nullable: true),
                    ResponsibleName = table.Column<string>(nullable: true),
                    IsSeatingOrder = table.Column<bool>(nullable: true),
                    Summary = table.Column<string>(nullable: true),
                    FilePath = table.Column<string>(maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_Meetings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingMaterials",
                columns: table => new
                { Id = table.Column<Guid>(nullable: false),
                    MeetingId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Num = table.Column<int>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true) },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingMaterials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingParticipants",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MeetingId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    Row = table.Column<int>(nullable: true),
                    Column = table.Column<int>(nullable: true),
                    ConfirmTime = table.Column<DateTime>(nullable: true),
                    SignTime = table.Column<DateTime>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingParticipantss", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeetingRooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Photo = table.Column<string>(maxLength: 200, nullable: false),
                    Num = table.Column<int>(nullable: false),
                    RoomType = table.Column<int>(nullable: true),
                    Address = table.Column<string>(maxLength: 500, nullable: true),
                    BuildDesc = table.Column<string>(nullable: true),
                    IsApprove = table.Column<bool>(nullable: true),
                    ManagerId = table.Column<string>(nullable: true),
                    ManagerName = table.Column<string>(maxLength: 50, nullable: true),
                    Row = table.Column<int>(nullable: true),
                    Column = table.Column<int>(nullable: true),
                    LayoutPattern = table.Column<int>(nullable: true),
                    PlanPath = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(maxLength: 500, nullable: true),
                    Devices = table.Column<string>(maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_MeetingRooms", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meetings");
            migrationBuilder.DropTable(
                name: "MeetingMaterials");
            migrationBuilder.DropTable(
                name: "MeetingParticipants");
            migrationBuilder.DropTable(
                name: "MeetingRooms");
        }
    }
}
