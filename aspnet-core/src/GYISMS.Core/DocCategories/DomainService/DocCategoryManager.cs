

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
using GYISMS.DocCategories;


namespace GYISMS.DocCategories.DomainService
{
    /// <summary>
    /// DocCategory领域层的业务管理
    ///</summary>
    public class DocCategoryManager :GYISMSDomainServiceBase, IDocCategoryManager
    {
		
		private readonly IRepository<DocCategory,int> _repository;

		/// <summary>
		/// DocCategory的构造方法
		///</summary>
		public DocCategoryManager(
			IRepository<DocCategory, int> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitDocCategory()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
