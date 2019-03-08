
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

using GYISMS.Growers.Authorization;
using GYISMS.Growers.Dtos;
using GYISMS.Growers;
using GYISMS.Authorization;
using GYISMS.ScheduleDetails;
using GYISMS.Employees;
using Abp.Auditing;
using GYISMS.Dtos;
using GYISMS.GYEnums;
using Abp.Domain.Uow;
using GYISMS.GrowerLocationLogs;
using GYISMS.SystemDatas;

namespace GYISMS.Growers
{
    /// <summary>
    /// Grower应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class GrowerAppService : GYISMSAppServiceBase, IGrowerAppService
    {
        private readonly IRepository<Grower, int> _growerRepository;
        private readonly IGrowerManager _growerManager;
        private readonly IRepository<ScheduleDetail, Guid> _scheduledetailRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IEmployeeManager _employeeManager;
        private readonly IRepository<GrowerLocationLog, Guid> _growerLocationLogRepository;
        private readonly IRepository<SystemData, int> _systemdataRepository;
        /// <summary>
        /// 构造函数 
        ///</summary>
        public GrowerAppService(IRepository<Grower, int> growerRepository
            , IGrowerManager growerManager
            , IRepository<ScheduleDetail, Guid> scheduledetailRepository
            , IRepository<Employee, string> employeeRepository
            , IEmployeeManager employeeManager
            , IRepository<GrowerLocationLog, Guid> growerLocationLogRepository
            , IRepository<SystemData, int> systemdataRepository
            )
        {
            _growerRepository = growerRepository;
            _growerManager = growerManager;
            _scheduledetailRepository = scheduledetailRepository;
            _employeeRepository = employeeRepository;
            _employeeManager = employeeManager;
            _growerLocationLogRepository = growerLocationLogRepository;
            _systemdataRepository = systemdataRepository;
        }


        /// <summary>
        /// 获取Grower的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GrowerListDto>> GetPagedGrowersAsync(GetGrowersInput input)
        {
            var areaCode = await GetCurrentUserAreaCodeAsync();
            var query = _growerRepository.GetAll().WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrEmpty(input.Employee), u => u.EmployeeName.Contains(input.Employee))
                //.WhereIf(input.AreaName.HasValue, u => u.AreaCode == input.AreaName)
                .WhereIf(areaCode.HasValue, u => u.AreaCode == areaCode)
                .WhereIf(input.IsEnable.HasValue, u => u.IsEnable == input.IsEnable);
            //区县层级查询
            if (!string.IsNullOrEmpty(input.AreaName))
            {
                //区县
                if (input.AreaName == "1" || input.AreaName == "2" || input.AreaName == "3")
                {
                    var selectedAreaCode = (AreaCodeEnum)int.Parse(input.AreaName);
                    query = query.Where(u => u.AreaCode == selectedAreaCode);
                }
                else//员工部门
                {
                    var employeeIds = await GetEmployeeIdsByTreeKey(input.AreaName);
                    query = query.Where(u => employeeIds.Contains(u.EmployeeId));
                }
            }

            var growerCount = await query.CountAsync();

            var growers = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var growerListDtos = ObjectMapper.Map<List <GrowerListDto>>(growers);
            var growerListDtos = growers.MapTo<List<GrowerListDto>>();

            return new PagedResultDto<GrowerListDto>(
                        growerCount,
                        growerListDtos
                );
        }

        private async Task<string[]> GetEmployeeIdsByTreeKey(string treeKey)
        {
            //员工 或 部门及子部门下面的员工 add by donald 2019-2-12
            var employeeIds = _employeeRepository.GetAll().Where(v => v.Id == treeKey).Select(v => v.Id).ToArray();
            if (employeeIds.Length == 0)//如果没有员工 找部门
            {
                long deptid = 0;
                if (long.TryParse(treeKey, out deptid))
                {
                    var depts = await _employeeManager.GetDeptIdArrayAsync(deptid);
                    employeeIds = _employeeRepository.GetAll().Where(v => depts.Any(d => v.Department.Contains(d))).Select(v => v.Id).ToArray();
                }
            }
            return employeeIds;
        }

        /// <summary>
        /// 获取烟农信息（不分页）
        /// </summary>
        [UnitOfWork(isTransactional: false)]
        public async Task<List<GrowerListDto>> GetGrowersNoPageAsync(GetGrowersInput input)
        {
            int count = await _scheduledetailRepository.GetAll().Where(v => v.ScheduleTaskId == input.Id).CountAsync();
            if (count != 0)
            {
                if (input.EmployeeId == "1" || input.EmployeeId == "2" || input.EmployeeId == "3")
                {
                    var areaCode = (AreaCodeEnum)int.Parse(input.EmployeeId);
                    //var deptArr = await _employeeManager.GetAreaDeptIdArrayAsync(areaCode);//获取该区县下配置的部门和子部门列表

                    var growerList = _growerRepository.GetAll().Where(v => v.IsEnable == true && v.AreaCode == areaCode);
                    //var employeeIds = _employeeRepository.GetAll().Where(v => v.AreaCode == areaCode || deptArr.Contains(v.Department)).Select(v => v.Id);
                    //var areaGrowerList = growerList.Where(v => employeeIds.Contains(v.EmployeeId));
                    var scheduleDetailList = _scheduledetailRepository.GetAll().Where(v => v.ScheduleId == input.ScheduleId && v.TaskId == input.TaskId);
                    var query = await (from g in growerList //areaGrowerList
                                       select new GrowerListDto()
                                       {
                                           Id = g.Id,
                                           Name = g.Name,
                                           EmployeeName = g.EmployeeName,
                                           EmployeeId = g.EmployeeId,
                                           UnitName = g.UnitName,
                                           Tel = g.Tel
                                       }).ToListAsync();
                    var scheduleDto = from s in scheduleDetailList
                                      select new
                                      {
                                          s.Id,
                                          s.VisitNum,
                                          s.GrowerId
                                      };
                    foreach (var scheduleItem in scheduleDto)
                    {
                        foreach (var item in query)
                        {
                            if (item.Id == scheduleItem.GrowerId)
                            {
                                item.Checked = true;
                                item.VisitNum = scheduleItem.VisitNum;
                                item.ScheduleDetailId = scheduleItem.Id;
                                break;
                            }
                        }
                    }
                    return query;
                }
                else
                {
                    //var growerList = _growerRepository.GetAll().Where(v => v.IsEnable == true && v.EmployeeId == input.EmployeeId);
                    //员工 或 部门及子部门下面的员工 add by donald 2019-2-12
                    var employeeIds = await GetEmployeeIdsByTreeKey(input.EmployeeId);
                    var growerList = _growerRepository.GetAll().Where(v => v.IsEnable == true && employeeIds.Contains(v.EmployeeId));

                    var scheduleDetailList = _scheduledetailRepository.GetAll().Where(v => v.ScheduleId == input.ScheduleId && v.TaskId == input.TaskId);
                    var query = await (from g in growerList
                                       select new GrowerListDto()
                                       {
                                           Id = g.Id,
                                           Name = g.Name,
                                           EmployeeName = g.EmployeeName,
                                           EmployeeId = g.EmployeeId,
                                           UnitName = g.UnitName,
                                           Tel = g.Tel
                                       }).ToListAsync();
                    var scheduleDto = from s in scheduleDetailList
                                      select new
                                      {
                                          s.Id,
                                          s.VisitNum,
                                          s.GrowerId
                                      };
                    foreach (var scheduleItem in scheduleDto)
                    {
                        foreach (var item in query)
                        {
                            if (item.Id == scheduleItem.GrowerId)
                            {
                                item.Checked = true;
                                item.VisitNum = scheduleItem.VisitNum;
                                item.ScheduleDetailId = scheduleItem.Id;
                                break;
                            }
                        }
                    }
                    return query;
                }
            }
            else
            {
                if (input.EmployeeId == "1" || input.EmployeeId == "2" || input.EmployeeId == "3")
                {
                    var areaCode = (AreaCodeEnum)int.Parse(input.EmployeeId);
                    //var deptArr = await _employeeManager.GetAreaDeptIdArrayAsync(areaCode);//获取该区县下配置的部门和子部门列表

                    var growerList = _growerRepository.GetAll().Where(v => v.IsEnable == true && v.AreaCode == areaCode);
                    //var employeeIds = _employeeRepository.GetAll().Where(v => v.AreaCode == areaCode || deptArr.Contains(v.Department)).Select(v => v.Id);
                    //var areaGrowerList = growerList.Where(v => employeeIds.Contains(v.EmployeeId));
                    var query = await (from g in growerList //areaGrowerList
                                       select new GrowerListDto()
                                       {
                                           Id = g.Id,
                                           Name = g.Name,
                                           EmployeeName = g.EmployeeName,
                                           EmployeeId = g.EmployeeId,
                                           UnitName = g.UnitName,
                                           Tel = g.Tel,
                                           //Checked = true
                                       }).ToListAsync();
                    return query;
                }
                else
                {
                    //员工 或 部门及子部门下面的员工 add by donald 2019-2-12
                    var employeeIds = await GetEmployeeIdsByTreeKey(input.EmployeeId);

                    var growerList = _growerRepository.GetAll().Where(v => v.IsEnable == true && employeeIds.Contains(v.EmployeeId));

                    var query = await (from g in growerList
                                       select new GrowerListDto()
                                       {
                                           Id = g.Id,
                                           Name = g.Name,
                                           EmployeeName = g.EmployeeName,
                                           EmployeeId = g.EmployeeId,
                                           UnitName = g.UnitName,
                                           Tel = g.Tel,
                                           //Checked = true
                                       }).ToListAsync();
                    return query;
                }

            }
        }

        /// <summary>
        /// 添加或者修改Grower的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateGrower(CreateOrUpdateGrowerInput input)
        {

            if (input.Grower.Id != null)
            {
                await UpdateGrowerAsync(input.Grower);
            }
            else
            {
                await CreateGrowerAsync(input.Grower);
            }
        }


        /// <summary>
        /// 新增Grower
        /// </summary>
        protected virtual async Task<GrowerEditDto> CreateGrowerAsync(GrowerEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Grower>(input);
            entity.IsDeleted = false;

            var id = await _growerRepository.InsertAndGetIdAsync(entity);
            return entity.MapTo<GrowerEditDto>();
        }

        /// <summary>
        /// 编辑Grower
        /// </summary>
        protected virtual async Task<GrowerEditDto> UpdateGrowerAsync(GrowerEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新      
            var entity = await _growerRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            // ObjectMapper.Map(input, entity);
            var result = await _growerRepository.UpdateAsync(entity);
            return result.MapTo<GrowerEditDto>();
        }



        /// <summary>
        /// 删除Grower信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(GrowerAppPermissions.Grower_Delete)]
        public async Task DeleteGrower(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _growerRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Grower的方法
        /// </summary>
        [AbpAuthorize(GrowerAppPermissions.Grower_BatchDelete)]
        public async Task BatchDeleteGrowersAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _growerRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 新增或修改烟农信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GrowerEditDto> CreateOrUpdateGrowerAsycn(GrowerEditDto input)
        {
            if (input.Id != null)
            {
                return await UpdateGrowerAsync(input);
            }
            else
            {
                return await CreateGrowerAsync(input);
            }
        }

        /// <summary>
        /// 根据id获取烟农信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GrowerListDto> GetGrowerByIdAsync(int id)
        {
            var entity = await _growerRepository.GetAsync(id);
            return entity.MapTo<GrowerListDto>();
        }

        /// <summary>
        /// 删除烟农信息
        /// </summary>
        public async Task GrowerDeleteByIdAsync(GrowerEditDto input)
        {
            var entity = await _growerRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = AbpSession.UserId;
            await _growerRepository.UpdateAsync(entity);
        }

        /// <summary>
        /// 更新烟农位置
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> SavePositionAsync(int id, decimal longitude, decimal latitude, string userId)
        {
            var grower = await _growerRepository.GetAsync(id);
            if (grower == null)
            {
                return new APIResultDto() { Code = 901, Msg = "烟农不存在" };
            }
            var date = DateTime.Now;
            var startTime = DateTime.Parse(date.Year + "-1-1");
            var endTime = DateTime.Parse((date.Year + 1) + "-1-1");
            var num = await _growerLocationLogRepository.GetAll().Where(g => g.CreationTime >= startTime && g.CreationTime < endTime && g.GrowerId == grower.Id).CountAsync();
            var systemData = await _systemdataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == GYCode.LocationLimitCode).FirstOrDefaultAsync();
            var limitNum = 3;
            if (systemData != null && !string.IsNullOrEmpty(systemData.Desc))
            {
                limitNum = int.Parse(systemData.Desc);
            }
            //if (num >= limitNum)
            //{
            //    return new APIResultDto() { Code = 901, Msg = "地理位置只允许修改"+ limitNum + "次,请尝试申请" };
            //}
            var log = new GrowerLocationLog()
            {
                EmployeeId = userId,
                GrowerId = id,
                Latitude = latitude,
                Longitude = longitude,
                CreationTime = DateTime.Now
            };
            await _growerLocationLogRepository.InsertAsync(log);
            grower.CollectNum = ++num;//采集次数加一
            grower.Longitude = longitude;
            grower.Latitude = latitude;
            await _growerRepository.UpdateAsync(grower);
            return new APIResultDto() { Code = 0, Msg = "采集位置成功", Data = new { lon = longitude, lat = latitude, colNum = grower.CollectNum } };
        }
    }
}


