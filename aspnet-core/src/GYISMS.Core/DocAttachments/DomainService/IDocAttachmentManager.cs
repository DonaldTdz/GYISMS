

using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Services;
using GYISMS.DocAttachments;


namespace GYISMS.DocAttachments.DomainService
{
    public interface IDocAttachmentManager : IDomainService
    {

        /// <summary>
        /// 初始化方法
        ///</summary>
        void InitDocAttachment();



		 
      
         

    }
}
