using System.Threading.Tasks;
using Abp.Application.Services;
using GYISMS.Authorization.Accounts.Dto;

namespace GYISMS.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
