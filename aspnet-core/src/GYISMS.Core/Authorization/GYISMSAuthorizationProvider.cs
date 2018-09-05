using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace GYISMS.Authorization
{
    public class GYISMSAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            context.CreatePermission(AppPermissions.Pages, L("Pages"));
            context.CreatePermission(AppPermissions.Pages_Administration, L("Administration"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
        }
    }
}
