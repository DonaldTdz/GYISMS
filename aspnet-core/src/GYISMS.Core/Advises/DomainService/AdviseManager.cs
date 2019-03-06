

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
using GYISMS.Advises;


namespace GYISMS.Advises.DomainService
{
    /// <summary>
    /// Advise领域层的业务管理
    ///</summary>
    public class AdviseManager :GYISMSDomainServiceBase, IAdviseManager
    {
		
		private readonly IRepository<Advise,Guid> _repository;

		/// <summary>
		/// Advise的构造方法
		///</summary>
		public AdviseManager(
			IRepository<Advise, Guid> repository
		)
		{
			_repository =  repository;
		}


		/// <summary>
		/// 初始化
		///</summary>
		public void InitAdvise()
		{
			throw new NotImplementedException();
		}

		// TODO:编写领域业务代码



		 
		  
		 

	}
}
