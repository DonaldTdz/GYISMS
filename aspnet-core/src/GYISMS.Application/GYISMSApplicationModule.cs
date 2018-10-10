using Abp.AutoMapper;
using Abp.Modules;
using Abp.Quartz;
using Abp.Reflection.Extensions;
using GYISMS.Authorization;

namespace GYISMS
{
    [DependsOn(
        typeof(GYISMSCoreModule), 
        typeof(AbpAutoMapperModule),
        typeof(AbpQuartzModule))]
    public class GYISMSApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<GYISMSAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(GYISMSApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
