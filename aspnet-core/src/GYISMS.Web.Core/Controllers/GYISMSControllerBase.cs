using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace GYISMS.Controllers
{
    public abstract class GYISMSControllerBase: AbpController
    {
        protected GYISMSControllerBase()
        {
            LocalizationSourceName = GYISMSConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
