
namespace GYISMS.Schedules.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="ScheduleAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class ScheduleAppPermissions
    {
    /// <summary>
        /// Schedule管理权限_自带查询授权
        ///</summary>
    public const string Schedule = "Pages.Schedule";

    /// <summary>
        /// Schedule创建权限
        ///</summary>
    public const string Schedule_Create = "Pages.Schedule.Create";

    /// <summary>
        /// Schedule修改权限
        ///</summary>
    public const string Schedule_Edit = "Pages.Schedule.Edit";

    /// <summary>
        /// Schedule删除权限
        ///</summary>
    public const string Schedule_Delete = "Pages.Schedule.Delete";

    /// <summary>
        /// Schedule批量删除权限
        ///</summary>
    public const string Schedule_BatchDelete  = "Pages.Schedule.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string Schedule_ExportToExcel = "Pages.Schedule.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

