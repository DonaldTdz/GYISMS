

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.VisitExamines;


namespace GYISMS.VisitExamines
{
    public interface IVisitExamineManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitVisitExamine();



		//// custom codes
 
        //// custom codes end

    }
}
