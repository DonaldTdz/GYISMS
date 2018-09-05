using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace GYISMS.EntityFrameworkCore
{
    public static class GYISMSDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<GYISMSDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<GYISMSDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
