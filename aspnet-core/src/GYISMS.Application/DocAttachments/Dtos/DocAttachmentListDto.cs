

using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using GYISMS.DocAttachments;
using GYISMS.GYEnums;

namespace GYISMS.DocAttachments.Dtos
{
    public class DocAttachmentListDto : FullAuditedEntityDto 
    {

        
		/// <summary>
		/// Name
		/// </summary>
		[Required(ErrorMessage="Name不能为空")]
		public string Name { get; set; }



		/// <summary>
		/// FileType
		/// </summary>
		public FileTypeEnum? FileType { get; set; }



		/// <summary>
		/// FileSize
		/// </summary>
		public decimal? FileSize { get; set; }



		/// <summary>
		/// Path
		/// </summary>
		[Required(ErrorMessage="Path不能为空")]
		public string Path { get; set; }



		/// <summary>
		/// DocId
		/// </summary>
		public Guid DocId { get; set; }




    }
}