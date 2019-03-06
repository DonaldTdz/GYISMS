

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Advises;


namespace GYISMS.Advises.DomainService
{
    public interface IAdviseManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitAdvise();



		 
      
         

    }
}
