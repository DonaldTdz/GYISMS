

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.VisitTasks;


namespace GYISMS.VisitTasks
{
    public interface IVisitTaskManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitVisitTask();



		//// custom codes
 
        //// custom codes end

    }
}
