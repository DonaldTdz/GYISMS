

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Documents;


namespace GYISMS.Documents.DomainService
{
    public interface IDocumentManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitDocument();



		 
      
         

    }
}
