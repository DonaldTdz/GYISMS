

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.GYEnums;
using GYISMS.SystemDatas;


namespace GYISMS.SystemDatas
{
    public interface ISystemDataManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitSystemData();

    }
}
