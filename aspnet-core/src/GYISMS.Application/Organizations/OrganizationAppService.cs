
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
using Abp.Auditing;
using GYISMS.DingDing;
using GYISMS.DingDing.Dtos;
using GYISMS.GYEnums;
using GYISMS.SystemDatas;
using Senparc.CO2NET.HttpUtility;

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
        private readonly IRepository<SystemData> _systemDataRepository;
        private readonly IDingDingAppService _dingDingAppService;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public OrganizationAppService(IRepository<Organization, long> organizationRepository
            , IOrganizationManager organizationManager
            , IRepository<Employee, string> employeeRepository
            , IDingDingAppService dingDingAppService
            , IRepository<SystemData> systemDataRepository
            )
        {
            _organizationRepository = organizationRepository;
            _organizationManager = organizationManager;
            _employeeRepository = employeeRepository;
            _dingDingAppService = dingDingAppService;
            _systemDataRepository = systemDataRepository;
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

        private AreaCodeArray GetAreaCodeArray()
        {
            AreaCodeArray array = new AreaCodeArray();
            var zhqpids = _systemDataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == GYCode.ZHQPID).Select(s => s.Desc).FirstOrDefault();
            if (zhqpids != null)
            {
                array.ZHQPIDArray = Array.ConvertAll(zhqpids.Split(','), z => long.Parse(z));
            }
            else
            {
                array.ZHQPIDArray = new long[0];
            }
            var jgxpids = _systemDataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == GYCode.JGXPID).Select(s => s.Desc).FirstOrDefault();
            if (jgxpids != null)
            {
                array.JGXPIDArray = Array.ConvertAll(jgxpids.Split(','), z => long.Parse(z));
            }
            else
            {
                array.JGXPIDArray = new long[0];
            }
            var wcxpids = _systemDataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == GYCode.WCXPID).Select(s => s.Desc).FirstOrDefault();
            if (wcxpids != null)
            {
                array.WCXPIDArray = Array.ConvertAll(wcxpids.Split(','), z => long.Parse(z));
            }
            else
            {
                array.WCXPIDArray = new long[0];
            }
            return array;
        }

        /// <summary>
        /// 同步组织架构&内部员工
        /// </summary>
        public async Task<APIResultDto> SynchronousOrganizationAsync()
        {
            //var arr = GetAreaCodeArray();  取消区县更新 改为区县配置
            string accessToken = _dingDingAppService.GetAccessTokenByApp(DingDingAppEnum.会议申请); //GetAccessToken();
            //IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
            //OapiDepartmentListRequest request = new OapiDepartmentListRequest();
            //request.SetHttpMethod("GET");
            //OapiDepartmentListResponse response = client.Execute(request, accessToken);
            var depts = Get.GetJson<DingDepartmentDto>(string.Format("https://oapi.dingtalk.com/department/list?access_token={0}", accessToken));
            var entityByDD = depts.department.Select(o => new OrganizationListDto()
                              {
                                  Id = o.id,
                                  DepartmentName = o.name,
                                  ParentId = o.parentid,
                                  CreationTime = DateTime.Now
                              }).ToList();

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
        private async Task<APIResultDto> SynchronousEmployeeAsync(long departId, string accessToken)
        {
            try
            {
                /*IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/list");
                OapiUserListRequest request = new OapiUserListRequest();
                request.DepartmentId = departId;
                request.SetHttpMethod("GET");
                OapiUserListResponse response = client.Execute(request, accessToken);*/
                var url = string.Format("https://oapi.dingtalk.com/user/list?access_token={0}&department_id={1}", accessToken, departId);
                var user = Get.GetJson<DingUserListDto>(url);
                var entityByDD = user.userlist.Select(e => new EmployeeListDto()
                                  {
                                      Id = e.userid,
                                      Name = e.name,
                                      Mobile = e.mobile,
                                      Position = e.position,
                                      Department = e.departmentStr,
                                      IsAdmin = e.isAdmin,
                                      IsBoss = e.isBoss,
                                      Email = e.email,
                                      HiredDate = e.hiredDate,
                                      Avatar = e.avatar,
                                      Active = e.active
                                  }).ToList();
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
                        //if (pidArr.ZHQPIDArray.Contains(departId))
                        //{
                        //    e.Area = AreaCodeEnum.昭化区.ToString();
                        //    e.AreaCode = AreaCodeEnum.昭化区;
                        //}
                        //else if (pidArr.JGXPIDArray.Contains(departId))
                        //{
                        //    e.Area = AreaCodeEnum.剑阁县.ToString();
                        //    e.AreaCode = AreaCodeEnum.剑阁县;
                        //}
                        //else if (pidArr.WCXPIDArray.Contains(departId))
                        //{
                        //    e.Area = AreaCodeEnum.旺苍县.ToString();
                        //    e.AreaCode = AreaCodeEnum.旺苍县;
                        //}
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
                        //if (pidArr.ZHQPIDArray.Contains(departId))
                        //{
                        //    e.Area = AreaCodeEnum.昭化区.ToString();
                        //    e.AreaCode = AreaCodeEnum.昭化区;
                        //}
                        //else if (pidArr.JGXPIDArray.Contains(departId))
                        //{
                        //    e.Area = AreaCodeEnum.剑阁县.ToString();
                        //    e.AreaCode = AreaCodeEnum.剑阁县;
                        //}
                        //else if (pidArr.WCXPIDArray.Contains(departId))
                        //{
                        //    e.Area = AreaCodeEnum.旺苍县.ToString();
                        //    e.AreaCode = AreaCodeEnum.旺苍县;
                        //}
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
        //private string GetAccessToken()
        //{
        //    DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
        //    OapiGettokenRequest request = new OapiGettokenRequest();
        //    request.Appkey = "ding7xespi5yumrzraaq";
        //    request.Appsecret = "idKPu4wVaZjBKo6oUvxcwSQB7tExjEbPaBpVpCEOGlcZPsH4BDx-sKilG726-nC3";
        //    request.SetHttpMethod("GET");
        //    OapiGettokenResponse response = client.Execute(request);
        //    return response.AccessToken;
        //}


        /// <summary>
        /// 按需获取子节点
        /// </summary>
        public async Task<List<OrganizationNzTreeNode>> GetTreesAsync()
        {
            int? count = 0;
            var organizationList = await (from o in _organizationRepository.GetAll()
                                          select new OrganizationListDto()
                                          {
                                              Id = o.Id,
                                              DepartmentName = o.DepartmentName,
                                              OrgDeptName = o.DepartmentName,
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
                deptName = t.OrgDeptName,
                children = GetTrees(t.Id, organizationList)
            }).ToList();
            return treeNodeList;
        }
    }

    public class AreaCodeArray
    {
        public long[] ZHQPIDArray { get; set; }

        public long[] JGXPIDArray { get; set; }


        public long[] WCXPIDArray { get; set; }
    }
}