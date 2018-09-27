
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

using GYISMS.ScheduleDetails.Authorization;
using GYISMS.ScheduleDetails.Dtos;
using GYISMS.ScheduleDetails;
using GYISMS.Authorization;
using GYISMS.Growers.Dtos;
using GYISMS.GYEnums;
using GYISMS.Growers;
using GYISMS.Schedules;

namespace GYISMS.ScheduleDetails
{
    /// <summary>
    /// ScheduleDetail应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class ScheduleDetailAppService : GYISMSAppServiceBase, IScheduleDetailAppService
    {
        //private readonly IRepository<ScheduleDetail, Guid> _scheduledetailRepository;
        private readonly IScheduleDetailManager _scheduledetailManager;
        private readonly IRepository<Grower, int> _growerRepository;
        private readonly IRepository<Schedule, Guid> _scheduleRepository;
        private readonly ISheduleDetailRepository _scheduledetailRepository;


        /// <summary>
        /// 构造函数 
        ///</summary>
        public ScheduleDetailAppService(//IRepository<ScheduleDetail, Guid> scheduledetailRepository
           ISheduleDetailRepository scheduledetailRepository, IScheduleDetailManager scheduledetailManager, IRepository<Grower, int> growerRepository,
            IRepository<Schedule, Guid> scheduleRepository)
        {
            _scheduledetailRepository = scheduledetailRepository;
            _scheduledetailManager = scheduledetailManager;
            _growerRepository = growerRepository;
            _scheduleRepository = scheduleRepository;
        }


        /// <summary>
        /// 获取ScheduleDetail的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<ScheduleDetailListDto>> GetPagedScheduleDetails(GetScheduleDetailsInput input)
        {

            var query = _scheduledetailRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var scheduledetailCount = await query.CountAsync();

            var scheduledetails = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var scheduledetailListDtos = ObjectMapper.Map<List <ScheduleDetailListDto>>(scheduledetails);
            var scheduledetailListDtos = scheduledetails.MapTo<List<ScheduleDetailListDto>>();

            return new PagedResultDto<ScheduleDetailListDto>(
                    scheduledetailCount,
                    scheduledetailListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取ScheduleDetailListDto信息
        /// </summary>
        public async Task<ScheduleDetailListDto> GetScheduleDetailByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _scheduledetailRepository.GetAsync(input.Id);

            return entity.MapTo<ScheduleDetailListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetScheduleDetailForEditOutput> GetScheduleDetailForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetScheduleDetailForEditOutput();
            ScheduleDetailEditDto scheduledetailEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);

                scheduledetailEditDto = entity.MapTo<ScheduleDetailEditDto>();

                //scheduledetailEditDto = ObjectMapper.Map<List <scheduledetailEditDto>>(entity);
            }
            else
            {
                scheduledetailEditDto = new ScheduleDetailEditDto();
            }

            output.ScheduleDetail = scheduledetailEditDto;
            return output;
        }

        /// <summary>
        /// 新增ScheduleDetail
        /// </summary>
        protected virtual async Task<ScheduleDetailEditDto> CreateScheduleDetailAsync(ScheduleDetailEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<ScheduleDetail>(input);

            entity = await _scheduledetailRepository.InsertAsync(entity);
            return entity.MapTo<ScheduleDetailEditDto>();
        }

        /// <summary>
        /// 编辑ScheduleDetail
        /// </summary>
        protected virtual async Task<ScheduleDetailEditDto> UpdateScheduleDetailAsync(ScheduleDetailEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);
            //input.MapTo(entity);
            entity.VisitNum = input.VisitNum;

            // ObjectMapper.Map(input, entity);
            entity = await _scheduledetailRepository.UpdateAsync(entity);
            return entity.MapTo<ScheduleDetailEditDto>();
        }
        //protected virtual async Task<GrowerListDto> UpdateScheduleDetailAsync(ScheduleDetailEditDto input)
        //{
        //    //TODO:更新前的逻辑判断，是否允许更新

        //    var entity = await _scheduledetailRepository.GetAsync(input.Id.Value);
        //    input.MapTo(entity);

        //    // ObjectMapper.Map(input, entity);
        //    entity = await _scheduledetailRepository.UpdateAsync(entity);
        //    var result = entity.MapTo<ScheduleDetailEditDto>();
        //    GrowerListDto growerDto = new GrowerListDto();
        //    growerDto.MapTo<ScheduleDetailEditDto>();
        //    return 
        //}



        /// <summary>
        /// 删除ScheduleDetail信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(ScheduleDetailAppPermissions.ScheduleDetail_Delete)]
        public async Task DeleteScheduleDetail(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _scheduledetailRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除ScheduleDetail的方法
        /// </summary>
        public async Task BatchDeleteScheduleDetailsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _scheduledetailRepository.DeleteAsync(s => input.Contains(s.Id));
        }



        public async Task<List<ScheduleDetailEditDto>> CreateOrUpdateScheduleTaskAsync(List<ScheduleDetailEditDto> input)
        {
            List<ScheduleDetailEditDto> list = new List<ScheduleDetailEditDto>();
            //更新前删除逻辑
            var param = input.Where(v => v.Id.HasValue).Select(v => new { v.EmployeeId, v.TaskId, v.ScheduleId }).FirstOrDefault();
            if (param != null)
            {
                Guid?[] newIds = input.Select(v => v.Id).ToArray();
                Guid[] oldIds = _scheduledetailRepository.GetAll().Where(v => v.TaskId == param.TaskId && v.ScheduleId == param.ScheduleId).Select(v => v.Id).ToArray();
                List<Guid> diffIds = oldIds.Where(v => !newIds.Contains(v)).ToList();
                await BatchDeleteScheduleDetailsAsync(diffIds);
                await CurrentUnitOfWork.SaveChangesAsync();
            }

            foreach (var item in input)
            {
                if (item.Id.HasValue)
                {
                    list.Add(await UpdateScheduleDetailAsync(item));
                }
                else
                {
                    list.Add(await CreateScheduleDetailAsync(item));
                }
            }
            return list;
        }

        /// <summary>
        /// 获取任务完成情况数据统计
        /// </summary>
        /// <returns></returns>
        public HomeInfo GetHomeInfo()
        {
            var homeInfo = new HomeInfo();
            //var aa = await _scheduledetailRepository.GetAll().ToListAsync();
            var totalCount =  _scheduledetailRepository.GetAll().Sum(s => s.VisitNum);
            homeInfo.Total = totalCount.HasValue ? totalCount.Value : 0;
            var compCount = _scheduledetailRepository.GetAll().Sum(s => s.CompleteNum);
            homeInfo.Completed = compCount.HasValue ? compCount.Value : 0;
            if (!compCount.HasValue)
            {
                homeInfo.CompletedRate = "0%";
            }
            else
            {
                homeInfo.CompletedRate = (Math.Round((double)homeInfo.Completed / homeInfo.Total, 2) * 100).ToString() + "%";
            }
            var expirCount =  _scheduledetailRepository.GetAll().Where(s => s.Status == ScheduleStatusEnum.已逾期).Sum(s => s.VisitNum - s.CompleteNum);
            homeInfo.Expired = expirCount.HasValue ? expirCount.Value : 0;
            return homeInfo;
        }

        /// <summary>
        /// 按区域时间统计计划完成的情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<SheduleStatisticalDto>> GetSchedulByAreaTime(ScheduleDetaStatisticalInput input)
        {
            var timeNow = DateTime.Today;
            input.startTime = input.startTime.HasValue ? input.startTime : timeNow.AddDays(1 - timeNow.Day);
            input.endTime = input.endTime.HasValue ? input.endTime : timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            var query = from sd in _scheduledetailRepository.GetAll()
                        join s in _scheduleRepository.GetAll().Where(s => s.BeginTime >= input.startTime && s.BeginTime <= input.endTime) on sd.ScheduleId equals s.Id
                        join g in _growerRepository.GetAll() on sd.GrowerId equals g.Id into sg
                        from wr in sg.DefaultIfEmpty()
                        select new
                        {
                            sd.Id,
                            sd.Status,
                            sd.VisitNum,
                            sd.CompleteNum,
                            wr.CountyCode,
                        };
            var lists = query.ToList();
            var list = await query.GroupBy(s => s.CountyCode).Select(g => new SheduleStatisticalDto
            {
                GroupName = g.Key.ToString(),
                Total = g.Sum(m => m.VisitNum),
                Completed = g.Sum(m => m.CompleteNum),
                Expired = g.Where(m => m.Status == ScheduleStatusEnum.已逾期).Sum(s => s.VisitNum - s.CompleteNum)
            }).ToListAsync();
            return list;
        }

        /// <summary>
        /// 按月份统计计划完成情况
        /// </summary>
        /// <param name="searchMoth">半年或一年</param>
        /// <returns></returns>
        public async Task<List<SheduleStatisticalDto>> GetSchedulByMothTime(int searchMoth)
        {
            var timeNow = DateTime.Today;
            DateTime startTime;
            DateTime endTime;
            if (searchMoth == 2)
            {
                startTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(-11);
                endTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            }
            else
            {
                startTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(-5);
                endTime = timeNow.AddDays(1 - timeNow.Day).AddMonths(1).AddDays(-1);
            }

            //var query = from sd in _scheduledetailRepository.GetAll()
            //            join s in _scheduleRepository.GetAll().Where(s => s.BeginTime >= startTime && s.EndTime <= endTime) on sd.ScheduleId equals s.Id
            //            select new
            //            {
            //                sd.Id,
            //                sd.Status,
            //                sd.VisitNum,
            //                sd.CompleteNum,
            //                s.BeginTime,
            //                s.EndTime
            //            };
            //var cList = _scheduleRepository.GetAll().ToList();
            //var schList = _scheduleRepository.GetAll().Where(s => s.BeginTime >= startTime && s.EndTime <= endTime).ToList();
            //var lists = query.ToList();
            //var list = await query.GroupBy(s => new { s.BeginTime }).Select(g => new SheduleStatisticalDto
            //{
            //    //GroupName = g.Key.Month.ToString() + "份",
            //    Total = g.Sum(s => s.VisitNum),
            //    Completed = g.Sum(s => s.CompleteNum),
            //    Expired = g.Sum(s => s.VisitNum - s.CompleteNum) 
            //}).ToListAsync();
            var list =await _scheduledetailRepository.GetSheduleStatisticalDtosByMothAsync(startTime, endTime);

            return list;
        }

        public async Task GetSumShedule(SheduleSumInput input)
        {

        }

    }
}


