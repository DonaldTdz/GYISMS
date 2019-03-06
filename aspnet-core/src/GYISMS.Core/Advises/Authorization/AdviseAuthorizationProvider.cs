

using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;
using GYISMS.Authorization;

namespace GYISMS.Advises.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="AdvisePermissions" /> for all permission names. Advise
    ///</summary>
    public class AdviseAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

		public AdviseAuthorizationProvider()
		{

		}

        public AdviseAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AdviseAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// 在这里配置了Advise 的权限。
			var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

			var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ?? pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

			var entityPermission = administration.CreateChildPermission(AdvisePermissions.Node , L("Advise"));
			entityPermission.CreateChildPermission(AdvisePermissions.Query, L("QueryAdvise"));
			entityPermission.CreateChildPermission(AdvisePermissions.Create, L("CreateAdvise"));
			entityPermission.CreateChildPermission(AdvisePermissions.Edit, L("EditAdvise"));
			entityPermission.CreateChildPermission(AdvisePermissions.Delete, L("DeleteAdvise"));
			entityPermission.CreateChildPermission(AdvisePermissions.BatchDelete, L("BatchDeleteAdvise"));
			entityPermission.CreateChildPermission(AdvisePermissions.ExportExcel, L("ExportExcelAdvise"));


		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, GYISMSConsts.LocalizationSourceName);
		}
    }
}