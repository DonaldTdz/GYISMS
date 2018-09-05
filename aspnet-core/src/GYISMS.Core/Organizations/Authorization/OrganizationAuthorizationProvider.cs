
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.Organizations.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="OrganizationAppPermissions" /> for all permission names. Organization
    ///</summary>
    public class OrganizationAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了Organization 的权限。
    var pages = context.GetPermissionOrNull(AppLtmPermissions.Pages) ?? context.CreatePermission(AppLtmPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppLtmPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppLtmPermissions.Pages_Administration, L("Administration"));

    var organization = administration.CreateChildPermission(OrganizationAppPermissions.Organization , L("Organizations"));
organization.CreateChildPermission(OrganizationAppPermissions.Organization_Create, L("Create"));
organization.CreateChildPermission(OrganizationAppPermissions.Organization_Edit, L("Edit"));
organization.CreateChildPermission(OrganizationAppPermissions.Organization_Delete, L("Delete"));
organization.CreateChildPermission(OrganizationAppPermissions.Organization_BatchDelete , L("BatchDelete"));
organization.CreateChildPermission(OrganizationAppPermissions.Organization_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }