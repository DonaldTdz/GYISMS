using System.Threading.Tasks;
using Abp.Application.Services;
using GYISMS.Sessions.Dto;

namespace GYISMS.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
