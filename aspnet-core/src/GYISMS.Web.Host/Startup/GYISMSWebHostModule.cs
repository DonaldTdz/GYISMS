using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using GYISMS.Configuration;

namespace GYISMS.Web.Host.Startup
{
    [DependsOn(
       typeof(GYISMSWebCoreModule))]
    public class GYISMSWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public GYISMSWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(GYISMSWebHostModule).GetAssembly());
        }
    }
}
