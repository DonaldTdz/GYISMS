
using Abp.Domain.Services;

namespace GYISMS
{
	public abstract class GYISMSDomainServiceBase : DomainService
	{
		/* Add your common members for all your domain services. */
		/*在领域服务中添加你的自定义公共方法*/
		
		//// custom codes
		
		//// custom codes end
		
		protected GYISMSDomainServiceBase()
		{
			LocalizationSourceName = GYISMSConsts.LocalizationSourceName;
		}
	}
}
