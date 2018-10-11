using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using GYISMS.Authorization.Users;
using GYISMS.MultiTenancy;
using GYISMS.GYEnums;

namespace GYISMS
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class GYISMSAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected GYISMSAppServiceBase()
        {
            LocalizationSourceName = GYISMSConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected async Task<AreaCodeEnum?> GetCurrentUserAreaCodeAsync()
        {
            var user = await GetCurrentUserAsync();
            var roles = await UserManager.GetRolesAsync(user);
            if (roles.Contains("DistrictAdmin"))// 如果是区县管理员
            {
                return user.AreaCode.HasValue? user.AreaCode : AreaCodeEnum.None;
            }
            return null;
        }
    }
}
