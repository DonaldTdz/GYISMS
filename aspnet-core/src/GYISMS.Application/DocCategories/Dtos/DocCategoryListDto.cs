

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using GYISMS.DocCategories;
using Abp.AutoMapper;

namespace GYISMS.DocCategories.Dtos
{
    [AutoMapFrom(typeof(DocCategory))]
    public class DocCategoryListDto : FullAuditedEntityDto 
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

        public string Summary { get; set; }
    }

    public class GridListDto
    {
        public string Icon { get; set; }
        public string Text { get; set; }
        public int Id { get; set; }
    }

    public class TabListDto
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public int? ParentId { get; set; }
    }
}