using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using GYISMS.Authorization.Roles;
using GYISMS.Authorization.Users;
using GYISMS.MultiTenancy;
using GYISMS.Organizations;
using GYISMS.SystemDatas;
using GYISMS.Meetings;
using GYISMS.MeetingMaterials;
using GYISMS.MeetingRooms;
using GYISMS.MeetingParticipants;
using GYISMS.Growers;
using GYISMS.Schedules;
using GYISMS.ScheduleDetails;
using GYISMS.ScheduleTasks;
using GYISMS.TaskExamines;
using GYISMS.VisitRecords;
using GYISMS.VisitExamines;
using GYISMS.VisitTasks;
using GYISMS.Employees;

namespace GYISMS.EntityFrameworkCore
{
    public class GYISMSDbContext : AbpZeroDbContext<Tenant, Role, User, GYISMSDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public GYISMSDbContext(DbContextOptions<GYISMSDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<SystemData> SystemDatas { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Meeting> Meetings { get; set; }
        public virtual DbSet<MeetingMaterial> MeetingMaterials { get; set; }
        public virtual DbSet<MeetingParticipant> MeetingParticipants { get; set; }
        public virtual DbSet<MeetingRoom> MeetingRooms { get; set; }

        public virtual DbSet<Grower> Growers { get; set; }

        public virtual DbSet<Schedule> Schedules { get; set; }

        public virtual DbSet<ScheduleDetail> ScheduleDetails { get; set; }

        public virtual DbSet<ScheduleTask> ScheduleTasks { get; set; }

        public virtual DbSet<VisitTask> VisitTasks { get; set; }

        public virtual DbSet<TaskExamine> TaskExamines { get; set; }

        public virtual DbSet<VisitExamine> VisitExamines { get; set; }

        public virtual DbSet<VisitRecord> VisitRecords { get; set; }

    }
}
