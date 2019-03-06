

namespace GYISMS.Advises.Authorization
{
	/// <summary>
    /// 定义系统的权限名称的字符串常量。
    /// <see cref="AdviseAuthorizationProvider" />中对权限的定义.
    ///</summary>
	public static  class AdvisePermissions
	{
		/// <summary>
		/// Advise权限节点
		///</summary>
		public const string Node = "Pages.Advise";

		/// <summary>
		/// Advise查询授权
		///</summary>
		public const string Query = "Pages.Advise.Query";

		/// <summary>
		/// Advise创建权限
		///</summary>
		public const string Create = "Pages.Advise.Create";

		/// <summary>
		/// Advise修改权限
		///</summary>
		public const string Edit = "Pages.Advise.Edit";

		/// <summary>
		/// Advise删除权限
		///</summary>
		public const string Delete = "Pages.Advise.Delete";

        /// <summary>
		/// Advise批量删除权限
		///</summary>
		public const string BatchDelete = "Pages.Advise.BatchDelete";

		/// <summary>
		/// Advise导出Excel
		///</summary>
		public const string ExportExcel="Pages.Advise.ExportExcel";

		 
		 
         
    }

}

