
using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace  GYISMS.DocCategories.Dtos
{
    [AutoMapTo(typeof(DocCategory))]
    public class DocCategoryEditDto : FullAuditedEntityDto<int?>
    {
		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }

		/// <summary>
		/// ParentId
		/// </summary>
		public int? ParentId { get; set; }

		/// <summary>
		/// Desc
		/// </summary>
		public string Desc { get; set; }

        /// <summary>
        /// 维护部门Id
        /// </summary>
        public long? DeptId { get; set; }

    }
}