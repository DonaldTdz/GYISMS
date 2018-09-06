

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.VisitRecords;


namespace GYISMS.VisitRecords
{
    public interface IVisitRecordManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitVisitRecord();



		//// custom codes
 
        //// custom codes end

    }
}
