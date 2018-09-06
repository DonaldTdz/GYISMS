
using System.Linq;
using Abp.Authorization;
using Abp.Localization;
using GYISMS.Authorization;

namespace GYISMS.SystemDatas.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="SystemDataAppPermissions" /> for all permission names. SystemData
    ///</summary>
    public class SystemDataAppAuthorizationProvider : AuthorizationProvider
    {
    public override void SetPermissions(IPermissionDefinitionContext context)
    {
    //在这里配置了SystemData 的权限。
    var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

    var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

    var systemdata = administration.CreateChildPermission(SystemDataAppPermissions.SystemData , L("SystemDatas"));
systemdata.CreateChildPermission(SystemDataAppPermissions.SystemData_Create, L("Create"));
systemdata.CreateChildPermission(SystemDataAppPermissions.SystemData_Edit, L("Edit"));
systemdata.CreateChildPermission(SystemDataAppPermissions.SystemData_Delete, L("Delete"));
systemdata.CreateChildPermission(SystemDataAppPermissions.SystemData_BatchDelete , L("BatchDelete"));
systemdata.CreateChildPermission(SystemDataAppPermissions.SystemData_ExportToExcel, L("ExportToExcel"));


    //// custom codes
    
    //// custom codes end
    }

    private static ILocalizableString L(string name)
    {
    return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
    }
    }
    }