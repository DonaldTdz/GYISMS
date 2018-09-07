
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.Growers.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="GrowerAppPermissions" /> for all permission names. Grower
    ///</summary>
    public class GrowerAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了Grower 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var grower = administration.CreateChildPermission(GrowerAppPermissions.Grower , L("Growers"));
grower.CreateChildPermission(GrowerAppPermissions.Grower_Create, L("Create"));
grower.CreateChildPermission(GrowerAppPermissions.Grower_Edit, L("Edit"));
grower.CreateChildPermission(GrowerAppPermissions.Grower_Delete, L("Delete"));
grower.CreateChildPermission(GrowerAppPermissions.Grower_BatchDelete , L("BatchDelete"));
grower.CreateChildPermission(GrowerAppPermissions.Grower_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }