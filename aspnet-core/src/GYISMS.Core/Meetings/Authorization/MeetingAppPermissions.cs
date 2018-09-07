
namespace GYISMS.Meetings.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="MeetingAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class MeetingAppPermissions
    {
    /// <summary>
        /// Meeting管理权限_自带查询授权
        ///</summary>
    public const string Meeting = "Pages.Meeting";

    /// <summary>
        /// Meeting创建权限
        ///</summary>
    public const string Meeting_Create = "Pages.Meeting.Create";

    /// <summary>
        /// Meeting修改权限
        ///</summary>
    public const string Meeting_Edit = "Pages.Meeting.Edit";

    /// <summary>
        /// Meeting删除权限
        ///</summary>
    public const string Meeting_Delete = "Pages.Meeting.Delete";

    /// <summary>
        /// Meeting批量删除权限
        ///</summary>
    public const string Meeting_BatchDelete  = "Pages.Meeting.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string Meeting_ExportToExcel = "Pages.Meeting.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

