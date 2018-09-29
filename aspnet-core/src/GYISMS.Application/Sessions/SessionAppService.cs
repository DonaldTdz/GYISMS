using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using GYISMS.Sessions.Dto;

namespace GYISMS.Sessions
{
    public class SessionAppService : GYISMSAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                Application = new ApplicationInfoDto
                {
                    Version = AppVersionHelper.Version,
                    ReleaseDate = AppVersionHelper.ReleaseDate,
                    Features = new Dictionary<string, bool>()
                }
            };

            if (AbpSession.TenantId.HasValue)
            {
                output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
            }

            if (AbpSession.UserId.HasValue)
            {
                var user = await GetCurrentUserAsync();
                output.User = ObjectMapper.Map<UserLoginInfoDto>(user);
                output.Roles = await UserManager.GetRolesAsync(user);
                if (!AbpSession.TenantId.HasValue)
                {
                    for (int i=0;i<output.Roles.Count;i++)
                    {
                        output.Roles[i] = "Host" + output.Roles[i];
                    }
                }
            }

            return output;
        }
    }
}
