

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.TaskExamines;


namespace GYISMS.TaskExamines
{
    public interface ITaskExamineManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitTaskExamine();



		//// custom codes
 
        //// custom codes end

    }
}
