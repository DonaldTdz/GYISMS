

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Abp.Linq;
using Abp.Linq.Extensions;
using Abp.Extensions;
using Abp.UI;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

using GYISMS;
using GYISMS.DocAttachments;


namespace GYISMS.DocAttachments.DomainService
{
    /// <summary>
    /// DocAttachment领域层的业务管理
    ///</summary>
    public class DocAttachmentManager :GYISMSDomainServiceBase, IDocAttachmentManager
    {
		
		private readonly IRepository<DocAttachment,Guid> _repository;

		/// <summary>
		/// DocAttachment的构造方法
		///</summary>
		public DocAttachmentManager(
			IRepository<DocAttachment, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitDocAttachment()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
