
namespace GYISMS.MeetingParticipants.Authorization
{

	 /// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="MeetingParticipantAppAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class MeetingParticipantAppPermissions
    {
    /// <summary>
        /// MeetingParticipant管理权限_自带查询授权
        ///</summary>
    public const string MeetingParticipant = "Pages.MeetingParticipant";

    /// <summary>
        /// MeetingParticipant创建权限
        ///</summary>
    public const string MeetingParticipant_Create = "Pages.MeetingParticipant.Create";

    /// <summary>
        /// MeetingParticipant修改权限
        ///</summary>
    public const string MeetingParticipant_Edit = "Pages.MeetingParticipant.Edit";

    /// <summary>
        /// MeetingParticipant删除权限
        ///</summary>
    public const string MeetingParticipant_Delete = "Pages.MeetingParticipant.Delete";

    /// <summary>
        /// MeetingParticipant批量删除权限
        ///</summary>
    public const string MeetingParticipant_BatchDelete  = "Pages.MeetingParticipant.BatchDelete";

	  /// <summary>
        /// 导出为Excel表
        ///</summary>
    public const string MeetingParticipant_ExportToExcel = "Pages.MeetingParticipant.ExportToExcel";


    //// custom codes
    
    //// custom codes end
    }

}

