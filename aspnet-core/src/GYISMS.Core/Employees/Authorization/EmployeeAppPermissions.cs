
namespace GYISMS.Employees.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="EmployeeAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class EmployeeAppPermissions
    {
    /// <summary>
        /// Employee管理权限_自带查询授权
        ///</summary>
    public const string Employee = "Pages.Employee";

    /// <summary>
        /// Employee创建权限
        ///</summary>
    public const string Employee_Create = "Pages.Employee.Create";

    /// <summary>
        /// Employee修改权限
        ///</summary>
    public const string Employee_Edit = "Pages.Employee.Edit";

    /// <summary>
        /// Employee删除权限
        ///</summary>
    public const string Employee_Delete = "Pages.Employee.Delete";

    /// <summary>
        /// Employee批量删除权限
        ///</summary>
    public const string Employee_BatchDelete  = "Pages.Employee.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string Employee_ExportToExcel = "Pages.Employee.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

