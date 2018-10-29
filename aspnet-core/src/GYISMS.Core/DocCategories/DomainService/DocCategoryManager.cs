

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
		
		private readonly IRepository<DocCategory> _repository;

		/// <summary>
		/// DocCategory的构造方法
		///</summary>
		public DocCategoryManager(
			IRepository<DocCategory> repository
		)
		{
			_repository =  repository;
		}

        /// <summary>
        /// 初始化
        ///</summary>
        public void InitDocCategory()
		{
			
		}

        private async Task GetParentCategoryAsync(int id, List<DocCategory> plist)
        {
            var entity = await _repository.GetAsync(id);
            if (entity != null)
            {
                plist.Insert(0, entity);
                if (entity.ParentId.HasValue && entity.ParentId != 0)
                {
                    await GetParentCategoryAsync(entity.ParentId.Value, plist);
                }
            }
        }

        public async Task<List<DocCategory>> GetHierarchyCategories(int id)
        {
            var plist = new List<DocCategory>();
            await GetParentCategoryAsync(id, plist);
            return plist;
        }


        // TODO:编写领域业务代码







    }
}
