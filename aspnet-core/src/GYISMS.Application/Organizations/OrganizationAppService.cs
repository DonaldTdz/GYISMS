
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
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using GYISMS.Dtos;
using GYISMS.Authorization;

namespace GYISMS.Organizations
{
    /// <summary>
    /// Organization应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class OrganizationAppService : GYISMSAppServiceBase, IOrganizationAppService
    {
        private readonly IRepository<Organization, long>
        _organizationRepository;


        private readonly IOrganizationManager _organizationManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public OrganizationAppService(
        IRepository<Organization, long>
    organizationRepository
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
        public async Task<OrganizationListDto> GetOrganizationByIdAsync(EntityDto<long> input)
        {
            var entity = await _organizationRepository.GetAsync(input.Id);

            return entity.MapTo<OrganizationListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetOrganizationForEditOutput> GetOrganizationForEdit(NullableIdDto<long> input)
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
        [AbpAuthorize(OrganizationAppPermissions.Organization_Edit)]
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
        public async Task DeleteOrganization(EntityDto<long> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _organizationRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Organization的方法
        /// </summary>
        [AbpAuthorize(OrganizationAppPermissions.Organization_BatchDelete)]
        public async Task BatchDeleteOrganizationsAsync(List<long> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _organizationRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 通过接口获取钉钉组织架构
        /// </summary>
        /// <returns></returns>
        //public List<Organization> GetOrganization()
        //{
        //    string accessToken = "de975eff4b473259ac5fa342dbfdeae7";
        //    IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
        //    OapiDepartmentListRequest request = new OapiDepartmentListRequest();
        //    request.Id = "1";
        //    request.SetHttpMethod("GET");
        //    OapiDepartmentListResponse response = client.Execute(request, accessToken);
        //    var entity = (from o in response.Department
        //                  select new Organization()
        //                  {
        //                      Id = o.Id,
        //                      DepartmentName = o.Name,
        //                      ParentId = o.Parentid,
        //                  }).ToList();
        //    return entity;
        //}



        /// <summary>
        /// 同步组织架构
        /// </summary>
        /// <returns></returns>
        public async Task<APIResultDto> SynchronousOrganizationAsync()
        {
            string accessToken = GetAccessToken();
            //string accessToken = "0929f705e9c93c3ba237c984b8522177";
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
            OapiDepartmentListRequest request = new OapiDepartmentListRequest();
            //request.Id = "1";
            request.SetHttpMethod("GET");
            OapiDepartmentListResponse response = client.Execute(request, accessToken);
            var entityByDD = (from o in response.Department
                              select new Organization()
                              {
                                  Id = o.Id,
                                  DepartmentName = o.Name,
                                  ParentId = o.Parentid,
                                  CreationTime = DateTime.Now
                              }).ToList();

            var originEntity = await _organizationRepository.GetAll().ToListAsync();
            foreach (var item in entityByDD)
            {
                var o = originEntity.Where(r => r.Id == item.Id).FirstOrDefault();
                if (o != null)
                {
                    o.Id = item.Id;
                    o.DepartmentName = item.DepartmentName;
                    o.ParentId = item.ParentId;
                    o.CreationTime = DateTime.Now;
                }
                else
                {
                    var organization = new Organization();
                    organization.Id = item.Id;
                    organization.DepartmentName = item.DepartmentName;
                    organization.ParentId = item.ParentId;
                    organization.CreationTime = DateTime.Now;
                    await CreateOrganizationAsync(organization);
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return new APIResultDto() { Code = 0, Msg = "同步组织架构成功" };
        }

        /// <summary>
        /// 插入组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<Organization> CreateOrganizationAsync(Organization input)
        {
            var entity = ObjectMapper.Map<Organization>(input);
            entity = await _organizationRepository.InsertAsync(entity);
            return entity.MapTo<Organization>();
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        private string GetAccessToken()
        {
            DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
            OapiGettokenRequest request = new OapiGettokenRequest();
            request.Appkey = "ding7xespi5yumrzraaq";
            request.Appsecret = "idKPu4wVaZjBKo6oUvxcwSQB7tExjEbPaBpVpCEOGlcZPsH4BDx-sKilG726-nC3";
            request.SetHttpMethod("GET");
            OapiGettokenResponse response = client.Execute(request);
            return response.AccessToken;
        }

        /// <summary>
        /// 获取根节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<NzTreeNode> GetRootTree(long? id)
        {
            var organiztion = await _organizationRepository.GetAll().Where(v => v.Id == id).FirstOrDefaultAsync();
            NzTreeNode treeNode = new NzTreeNode();
            treeNode.title = organiztion.DepartmentName;
            treeNode.key = organiztion.Id.ToString();
            return treeNode;
        }

        /// <summary>
        /// 按需获取子节点
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<NzTreeNode>> GetChildTree(string id)
        {
            var orgChild = await _organizationRepository.GetAll().Where(v => v.ParentId == Convert.ToInt32(id)).ToListAsync();
            List<NzTreeNode> treeNodeList = new List<NzTreeNode>();
            foreach (var item in orgChild)
            {
                NzTreeNode treeNode = new NzTreeNode();
                treeNode.title = item.DepartmentName;
                treeNode.key = item.Id.ToString();
                treeNodeList.Add(treeNode);
            }
            return treeNodeList;
        }
    }
}


