using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Domain.Repositories;
using GYISMS.Employees;
using GYISMS.Sessions.Dto;

namespace GYISMS.Sessions
{
    public class SessionAppService : GYISMSAppServiceBase, ISessionAppService
    {
        private readonly IRepository<Employee, string> _employeeRepository;

        public SessionAppService(IRepository<Employee, string> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

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
                if (!string.IsNullOrEmpty(output.User.EmployeeId))
                {
                    var avatar = await _employeeRepository.GetAsync(output.User.EmployeeId);
                    output.User.Avatar = avatar.Avatar;
                }
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
