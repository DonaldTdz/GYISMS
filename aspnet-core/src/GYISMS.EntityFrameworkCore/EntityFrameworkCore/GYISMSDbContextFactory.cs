using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using GYISMS.Configuration;
using GYISMS.Web;

namespace GYISMS.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class GYISMSDbContextFactory : IDesignTimeDbContextFactory<GYISMSDbContext>
    {
        public GYISMSDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<GYISMSDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            GYISMSDbContextConfigurer.Configure(builder, configuration.GetConnectionString(GYISMSConsts.ConnectionStringName));

            return new GYISMSDbContext(builder.Options);
        }
    }
}
