

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.Growers;


namespace GYISMS.Growers
{
    public interface IGrowerManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitGrower();



		//// custom codes
 
        //// custom codes end

    }
}
