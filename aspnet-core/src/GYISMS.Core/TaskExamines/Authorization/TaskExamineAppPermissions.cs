
namespace GYISMS.TaskExamines.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="TaskExamineAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class TaskExamineAppPermissions
    {
    /// <summary>
        /// TaskExamine管理权限_自带查询授权
        ///</summary>
    public const string TaskExamine = "Pages.TaskExamine";

    /// <summary>
        /// TaskExamine创建权限
        ///</summary>
    public const string TaskExamine_Create = "Pages.TaskExamine.Create";

    /// <summary>
        /// TaskExamine修改权限
        ///</summary>
    public const string TaskExamine_Edit = "Pages.TaskExamine.Edit";

    /// <summary>
        /// TaskExamine删除权限
        ///</summary>
    public const string TaskExamine_Delete = "Pages.TaskExamine.Delete";

    /// <summary>
        /// TaskExamine批量删除权限
        ///</summary>
    public const string TaskExamine_BatchDelete  = "Pages.TaskExamine.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string TaskExamine_ExportToExcel = "Pages.TaskExamine.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

