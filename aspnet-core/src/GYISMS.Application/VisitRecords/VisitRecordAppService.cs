
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

using GYISMS.VisitRecords.Authorization;
using GYISMS.VisitRecords.Dtos;
using GYISMS.VisitRecords;
using GYISMS.Authorization;
using GYISMS.ScheduleDetails;
using GYISMS.VisitTasks;
using GYISMS.TaskExamines;
using GYISMS.Dtos;
using GYISMS.VisitExamines;
using GYISMS.GYEnums;
using GYISMS.Employees;
using Abp.Runtime.Validation;

namespace GYISMS.VisitRecords
{
    /// <summary>
    /// VisitRecord应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class VisitRecordAppService : GYISMSAppServiceBase, IVisitRecordAppService
    {
        private readonly IRepository<VisitRecord, Guid> _visitrecordRepository;
        private readonly IRepository<ScheduleDetail, Guid> _scheduleDetailRepository;
        private readonly IRepository<VisitTask> _visitTaskRepository;
        private readonly IRepository<TaskExamine> _taskExamineRepository;
        private readonly IRepository<VisitExamine, Guid> _visitExamineRepository;
        private readonly IRepository<Employee, string> _employeeRepository;

        private readonly IVisitRecordManager _visitrecordManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public VisitRecordAppService(
            IRepository<VisitRecord, Guid> visitrecordRepository
            , IRepository<ScheduleDetail, Guid> scheduleDetailRepository
            , IRepository<VisitTask> visitTaskRepository
            , IRepository<TaskExamine> taskExamineRepository
            , IRepository<VisitExamine, Guid> visitExamineRepository
            , IVisitRecordManager visitrecordManager
            , IRepository<Employee, string> employeeRepository
            )
        {
            _visitrecordRepository = visitrecordRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _visitTaskRepository = visitTaskRepository;
            _taskExamineRepository = taskExamineRepository;
            _visitExamineRepository = visitExamineRepository;
            _visitrecordManager = visitrecordManager;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取VisitRecord的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<VisitRecordListDto>> GetPagedVisitRecords(GetVisitRecordsInput input)
        {

            var query = _visitrecordRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var visitrecordCount = await query.CountAsync();

            var visitrecords = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var visitrecordListDtos = ObjectMapper.Map<List <VisitRecordListDto>>(visitrecords);
            var visitrecordListDtos = visitrecords.MapTo<List<VisitRecordListDto>>();

            return new PagedResultDto<VisitRecordListDto>(
            visitrecordCount,
            visitrecordListDtos
                );
        }

        /// <summary>
        /// 获取烟农被拜访记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<VisitRecordListDto>> GetVisitRecordsByGrowerId(GetVisitRecordsInput input)
        {
            var record = _visitrecordRepository.GetAll().Where(v => v.GrowerId == input.GrowerId);
            var employee = _employeeRepository.GetAll();
            var query = from r in record
                        join e in employee on r.EmployeeId equals e.Id
                        select new VisitRecordListDto()
                        {
                            Id = r.Id,
                            EmployeeId = r.EmployeeId,
                            GrowerId = r.GrowerId,
                            Location = r.Location,
                            Longitude = r.Longitude,
                            SignTime = r.SignTime,
                            ScheduleDetailId = r.ScheduleDetailId,
                            Desc = r.Desc,
                            ImgPath = r.ImgPath,
                            Latitude = r.Latitude,
                            CreationTime = r.CreationTime,
                            EmployeeName = e.Name
                        };

            var visitrecordCount = await query.CountAsync();
            var visitrecords = await query
                    .OrderByDescending(v => v.SignTime).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            var visitrecordListDtos = visitrecords.MapTo<List<VisitRecordListDto>>();

            return new PagedResultDto<VisitRecordListDto>(
                        visitrecordCount,
                        visitrecordListDtos
                );
        }
        /// <summary>
        /// 通过指定id获取VisitRecordListDto信息
        /// </summary>
        public async Task<VisitRecordListDto> GetVisitRecordByIdAsync(EntityDto<Guid> input)
        {
            var entity = await _visitrecordRepository.GetAsync(input.Id);

            return entity.MapTo<VisitRecordListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetVisitRecordForEditOutput> GetVisitRecordForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetVisitRecordForEditOutput();
            VisitRecordEditDto visitrecordEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _visitrecordRepository.GetAsync(input.Id.Value);

                visitrecordEditDto = entity.MapTo<VisitRecordEditDto>();

                //visitrecordEditDto = ObjectMapper.Map<List <visitrecordEditDto>>(entity);
            }
            else
            {
                visitrecordEditDto = new VisitRecordEditDto();
            }

            output.VisitRecord = visitrecordEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改VisitRecord的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateVisitRecord(CreateOrUpdateVisitRecordInput input)
        {

            if (input.VisitRecord.Id.HasValue)
            {
                await UpdateVisitRecordAsync(input.VisitRecord);
            }
            else
            {
                await CreateVisitRecordAsync(input.VisitRecord);
            }
        }


        /// <summary>
        /// 新增VisitRecord
        /// </summary>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_Create)]
        protected virtual async Task<VisitRecordEditDto> CreateVisitRecordAsync(VisitRecordEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<VisitRecord>(input);

            entity = await _visitrecordRepository.InsertAsync(entity);
            return entity.MapTo<VisitRecordEditDto>();
        }

        /// <summary>
        /// 编辑VisitRecord
        /// </summary>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_Edit)]
        protected virtual async Task UpdateVisitRecordAsync(VisitRecordEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _visitrecordRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _visitrecordRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除VisitRecord信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_Delete)]
        public async Task DeleteVisitRecord(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _visitrecordRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除VisitRecord的方法
        /// </summary>
        [AbpAuthorize(VisitRecordAppPermissions.VisitRecord_BatchDelete)]
        public async Task BatchDeleteVisitRecordsAsync(List<Guid> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _visitrecordRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        [AbpAllowAnonymous]
        public async Task<DingDingVisitRecordInputDto> GetCreateDingDingVisitRecordAsync(Guid scheduleDetailId)
        {
            var query = from sd in _scheduleDetailRepository.GetAll()
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                        where sd.Id == scheduleDetailId
                        select new
                        {
                            sd.Id,
                            sd.EmployeeId,
                            sd.GrowerId,
                            sd.GrowerName,
                            t.Name,
                            t.Type,
                            t.Desc,
                            TaskId = t.Id
                        };
            var dmdata = await query.FirstOrDefaultAsync();
            var result = new DingDingVisitRecordInputDto()
            {
                ScheduleDetailId = dmdata.Id,
                EmployeeId = dmdata.EmployeeId,
                GrowerId = dmdata.GrowerId,
                GrowerName = dmdata.GrowerName,
                TaskDesc = string.Format("{0}（{1}），{2}", dmdata.Name, dmdata.Type.ToString(), dmdata.Desc)
            };

            var examines = await _taskExamineRepository.GetAll().Where(t => t.TaskId == dmdata.TaskId).OrderBy(e => e.Seq).ToListAsync();
            result.Examines = examines.MapTo<List<DingDingTaskExamineDto>>();
            return result;
        }

        [AbpAllowAnonymous]
        //[DisableValidation]
        public async Task<APIResultDto> SaveDingDingVisitRecordAsync(DingDingVisitRecordInputDto input)
        {
            var vistitRecord = input.MapTo<VisitRecord>();
            vistitRecord.SignTime = DateTime.Now;
            //拜访记录
            var vrId = await _visitrecordRepository.InsertAndGetIdAsync(vistitRecord);
            await CurrentUnitOfWork.SaveChangesAsync();
            //考核项
            foreach (var item in input.Examines)
            {
                var ve = new VisitExamine()
                {
                    EmployeeId = input.EmployeeId,
                    GrowerId = input.GrowerId,
                    Score = item.Score,
                    TaskExamineId = item.Id,
                    VisitRecordId = vrId
                };
                await _visitExamineRepository.InsertAsync(ve);
            }
            //更新拜访明细
            var detail = await _scheduleDetailRepository.GetAsync(input.ScheduleDetailId);
            detail.CompleteNum++;
            detail.Status = detail.CompleteNum == detail.VisitNum ? ScheduleStatusEnum.已完成 : ScheduleStatusEnum.进行中;

            return new APIResultDto() { Code = 0, Msg = "保存数据成功" };
        }

        /// <summary>
        /// 导出VisitRecord为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetVisitRecordsToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}



        //// custom codes

        //// custom codes end

    }
}


