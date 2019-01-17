
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

        /// <summary>
        /// 构造函数 
        ///</summary>
        public GrowerAppService(IRepository<Grower, int> growerRepository
            , IGrowerManager growerManager
            , IRepository<ScheduleDetail, Guid> scheduledetailRepository
            , IRepository<Employee, string> employeeRepository
            )
        {
            _growerRepository = growerRepository;
            _growerManager = growerManager;
            _scheduledetailRepository = scheduledetailRepository;
            _employeeRepository = employeeRepository;
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
                .WhereIf(input.AreaName.HasValue, u => u.AreaCode == input.AreaName)
                .WhereIf(areaCode.HasValue, u => u.AreaCode == areaCode)
                .WhereIf(input.IsEnable.HasValue,u=>u.IsEnable==input.IsEnable);
            // TODO:根据传入的参数添加过滤条件

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

        /// <summary>
        /// 获取烟农信息（不分页）
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork(isTransactional: false)]
        public async Task<List<GrowerListDto>> GetGrowersNoPageAsync(GetGrowersInput input)
        {
            int count = await _scheduledetailRepository.GetAll().Where(v => v.ScheduleTaskId == input.Id).CountAsync();
            if (count != 0)
            {
                if(input.EmployeeId == "1" || input.EmployeeId == "2"|| input.EmployeeId == "3")
                {
                    var areaCode = (AreaCodeEnum)int.Parse(input.EmployeeId);
                    var growerList = _growerRepository.GetAll().Where(v => v.IsDeleted == false);
                    var employeeIds = _employeeRepository.GetAll().Where(v=> v.AreaCode == areaCode).Select(v => v.Id);
                    var areaGrowerList = growerList.Where(v =>v.IsDeleted==false && employeeIds.Contains(v.EmployeeId));
                    var scheduleDetailList = _scheduledetailRepository.GetAll().Where(v => v.ScheduleId == input.ScheduleId && v.TaskId == input.TaskId);
                    var query = await (from g in areaGrowerList
                                       select new GrowerListDto()
                                       {
                                           Id = g.Id,
                                           Name = g.Name,
                                           EmployeeName = g.EmployeeName,
                                           EmployeeId = g.EmployeeId,
                                           UnitName = g.UnitName,
                                           Tel = g.Tel
                                       }).AsNoTracking().ToListAsync();
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
                    var growerList = _growerRepository.GetAll().Where(v => v.IsDeleted == false && v.EmployeeId == input.EmployeeId);
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
                                       }).AsNoTracking().ToListAsync();
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
                    var growerList = _growerRepository.GetAll().Where(v => v.IsDeleted == false);
                    var employeeIds = _employeeRepository.GetAll().Where(v => v.AreaCode == areaCode).Select(v => v.Id);
                    var areaGrowerList = growerList.Where(v => v.IsDeleted == false && employeeIds.Contains(v.EmployeeId));
                    var query = await (from g in areaGrowerList
                                       select new GrowerListDto()
                                       {
                                           Id = g.Id,
                                           Name = g.Name,
                                           EmployeeName = g.EmployeeName,
                                           EmployeeId = g.EmployeeId,
                                           UnitName = g.UnitName,
                                           Tel = g.Tel,
                                           //Checked = true
                                       }).AsNoTracking().ToListAsync();
                    return query;
                }
                else
                {
                    var growerList = _growerRepository.GetAll().Where(v => v.IsDeleted == false && v.EmployeeId == input.EmployeeId);
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
                                       }).AsNoTracking().ToListAsync();
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
        public async Task<APIResultDto> SavePositionAsync(int id, decimal longitude, decimal latitude)
        {
            var grower = await _growerRepository.GetAsync(id);
            if (grower == null)
            {
                return new APIResultDto() { Code = 901, Msg = "烟农不存在" };
            }

            grower.Longitude = longitude;
            grower.Latitude = latitude;
            await _growerRepository.UpdateAsync(grower);
            return new APIResultDto() { Code = 0, Msg = "采集位置成功", Data = new { lon = longitude , lat = latitude } };
        }
    }
}


