using Abp.Application.Services;
using Abp.Application.Services.Dto;
using GYISMS.MultiTenancy.Dto;

namespace GYISMS.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
