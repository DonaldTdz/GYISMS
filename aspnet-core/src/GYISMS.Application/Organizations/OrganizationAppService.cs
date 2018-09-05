
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;

using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

using GYISMS.Organizations.Authorization;
using GYISMS.Organizations.Dtos;
using GYISMS.Organizations;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using GYISMS.Dtos;

namespace GYISMS.Organizations
{
    /// <summary>
    /// Organization应用层服务的接口实现方法  
    ///</summary>
//[AbpAuthorize(OrganizationAppPermissions.Organization)] 
    public class OrganizationAppService : GYISMSAppServiceBase, IOrganizationAppService
    {
        private readonly IRepository<Organization, int>
        _organizationRepository;


        private readonly IOrganizationManager _organizationManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public OrganizationAppService(
        IRepository<Organization, int> organizationRepository
            , IOrganizationManager organizationManager
            )
        {
            _organizationRepository = organizationRepository;
            _organizationManager = organizationManager;
        }


        /// <summary>
        /// 获取Organization的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationListDto>> GetPagedOrganizationsAsync(GetOrganizationsInput input)
        {
            var query = _organizationRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var organizationCount = await query.CountAsync();
            var organizations = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var organizationListDtos = ObjectMapper.Map<List <OrganizationListDto>>(organizations);
            var organizationListDtos = organizations.MapTo<List<OrganizationListDto>>();
            return new PagedResultDto<OrganizationListDto>(
                    organizationCount,
                organizationListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取OrganizationListDto信息
        /// </summary>
        public async Task<OrganizationListDto> GetOrganizationByIdAsync(EntityDto<int> input)
        {
            var entity = await _organizationRepository.GetAsync(input.Id);

            return entity.MapTo<OrganizationListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetOrganizationForEditOutput> GetOrganizationForEdit(NullableIdDto<int> input)
        {
            var output = new GetOrganizationForEditOutput();
            OrganizationEditDto organizationEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _organizationRepository.GetAsync(input.Id.Value);

                organizationEditDto = entity.MapTo<OrganizationEditDto>();

                //organizationEditDto = ObjectMapper.Map<List <organizationEditDto>>(entity);
            }
            else
            {
                organizationEditDto = new OrganizationEditDto();
            }

            output.Organization = organizationEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Organization的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateOrganization(CreateOrUpdateOrganizationInput input)
        {

            if (input.Organization.Id.HasValue)
            {
                await UpdateOrganizationAsync(input.Organization);
            }
            else
            {
                await CreateOrganizationAsync(input.Organization);
            }
        }


        /// <summary>
        /// 新增Organization
        /// </summary>
        [AbpAuthorize(OrganizationAppPermissions.Organization_Create)]
        protected virtual async Task<OrganizationEditDto> CreateOrganizationAsync(OrganizationEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Organization>(input);

            entity = await _organizationRepository.InsertAsync(entity);
            return entity.MapTo<OrganizationEditDto>();
        }

        /// <summary>
        /// 编辑Organization
        /// </summary>
        //[AbpAuthorize(OrganizationAppPermissions.Organization_Edit)]
        protected virtual async Task UpdateOrganizationAsync(OrganizationEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _organizationRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _organizationRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Organization信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(OrganizationAppPermissions.Organization_Delete)]
        public async Task DeleteOrganization(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _organizationRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Organization的方法
        /// </summary>
        [AbpAuthorize(OrganizationAppPermissions.Organization_BatchDelete)]
        public async Task BatchDeleteOrganizationsAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _organizationRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 通过接口获取钉钉组织架构
        /// </summary>
        /// <returns></returns>
        public List<Organization> GetOrganization()
        {
            string accessToken = "de975eff4b473259ac5fa342dbfdeae7";
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
            OapiDepartmentListRequest request = new OapiDepartmentListRequest();
            request.Id = "1";
            request.SetHttpMethod("GET");
            OapiDepartmentListResponse response = client.Execute(request, accessToken);
            var entity = (from o in response.Department
                          select new Organization()
                          {
                              Id = (int)o.Id,
                              DepartmentName = o.Name,
                              ParentId = (int)o.Parentid,
                              IsDeleted = false
                          }).ToList();
            return entity;
        }

        //public async Task<PagedResultDto<OrganizationListDto>> GetOrganizationAsync(GetOrganizationsInput input)
        //{
        //    string accessToken = "de975eff4b473259ac5fa342dbfdeae7";
        //    IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
        //    OapiDepartmentListRequest request = new OapiDepartmentListRequest();
        //    request.Id = "1";
        //    request.SetHttpMethod("GET");
        //    OapiDepartmentListResponse response = client.Execute(request, accessToken);
        //    var entity = from o in response.Department
        //                  select new Organization()
        //                  {
        //                      Id = (int)o.Id,
        //                      DepartmentName = o.Name,
        //                      ParentId = (int)o.Parentid,
        //                      IsDeleted = false
        //                  };
        //    var organizationCount = entity.Count();

        //    var organizations =  entity
        //            .OrderBy(input.Sorting).AsNoTracking()
        //            .PageBy(input)
        //            .ToList();

        //    // var organizationListDtos = ObjectMapper.Map<List <OrganizationListDto>>(organizations);
        //    var organizationListDtos = organizations.MapTo<List<OrganizationListDto>>();

        //    return new PagedResultDto<OrganizationListDto>(
        //        organizationCount,
        //        organizationListDtos
        //        );
        //    return entity;
        //}

        public async Task<APIResultDto> SynchronousOrganizationAsync()
        {
            return new APIResultDto() { Code = 0, Msg = "成功" };
        }

        /// <summary>
        /// 同步组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SynchronousOrganizationAsync(OrganizationEditDto input)
        {
            var list = GetOrganization();
            var organization = new Organization();
            foreach (var item in list)
            {
                organization.Id = item.Id;
                organization.DepartmentName = item.DepartmentName;
                organization.IsDeleted = item.IsDeleted;
            }
            //if (input.Id.HasValue)
            //{
            //    await UpdateOrganizationAsync(input);
            //}
            //else
            //{
            //    await CreateOrganizationAsync(input);
            //}
        }
    }
}


