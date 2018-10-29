

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.DocCategories;


namespace GYISMS.DocCategories.DomainService
{
    public interface IDocCategoryManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitDocCategory();



        Task<List<DocCategory>> GetHierarchyCategories(int id);


    }
}
