
namespace GYISMS.Organizations.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="OrganizationAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class OrganizationAppPermissions
    {
    /// <summary>
        /// Organization管理权限_自带查询授权
        ///</summary>
    public const string Organization = "Pages.Organization";

    /// <summary>
        /// Organization创建权限
        ///</summary>
    public const string Organization_Create = "Pages.Organization.Create";

    /// <summary>
        /// Organization修改权限
        ///</summary>
    public const string Organization_Edit = "Pages.Organization.Edit";

    /// <summary>
        /// Organization删除权限
        ///</summary>
    public const string Organization_Delete = "Pages.Organization.Delete";

    /// <summary>
        /// Organization批量删除权限
        ///</summary>
    public const string Organization_BatchDelete  = "Pages.Organization.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string Organization_ExportToExcel = "Pages.Organization.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

