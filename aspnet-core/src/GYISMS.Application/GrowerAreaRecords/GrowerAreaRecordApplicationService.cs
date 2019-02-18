
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
using GYISMS.Helpers;
using Microsoft.AspNetCore.Hosting;
using GYISMS.GYEnums;

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
        private readonly IHostingEnvironment _hostingEnvironment;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public GrowerAreaRecordAppService(
        IRepository<GrowerAreaRecord, Guid> entityRepository
        , IRepository<ScheduleDetail, Guid> scheduledetailRepository
        , IRepository<Schedule, Guid> scheduleRepository
        , IRepository<Grower, int> growerRepository
        , IHostingEnvironment env
        )
        {
            _entityRepository = entityRepository;
            _scheduledetailRepository = scheduledetailRepository;
            _scheduleRepository = scheduleRepository;
            _growerRepository = growerRepository;
            _hostingEnvironment = env;
        }


        /// <summary>
        /// 获取GrowerAreaRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<GrowerAreaRecordListDto>> GetPaged(GetGrowerAreaRecordsInput input)
        {

            var growerAreaRecord = _entityRepository.GetAll().Where(v => v.GrowerId == input.GrowerId);
            // TODO:根据传入的参数添加过滤条件

            var scheduleDetail = _scheduledetailRepository.GetAll().Select(v => new ScheduleDetailListDto
            {
                Id = v.Id,
                ScheduleId = v.ScheduleId
            });

            var schedule = _scheduleRepository.GetAll();
            var result = from g in growerAreaRecord
                         join sd in scheduleDetail on g.ScheduleDetailId equals sd.Id
                         join s in schedule on sd.ScheduleId equals s.Id
                         select new GrowerAreaRecordListDto()
                         {
                             Id = g.Id,
                             Area = g.Area,
                             ImgPath = g.ImgPath,
                             Location = g.Location,
                             EmployeeName = g.EmployeeName,
                             CollectionTime = g.CollectionTime,
                             Remark = g.Remark,
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
        public async Task<DistrictAreaChartDto> GetDistrictDDChartDataAsync()
        {
            //var actual = await (_growerRepository.GetAll().GroupBy(v => v.AreaCode)
            //    .Select(v => new AreaChartDto()
            //    {
            //        GroupName = "落实面积",
            //        Area = v.Sum(a => a.ActualArea ?? 0),
            //        AreaName = v.Key.ToString()
            //    })).AsNoTracking().ToListAsync();

            //var expected = await (_growerRepository.GetAll().GroupBy(v => v.AreaCode)
            //   .Select(v => new AreaChartDto()
            //   {
            //       GroupName = "约定面积",
            //       Area = v.Sum(a => a.PlantingArea??0),
            //       AreaName = v.Key.ToString()
            //   })).AsNoTracking().ToListAsync();

            //List<AreaChartDto> list = new List<AreaChartDto>();
            //list.AddRange(expected);
            //list.AddRange(actual);
            //return list;
            DistrictAreaChartDto result = new DistrictAreaChartDto();
            result.list = new List<AreaChartDto>();

            //昭化
            AreaChartDto zhExpected = new AreaChartDto();
            zhExpected.GroupName = "约定面积";
            zhExpected.AreaName = "昭化区";
            zhExpected.Area = await _growerRepository.GetAll().Where(v=>v.AreaCode == GYEnums.AreaCodeEnum.昭化区).SumAsync(v => v.PlantingArea ?? 0);
            AreaChartDto zhActual = new AreaChartDto();
            zhActual.GroupName = "落实面积";
            zhActual.AreaName = "昭化区";
            zhActual.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.昭化区).SumAsync(v => v.ActualArea ?? 0);
            //剑阁
            AreaChartDto jgExpected = new AreaChartDto();
            jgExpected.GroupName = "约定面积";
            jgExpected.AreaName = "剑阁县";
            jgExpected.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.剑阁县).SumAsync(v => v.PlantingArea ?? 0);
            AreaChartDto jgActual = new AreaChartDto();
            jgActual.GroupName = "落实面积";
            jgActual.AreaName = "剑阁县";
            jgActual.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.剑阁县).SumAsync(v => v.ActualArea ?? 0);
            //旺苍
            AreaChartDto wcExpected = new AreaChartDto();
            wcExpected.GroupName = "约定面积";
            wcExpected.AreaName = "旺苍县";
            wcExpected.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.旺苍县).SumAsync(v => v.PlantingArea ?? 0);
            AreaChartDto wcActual = new AreaChartDto();
            wcActual.GroupName = "落实面积";
            wcActual.AreaName = "旺苍县";
            wcActual.Area = await _growerRepository.GetAll().Where(v => v.AreaCode == GYEnums.AreaCodeEnum.旺苍县).SumAsync(v => v.ActualArea ?? 0);

            result.list.Add(zhExpected);
            result.list.Add(zhActual);
            result.list.Add(jgExpected);
            result.list.Add(jgActual);
            result.list.Add(wcExpected);
            result.list.Add(wcActual);

            result.ZhExpected = zhExpected.Area;
            result.ZhActual = zhActual.Area;
            result.JgExpected = jgExpected.Area;
            result.JgActual = jgActual.Area;
            result.WcExpected = wcExpected.Area;
            result.WcActual = wcActual.Area;
            return result;
        }

        [AbpAllowAnonymous]
        public async Task SaveGrowerAreaRecordAsync(DingDingAreaRecordInput input)
        {
            GrowerAreaRecord record = new GrowerAreaRecord();
            var scheduledetail = await _scheduledetailRepository.GetAsync(input.ScheduleDetailId);
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
        public async Task PostDeleteAsync(EntityDto<Guid> input)
        {
            await _entityRepository.DeleteAsync(input.Id);
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
    }
}


