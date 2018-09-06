using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace GYISMS.EntityFrameworkCore
{
    public static class GYISMSDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<GYISMSDbContext> builder, string connectionString)
        {
            //builder.UseSqlServer(connectionString);
            builder.UseSqlServer(connectionString, option => option.UseRowNumberForPaging());
        }

        public static void Configure(DbContextOptionsBuilder<GYISMSDbContext> builder, DbConnection connection)
        {
            //builder.UseSqlServer(connection);
            builder.UseSqlServer(connection, option => option.UseRowNumberForPaging());
        }
    }
}
