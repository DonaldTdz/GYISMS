
namespace GYISMS.SystemDatas.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="SystemDataAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class SystemDataAppPermissions
    {
    /// <summary>
        /// SystemData管理权限_自带查询授权
        ///</summary>
    public const string SystemData = "Pages.SystemData";

    /// <summary>
        /// SystemData创建权限
        ///</summary>
    public const string SystemData_Create = "Pages.SystemData.Create";

    /// <summary>
        /// SystemData修改权限
        ///</summary>
    public const string SystemData_Edit = "Pages.SystemData.Edit";

    /// <summary>
        /// SystemData删除权限
        ///</summary>
    public const string SystemData_Delete = "Pages.SystemData.Delete";

    /// <summary>
        /// SystemData批量删除权限
        ///</summary>
    public const string SystemData_BatchDelete  = "Pages.SystemData.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string SystemData_ExportToExcel = "Pages.SystemData.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

