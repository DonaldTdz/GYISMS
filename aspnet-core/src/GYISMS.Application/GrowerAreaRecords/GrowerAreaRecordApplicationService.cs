
using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using Abp.UI;
using Abp.AutoMapper;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;


using GYISMS.GrowerAreaRecords;
using GYISMS.GrowerAreaRecords.Dtos;
using GYISMS.Authorization;
using GYISMS.ScheduleDetails;
using GYISMS.ScheduleDetails.Dtos;
using GYISMS.Schedules;
using GYISMS.Schedules.Dtos;
using GYISMS.Growers;
using GYISMS.SystemDatas;
using GYISMS.GYEnums;
using GYISMS.Organizations;
using GYISMS.Employees;
using GYISMS.Employees.Dtos;
using GYISMS.Helpers;
using Microsoft.AspNetCore.Hosting;
using GYISMS.GYEnums;
using System.Collections;

namespace GYISMS.GrowerAreaRecords
{
    /// <summary>
    /// GrowerAreaRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class GrowerAreaRecordAppService : GYISMSAppServiceBase, IGrowerAreaRecordAppService
    {
        private readonly IRepository<GrowerAreaRecord, Guid> _entityRepository;
        private readonly IRepository<ScheduleDetail, Guid> _scheduledetailRepository;
        private readonly IRepository<Schedule, Guid> _scheduleRepository;
        private readonly IRepository<Grower, int> _growerRepository;
        private readonly IRepository<SystemData, int> _systemdataRepository;
        private readonly IRepository<Organization, long> _organizationRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IEmployeeManager _employeeManager;
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public GrowerAreaRecordAppService(
        IRepository<GrowerAreaRecord, Guid> entityRepository
        , IRepository<ScheduleDetail, Guid> scheduledetailRepository
        , IRepository<Schedule, Guid> scheduleRepository
        , IRepository<Grower, int> growerRepository
        , IRepository<SystemData, int> systemdataRepository
        , IRepository<Organization, long> organizationRepository
        , IRepository<Employee, string> employeeRepository
        , IEmployeeManager employeeManager
        , IHostingEnvironment env
        )
        {
            _entityRepository = entityRepository;
            _scheduledetailRepository = scheduledetailRepository;
            _scheduleRepository = scheduleRepository;
            _growerRepository = growerRepository;
            _systemdataRepository = systemdataRepository;
            _organizationRepository = organizationRepository;
            _employeeRepository = employeeRepository;
            _hostingEnvironment = env;
            _employeeManager = employeeManager;
        }


        /// <summary>
        /// 获取GrowerAreaRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<GrowerAreaRecordListDto>> GetPaged(GetGrowerAreaRecordsInput input)
        {
            var result = from g in _entityRepository.GetAll()
                         join sd in _scheduledetailRepository.GetAll() on g.ScheduleDetailId equals sd.Id
                         join s in _scheduleRepository.GetAll() on sd.ScheduleId equals s.Id
                         where g.GrowerId == input.GrowerId
                         select new GrowerAreaRecordListDto()
                         {
                             Id = g.Id,
                             Area = g.Area,
                             ImgPath = g.ImgPath,
                             Location = g.Location,
                             EmployeeName = g.EmployeeName,
                             CollectionTime = g.CollectionTime,
                             Remark = g.Remark,
                             ScheduleDetailId = g.ScheduleDetailId,
                             ScheduleName = s.Name
                         };
            var count = await result.CountAsync();

            var entityList = await result
                    .OrderByDescending(v => v.CollectionTime).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<GrowerAreaRecordListDto>>(entityList);
            //var entityListDtos = entityList.MapTo<List<GrowerAreaRecordListDto>>();

            return new PagedResultDto<GrowerAreaRecordListDto>(count, entityList);
        }


        /// <summary>
        /// 通过指定id获取GrowerAreaRecordListDto信息
        /// </summary>

        public async Task<GrowerAreaRecordListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<GrowerAreaRecordListDto>();
        }

        /// <summary>
        /// 获取编辑 GrowerAreaRecord
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetGrowerAreaRecordForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetGrowerAreaRecordForEditOutput();
            GrowerAreaRecordEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<GrowerAreaRecordEditDto>();

                //growerAreaRecordEditDto = ObjectMapper.Map<List<growerAreaRecordEditDto>>(entity);
            }
            else
            {
                editDto = new GrowerAreaRecordEditDto();
            }

            output.GrowerAreaRecord = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改GrowerAreaRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateGrowerAreaRecordInput input)
        {

            if (input.GrowerAreaRecord.Id.HasValue)
            {
                await Update(input.GrowerAreaRecord);
            }
            else
            {
                await Create(input.GrowerAreaRecord);
            }
        }


        /// <summary>
        /// 新增GrowerAreaRecord
        /// </summary>

        protected virtual async Task<GrowerAreaRecordEditDto> Create(GrowerAreaRecordEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <GrowerAreaRecord>(input);
            var entity = input.MapTo<GrowerAreaRecord>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<GrowerAreaRecordEditDto>();
        }

        /// <summary>
        /// 编辑GrowerAreaRecord
        /// </summary>

        protected virtual async Task Update(GrowerAreaRecordEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除GrowerAreaRecord信息的方法
        /// </summary>
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除GrowerAreaRecord的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 广元市落实面积图表
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<CityAreaChartDto> GetCityDDChartDataAsync()
        {
            CityAreaChartDto result = new CityAreaChartDto();

            result.list = new List<AreaChartDto>();

            AreaChartDto actual = new AreaChartDto();
            actual.GroupName = "落实面积";
            actual.AreaName = "广元市";
            actual.Area = await _growerRepository.GetAll().SumAsync(v => v.ActualArea ?? 0);
            AreaChartDto expected = new AreaChartDto();
            expected.GroupName = "约定面积";
            expected.AreaName = "广元市";
            expected.Area = await _growerRepository.GetAll().SumAsync(v => v.PlantingArea ?? 0);

            result.list.Add(expected);
            result.list.Add(actual);
            result.Expected = expected.Area;
            result.Actual = actual.Area;
            return result;
        }

        /// <summary>
        /// 区县落实面积图表
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<DistrictAreaChartDto> GetDistrictDDChartDataAsync(string id)
        {
            var areacode = await _employeeManager.GetAreaCodeByUserIdAsync(id);

            DistrictAreaChartDto result = new DistrictAreaChartDto();
            result.list = new List<AreaChartDto>();
            if (areacode == AreaCodeEnum.广元市 || areacode == AreaCodeEnum.昭化区)
            {
                //昭化
                AreaChartDto zhExpected = new AreaChartDto();
                zhExpected.GroupName = "约定面积";
                zhExpected.AreaName = "昭化区";
                var zhExpectedArea =  _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.昭化区).Select(g => g.PlantingArea).Sum();
                zhExpected.Area = zhExpectedArea ?? 0;
                AreaChartDto zhActual = new AreaChartDto();
                zhActual.GroupName = "落实面积";
                zhActual.AreaName = "昭化区";
                //zhActual.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.昭化区).SumAsync(v => v.ActualArea ?? 0);
                var zhActualArea =  _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.昭化区).Select(g => g.ActualArea).Sum();
                zhActual.Area = zhActualArea ?? 0;
                result.list.Add(zhExpected);
                result.list.Add(zhActual);
                result.ZhExpected = zhExpected.Area;
                result.ZhActual = zhActual.Area;
            }
            if (areacode == AreaCodeEnum.广元市 || areacode == AreaCodeEnum.剑阁县)
            {
                //剑阁
                AreaChartDto jgExpected = new AreaChartDto();
                jgExpected.GroupName = "约定面积";
                jgExpected.AreaName = "剑阁县";
                //jgExpected.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.剑阁县).SumAsync(v => v.PlantingArea ?? 0);
                var jgExpectedArea =  _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.剑阁县).Select(g => g.PlantingArea).Sum();
                jgExpected.Area = jgExpectedArea ?? 0;
                AreaChartDto jgActual = new AreaChartDto();
                jgActual.GroupName = "落实面积";
                jgActual.AreaName = "剑阁县";
                //jgActual.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.剑阁县).SumAsync(v => v.ActualArea ?? 0);
                var jgActualArea =  _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.剑阁县).Select(g => g.ActualArea).Sum();
                jgActual.Area = jgActualArea ?? 0;
                result.list.Add(jgExpected);
                result.list.Add(jgActual);
                result.JgExpected = jgExpected.Area;
                result.JgActual = jgActual.Area;
            }
            if (areacode == AreaCodeEnum.广元市 || areacode == AreaCodeEnum.旺苍县)
            {
                //旺苍
                AreaChartDto wcExpected = new AreaChartDto();
                wcExpected.GroupName = "约定面积";
                wcExpected.AreaName = "旺苍县";
                //wcExpected.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.旺苍县).SumAsync(v => v.PlantingArea ?? 0);
                var wcExpectedArea =  _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.旺苍县).Select(g => g.PlantingArea).Sum();
                wcExpected.Area = wcExpectedArea ?? 0;
                AreaChartDto wcActual = new AreaChartDto();
                wcActual.GroupName = "落实面积";
                wcActual.AreaName = "旺苍县";
                //wcActual.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.旺苍县).SumAsync(v => v.ActualArea ?? 0);
                var wcActualArea =  _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.旺苍县).Select(g => g.ActualArea).Sum();
                wcActual.Area = wcActualArea ?? 0;
                result.list.Add(wcExpected);
                result.list.Add(wcActual);
                result.WcExpected = wcExpected.Area;
                result.WcActual = wcActual.Area;
            }
            return result;
        }

        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="childrenList"></param>
        /// <returns></returns>
        private List<EmployeeNzTreeNode> GetDeptChildren(long orgId, List<EmployeeNzTreeNode> childrenList)
        {
            //string otherIds ="";
            var orgList = _organizationRepository.GetAll().Where(o => o.ParentId == orgId).ToList();
            //var childrenList = new List<EmployeeNzTreeNode>();
            List<EmployeeNzTreeNode> treeNodeList = orgList.Select(t => new EmployeeNzTreeNode()
            {
                key = t.Id.ToString(),
                title = t.DepartmentName,
                children = GetDeptChildren(t.Id, childrenList)
            }).ToList();
            //childrenList.AddRange(treeNodeList);
            var employeeList = (from o in _employeeRepository.GetAll()
                                    .Where(v => v.Department.Contains("[" + orgId + "]"))
                                select new EmployeeNzTreeNode()
                                {
                                    key = o.Id,
                                    title = o.Name,
                                    children = null,
                                }).ToList();
            //if (treeNodeList.Count == 0 && employeeList.Count > 0)
            //{
            //    //childrenList.Add(new EmployeeNzTreeNode() { key ="999",title="其他",children=null});
            //    otherIds = string.Join(',', employeeList.Select(v => v.key));
            //}
            //else
            //{
            childrenList.AddRange(employeeList);
            //}
            return childrenList;
        }

        private async Task<string[]> GetEmployeeIdsByDeptId(long deptId)
        {
            var childrenDeptIds = await _employeeManager.GetDeptIdArrayAsync(deptId);
            var query = _employeeRepository.GetAll().Where(e => childrenDeptIds.Any(c => e.Department.Contains(c))).Select(e => e.Id);
            return await query.ToArrayAsync();
        }

        private async Task<string[]> GetOtherEmployeeIdsByDeptId(long deptId)
        {
            var strDept = "[" + deptId + "]";
            var query = _employeeRepository.GetAll().Where(e => e.Department.Contains(strDept)).Select(e => e.Id);
            return await query.ToArrayAsync();
        }

        /// <summary>
        /// 获取区县下面的钉钉组织架构
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<CommDetail> GetAreaOrganization(GetDingDingAreaRecordsInput input)
        {
            var orgCode = "";
            var orgIds = "";
            var isArea = false;
            //当type为空 表示为区县级
            if (string.IsNullOrEmpty(input.Type))
            {
                switch (input.Id)
                {
                    case "1":
                        {
                            //区县枚举值
                            //input.Id = "1";
                            orgCode = GYCode.昭化区;
                        }
                        break;
                    case "2":
                        {
                            //input.Id = "2";
                            orgCode = GYCode.剑阁县;
                        }
                        break;
                    case "3":
                        {
                            //input.Id = "3";
                            orgCode = GYCode.旺苍县;
                        }
                        break;
                }
                isArea = true;
                orgIds = _systemdataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == orgCode).First().Desc;
            }
            else if (input.Type == "otherArea")
            {
                //其他类型 id为上级部门
                orgIds = input.Id;
            }
            else
            {
                //查子部门
                orgIds = string.Join(',', await _organizationRepository.GetAll().Where(v => v.ParentId == long.Parse(input.Id)).Select(v => v.Id).ToListAsync());
            }
            CommDetail commDetail = new CommDetail();
            if (!string.IsNullOrEmpty(orgIds) && input.Type != "otherArea") //部门统计
            {
                commDetail.Type = 0;
                var orgIdArr = orgIds.Split(',');
                foreach (var orgid in orgIdArr)
                {
                    var longOrgId = long.Parse(orgid);
                    var org = _organizationRepository.Get(longOrgId);
                    //var childrenList = new List<EmployeeNzTreeNode>();
                    //var employeeIds = GetDeptChildren(longOrgId, childrenList);
                    //decimal planArea = 0;
                    //decimal actualArea = 0;
                    //foreach (var item in employeeIds)
                    //{
                    //    planArea += await _growerRepository.GetAll().Where(v => v.EmployeeId == item.key).Select(v => v.PlantingArea ?? 0).SumAsync();
                    //    actualArea += await _growerRepository.GetAll().Where(v => v.EmployeeId == item.key).Select(v => v.ActualArea ?? 0).SumAsync();
                    //}
                    var employeeIds = await GetEmployeeIdsByDeptId(longOrgId);
                    decimal planArea = 0;
                    decimal actualArea = 0;
                    planArea = await _growerRepository.GetAll().Where(v => employeeIds.Contains(v.EmployeeId)).Select(v => v.PlantingArea ?? 0).SumAsync();
                    actualArea = await _growerRepository.GetAll().Where(v => employeeIds.Contains(v.EmployeeId)).Select(v => v.ActualArea ?? 0).SumAsync();

                    commDetail.List.Add(new CommChartDto()
                    {
                        GroupName = "约定面积",
                        AreaName = org.DepartmentName,
                        Area = planArea,
                    });
                    commDetail.List.Add(new CommChartDto()
                    {
                        GroupName = "落实面积",
                        AreaName = org.DepartmentName,
                        Area = actualArea
                    });
                    commDetail.Detail.Add(new AreaDetailDto()
                    {
                        DepartmentId = org.Id.ToString(),
                        AreaName = org.DepartmentName,
                        Expected = planArea,
                        Actual = actualArea
                    });
                }

                //other
                var pDeptId = long.Parse(input.Id);
                var employeeOtherIds = await GetOtherEmployeeIdsByDeptId(pDeptId);
                if (string.IsNullOrEmpty(input.Type))//区县 其他判断
                {
                    var tempCode = (AreaCodeEnum)int.Parse(input.Id);
                    var areaOtherIds = await _employeeRepository.GetAll().Where(v => v.AreaCode == tempCode).Select(v => v.Id).ToArrayAsync();
                    if (areaOtherIds.Count() > 0)
                    {
                        decimal planOtherArea = 0;
                        decimal actualOtherArea = 0;
                        planOtherArea = await _growerRepository.GetAll().Where(v => areaOtherIds.Contains(v.EmployeeId)).Select(v => v.PlantingArea ?? 0).SumAsync();
                        actualOtherArea = await _growerRepository.GetAll().Where(v => areaOtherIds.Contains(v.EmployeeId)).Select(v => v.ActualArea ?? 0).SumAsync();
                        commDetail.List.Add(new CommChartDto()
                        {
                            GroupName = "约定面积",
                            AreaName = "其他",
                            Area = planOtherArea,
                        });
                        commDetail.List.Add(new CommChartDto()
                        {
                            GroupName = "落实面积",
                            AreaName = "其他",
                            Area = actualOtherArea
                        });
                        commDetail.Detail.Add(new AreaDetailDto()
                        {
                            DepartmentId = input.Id,
                            AreaName = "其他",
                            Expected = planOtherArea,
                            Actual = actualOtherArea
                        });
                    }
                }
                if (employeeOtherIds.Count() > 0)
                {
                    decimal planOtherArea = 0;
                    decimal actualOtherArea = 0;
                    planOtherArea = await _growerRepository.GetAll().Where(v => employeeOtherIds.Contains(v.EmployeeId)).Select(v => v.PlantingArea ?? 0).SumAsync();
                    actualOtherArea = await _growerRepository.GetAll().Where(v => employeeOtherIds.Contains(v.EmployeeId)).Select(v => v.ActualArea ?? 0).SumAsync();
                    commDetail.List.Add(new CommChartDto()
                    {
                        GroupName = "约定面积",
                        AreaName = "其他",
                        Area = planOtherArea,
                    });
                    commDetail.List.Add(new CommChartDto()
                    {
                        GroupName = "落实面积",
                        AreaName = "其他",
                        Area = actualOtherArea
                    });
                    commDetail.Detail.Add(new AreaDetailDto()
                    {
                        DepartmentId = input.Id,
                        AreaName = "其他",
                        Expected = planOtherArea,
                        Actual = actualOtherArea
                    });
                }
            }
            else //烟技员统计
            {
                var employee = new List<Employee>();
                if ((input.Type == "otherArea" || isArea) && (input.Id == "1" || input.Id == "2" || input.Id == "3"))
                {
                    //区县-其他
                    var tempCode = (AreaCodeEnum)int.Parse(input.Id);
                    employee = await _employeeRepository.GetAll().Where(v => v.AreaCode == tempCode).ToListAsync();
                }
                else
                {
                    //其他
                    employee = await _employeeRepository.GetAll().Where(v => v.Department.Contains(input.Id.ToString())).ToListAsync();
                }
                commDetail.Type = 1;
                foreach (var item in employee)
                {
                    decimal planArea = 0;
                    decimal actualArea = 0;
                    planArea += await _growerRepository.GetAll().Where(v => v.EmployeeId == item.Id).Select(v => v.PlantingArea ?? 0).SumAsync();
                    actualArea += await _growerRepository.GetAll().Where(v => v.EmployeeId == item.Id).Select(v => v.ActualArea ?? 0).SumAsync();
                    commDetail.List.Add(new CommChartDto()
                    {
                        GroupName = "约定面积",
                        AreaName = item.Name,
                        Area = planArea,
                    });
                    commDetail.List.Add(new CommChartDto()
                    {
                        GroupName = "落实面积",
                        AreaName = item.Name,
                        Area = actualArea
                    });
                    commDetail.Detail.Add(new AreaDetailDto()
                    {
                        DepartmentId = item.Id,
                        AreaName = item.Name,
                        Expected = planArea,
                        Actual = actualArea
                    });
                }
            }
            return commDetail;
        }

        [AbpAllowAnonymous]
        public async Task SaveGrowerAreaRecordAsync(DingDingAreaRecordInput input)
        {
            GrowerAreaRecord record = new GrowerAreaRecord();
            var scheduledetail = await _scheduledetailRepository.GetAsync(input.ScheduleDetailId);
            scheduledetail.Status = ScheduleStatusEnum.进行中;

            record.EmployeeId = scheduledetail.EmployeeId;
            record.EmployeeName = scheduledetail.EmployeeName;
            record.CollectionTime = DateTime.Now;
            record.GrowerId = scheduledetail.GrowerId;
            record.ImgPath = ImageHelper.GenerateWatermarkImg(input.ImgPaths, input.Location, record.EmployeeName, scheduledetail.GrowerName, _hostingEnvironment.WebRootPath); //string.Join(',', input.ImgPaths);
            record.Latitude = input.Latitude;
            record.Longitude = input.Longitude;
            record.Location = input.Location;
            record.Remark = input.Remark;
            record.Area = input.Area;
            record.ScheduleDetailId = input.ScheduleDetailId;
            await _entityRepository.InsertAsync(record);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        [AbpAllowAnonymous]
        public async Task PostDeleteAsync(Guid id)
        {
            await _entityRepository.DeleteAsync(id);
        }

        [AbpAllowAnonymous]
        public async Task SubmitGrowerAreaAsync(EntityDto<Guid> input)
        {
            //更新计划状态
            var scheduleDetail = await _scheduledetailRepository.GetAsync(input.Id);
            scheduleDetail.Status = ScheduleStatusEnum.已完成;
            scheduleDetail.CompleteNum = scheduleDetail.VisitNum;
            //更新烟农落实面积
            var grower = await _growerRepository.GetAsync(scheduleDetail.GrowerId);
            var sumArea = _entityRepository.GetAll().Where(e => e.GrowerId == grower.Id && e.ScheduleDetailId == input.Id).Sum(e => e.Area);
            grower.AreaStatus = AreaStatusEnum.已落实;
            grower.AreaTime = DateTime.Now;
            grower.ActualArea = sumArea;
            grower.AreaScheduleDetailId = input.Id;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 根据烟技员获取烟农列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<AreaDetailDto>> GetGrowListByIdAsync(GetDingDingAreaRecordsInput input)
        {
            var result = await (_growerRepository.GetAll().Where(v => v.EmployeeId == input.Id.ToString())
                .Select(v => new AreaDetailDto()
                {
                    DepartmentId = v.Id.ToString(),
                    AreaName = v.Name,
                    Actual = v.ActualArea ?? 0,
                    Expected = v.PlantingArea ?? 0
                })).ToListAsync();
            return result;
        }

        /// <summary>
        /// 获取烟技员信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<EmployeeListDto> GetEmployessByIdAsync(GetDingDingAreaRecordsInput input)
        {
            var entity = await _employeeRepository.GetAsync(input.Id);
            //var result = await _employeeRepository.GetAll().Where(v=>v.Id == input.Id).FirstOrDefaultAsync();
            var result = entity.MapTo<EmployeeListDto>();
            var area = await _employeeManager.GetAreaCodeByUserIdAsync(input.Id);//钉钉用户区县权限
            result.Area = area.ToString();
            return result;
        }
    }
}