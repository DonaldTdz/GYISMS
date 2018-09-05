using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using GYISMS.Authorization.Roles;
using GYISMS.Authorization.Users;
using GYISMS.MultiTenancy;
using GYISMS.Organizations;

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

    }
}
