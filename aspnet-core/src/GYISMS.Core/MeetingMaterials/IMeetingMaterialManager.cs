

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.MeetingMaterials;


namespace GYISMS.MeetingMaterials
{
    public interface IMeetingMaterialManager : IDomainService
    {

        /// <summary>
    /// 初始化方法
    ///</summary>
        void InitMeetingMaterial();



		//// custom codes
 
        //// custom codes end

    }
}
