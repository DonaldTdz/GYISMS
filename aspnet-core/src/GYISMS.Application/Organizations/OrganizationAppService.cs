
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
using GYISMS.Employees;
using GYISMS.Employees.Dtos;

namespace GYISMS.Organizations
{
    /// <summary>
    /// Organization应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class OrganizationAppService : GYISMSAppServiceBase, IOrganizationAppService
    {
        private readonly IRepository<Organization, long> _organizationRepository;
        private readonly IOrganizationManager _organizationManager;
        private readonly IRepository<Employee, string> _employeeRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public OrganizationAppService(IRepository<Organization, long> organizationRepository
            , IOrganizationManager organizationManager
            , IRepository<Employee, string> employeeRepository
            )
        {
            _organizationRepository = organizationRepository;
            _organizationManager = organizationManager;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取Organization的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrganizationListDto>> GetPagedOrganizationsAsync(GetOrganizationsInput input)
        {
            var query = _organizationRepository.GetAll()
                                .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.DepartmentName.Contains(input.Name)); ;
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
        //[AbpAuthorize(OrganizationAppPermissions.Organization_Create)]
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
        /// 同步组织架构&内部员工
        /// </summary>
        /// <returns></returns>
        public async Task<APIResultDto> SynchronousOrganizationAsync()
        {
            string accessToken = GetAccessToken();
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
            OapiDepartmentListRequest request = new OapiDepartmentListRequest();
            request.SetHttpMethod("GET");
            OapiDepartmentListResponse response = client.Execute(request, accessToken);
            var entityByDD = (from o in response.Department
                              select new OrganizationListDto()
                              {
                                  Id = o.Id,
                                  DepartmentName = o.Name,
                                  ParentId = o.Parentid,
                                  CreationTime = DateTime.Now
                              });

            var originEntity = await _organizationRepository.GetAll().ToListAsync();
            foreach (var item in entityByDD)
            {
                var o = originEntity.Where(r => r.Id == item.Id).FirstOrDefault();
                if (o != null)
                {
                    o.DepartmentName = item.DepartmentName;
                    o.ParentId = item.ParentId;
                    o.CreationTime = DateTime.Now;
                    if (o.Id != 1)
                    {
                        await SynchronousEmployeeAsync(o.Id, accessToken);
                    }
                }
                else
                {
                    var organization = new OrganizationListDto();
                    organization.Id = item.Id;
                    organization.DepartmentName = item.DepartmentName;
                    organization.ParentId = item.ParentId;
                    organization.CreationTime = DateTime.Now;
                    await CreateSyncOrganizationAsync(organization);
                    if (organization.Id != 1)
                    {
                        await SynchronousEmployeeAsync(organization.Id, accessToken);
                    }
                }
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return new APIResultDto() { Code = 0, Msg = "同步组织架构成功" };
        }

        /// <summary>
        /// 同步内部员工
        /// </summary>
        /// <param name="departId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private async Task<APIResultDto> SynchronousEmployeeAsync(long departId, string accessToken)
        {
            try
            {
                IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/list");
                OapiUserListRequest request = new OapiUserListRequest();
                request.DepartmentId = departId;
                request.SetHttpMethod("GET");
                OapiUserListResponse response = client.Execute(request, accessToken);
                var entityByDD = (from e in response.Userlist
                                  select new EmployeeListDto()
                                  {
                                      Id = e.Userid,
                                      Name = e.Name,
                                      Mobile = e.Mobile,
                                      Position = e.Position,
                                      Department = e.Department,
                                      IsAdmin = e.IsAdmin,
                                      IsBoss = e.IsBoss,
                                      Email = e.Email,
                                      HiredDate = e.HiredDate,
                                      Avatar = e.Avatar,
                                      Active = e.Active
                                  });
                var originEntity = await _employeeRepository.GetAll().ToListAsync();
                foreach (var item in entityByDD)
                {
                    var e = originEntity.Where(r => r.Id == item.Id).FirstOrDefault();
                    if (e != null)
                    {
                        e.Name = item.Name;
                        e.Mobile = item.Mobile;
                        e.Position = item.Position;
                        e.Department = item.Department;
                        e.IsAdmin = item.IsAdmin;
                        e.IsBoss = item.IsBoss;
                        e.Email = item.Email;
                        e.HiredDate = item.HiredDate;
                        e.Avatar = item.Avatar;
                        e.Active = item.Active;
                    }
                    else
                    {
                        var employee = new EmployeeListDto();
                        employee.Id = item.Id;
                        employee.Name = item.Name;
                        employee.Mobile = item.Mobile;
                        employee.Position = item.Position;
                        employee.Department = item.Department;
                        employee.IsAdmin = item.IsAdmin;
                        employee.IsBoss = item.IsBoss;
                        employee.Email = item.Email;
                        employee.HiredDate = item.HiredDate;
                        employee.Avatar = item.Avatar;
                        employee.Active = item.Active;
                        await CreateSyncEmployeeAsync(employee);
                    }
                }
                await CurrentUnitOfWork.SaveChangesAsync();
                return new APIResultDto() { Code = 0, Msg = "同步内部员工成功" };
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SynchronousEmployeeAsync errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "同步内部员工失败" };
            }
        }

        /// <summary>
        /// 插入组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<Organization> CreateSyncOrganizationAsync(OrganizationListDto input)
        {
            var entity = ObjectMapper.Map<Organization>(input);
            entity = await _organizationRepository.InsertAsync(entity);
            return entity.MapTo<Organization>();
        }

        private async Task<Employee> CreateSyncEmployeeAsync(EmployeeListDto input)
        {
            var entity = ObjectMapper.Map<Employee>(input);
            entity = await _employeeRepository.InsertAsync(entity);
            return entity.MapTo<Employee>();
        }

        /// <summary>
        /// 获取AccessToken ToDo钉钉配置
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
        /// 按需获取子节点
        /// </summary>
        public async Task<List<OrganizationNzTreeNode>> GetTreesAsync()
        {
            int? count = 0;
            var organizationList =await (from o in _organizationRepository.GetAll()
                                   select new OrganizationListDto()
                                   {
                                       Id = o.Id,
                                       DepartmentName = o.DepartmentName,
                                       ParentId = o.ParentId
                                   }).ToListAsync();
            foreach (var item in organizationList)
            {
                if (item.Id == 1)
                    count = await _employeeRepository.GetAll().CountAsync();
                else
                    count = await _employeeRepository.GetAll().Where(v => v.Department.Contains(item.Id.ToString())).CountAsync();
                item.Id = item.Id;
                item.ParentId = item.ParentId;
                item.DepartmentName = item.DepartmentName + $"({count}人)";
            }
            return GetTrees(0, organizationList);
        }

        private List<OrganizationNzTreeNode> GetTrees(long? id, List<OrganizationListDto> organizationList)
        {
            List<OrganizationNzTreeNode> treeNodeList = organizationList.Where(o => o.ParentId == id).Select(t => new OrganizationNzTreeNode()
            {
                key = t.Id.ToString(),
                title = t.DepartmentName,
                children = GetTrees(t.Id, organizationList)
            }).ToList();
            return treeNodeList;
        }
    }
}