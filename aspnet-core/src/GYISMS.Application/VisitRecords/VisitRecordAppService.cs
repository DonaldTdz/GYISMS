
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
using Abp.Auditing;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.Primitives;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Processing.Text;
using GYISMS.Helpers;
using GYISMS.SystemDatas;
using GYISMS.ScheduleTasks;
//using PT = SixLabors.ImageSharp.Processing.Processors.Text;

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
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IRepository<SystemData> _systemDataRepository;

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
            , IRepository<SystemData> systemDataRepository
            , IHostingEnvironment env
            )
        {
            _visitrecordRepository = visitrecordRepository;
            _scheduleDetailRepository = scheduleDetailRepository;
            _visitTaskRepository = visitTaskRepository;
            _taskExamineRepository = taskExamineRepository;
            _visitExamineRepository = visitExamineRepository;
            _visitrecordManager = visitrecordManager;
            _employeeRepository = employeeRepository;
            _systemDataRepository = systemDataRepository;
            _hostingEnvironment = env;
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
            var scheduleDetail = _scheduleDetailRepository.GetAll();
            var task = _visitTaskRepository.GetAll();
            var taskExamine = _taskExamineRepository.GetAll();
            var query = await (from r in record
                               join e in employee on r.EmployeeId equals e.Id
                               join s in scheduleDetail on r.ScheduleDetailId equals s.Id
                               join t in task on s.TaskId equals t.Id
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
                                   EmployeeName = e.Name,
                                   TaskName = t.Name,
                                   HasExamine = t.IsExamine,
                                   TaskId = t.Id
                               }).ToListAsync();
            foreach (var item in query)
            {
                if (item.HasExamine == true)
                {
                    var list = _taskExamineRepository.GetAll().Where(v => v.TaskId == item.TaskId).Select(v => v.Name).ToList();
                    string examineName = string.Join(",", list.ToArray());
                    item.ExaminesName = examineName;
                }
            }
            var visitrecordCount =  query.Count();
            //var visitrecords = await query
            //        .OrderByDescending(v => v.SignTime).AsNoTracking()
            //        .PageBy(input)
            //        .ToListAsync();
            var visitrecords = query
                    .OrderByDescending(v => v.SignTime)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount);

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
        [Audited]
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

        private string GetWeekDay(DayOfWeek dayWeek)
        {
            switch (dayWeek)
            {
                case DayOfWeek.Friday:
                    return "星期五";
                case DayOfWeek.Monday:
                    return "星期一";
                case DayOfWeek.Saturday:
                    return "星期六";
                case DayOfWeek.Sunday:
                    return "星期日";
                case DayOfWeek.Thursday:
                    return "星期四";
                case DayOfWeek.Tuesday:
                    return "星期二";
                case DayOfWeek.Wednesday:
                    return "星期三";
                default:
                    return string.Empty;
            }
        }

        private string GenerateWatermarkImg(string imgPath, string location, string userName, string growerName)
        {
            //拜访时间
            DateTime stime = DateTime.Now;
            var host = _hostingEnvironment.WebRootPath;
            var imgFullPath = host + imgPath;
            using (FileStream stream = File.OpenRead(imgFullPath))
            using (Image<Rgba32> vimage = Image.Load(stream))
            {
                //画文字
                var fontCollection = new FontCollection();
                var fontPath = "C:/Windows/Fonts/simkai.ttf";
                //var fontPath = "C:/Windows/Fonts/STXINWEI.TTF";
                //var fontPath = "C:/Windows/Fonts/simfang.ttf";
                var fontTitle = new Font(fontCollection.Install(fontPath), 20, FontStyle.Bold);
                var font = new Font(fontCollection.Install(fontPath), 12, FontStyle.Bold);
                //var fontTitle = SystemFonts.CreateFont("Microsoft YaHei UI", 20, FontStyle.Bold);
                //var font = SystemFonts.CreateFont("Microsoft YaHei UI", 12, FontStyle.Bold);
                vimage.Mutate(x => x.DrawText(stime.ToString("HH:mm"), fontTitle, Rgba32.White, new PointF(10, 5)));
                vimage.Mutate(x => x.DrawText(string.Format("{0} {1}", stime.ToString("yyyy.MM.dd"), GetWeekDay(stime.DayOfWeek)), font, Rgba32.White, new PointF(10, 30)));
                vimage.Mutate(x => x.DrawText(string.Format("拜访烟农: {0}", growerName), font, Rgba32.White, new PointF(10, 48)));
                TextGraphicsOptions options = new TextGraphicsOptions(true)
                {
                    Antialias = true,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center
                };
                var height = vimage.Height;
                vimage.Mutate(x => x.DrawText(options, "用户: " + userName, font, Rgba32.White, new PointF(350, height - 46)));
                vimage.Mutate(x => x.DrawText(options, "位置: " + location, font, Rgba32.White, new PointF(350, height - 28)));
                var newImagePath = imgPath.Replace("visit", "visit/watermark");
                var newFolder = host + "/visit/watermark";
                if (!Directory.Exists(newFolder))
                {
                    Directory.CreateDirectory(newFolder);
                }
                vimage.Save(host + newImagePath);
                return newImagePath;
            }
        }

        [AbpAllowAnonymous]
        //[DisableValidation]
        [Audited]
        public async Task<APIResultDto> SaveDingDingVisitRecordAsync(DingDingVisitRecordInputDto input)
        {
            var vistitRecord = input.MapTo<VisitRecord>();
            vistitRecord.SignTime = DateTime.Now;
            //计划明细
            var detail = await _scheduleDetailRepository.GetAsync(input.ScheduleDetailId);
            //生成水印图片
            vistitRecord.ImgPath = GenerateWatermarkImg(vistitRecord.ImgPath, vistitRecord.Location, detail.EmployeeName, detail.GrowerName);
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

            detail.CompleteNum++;
            detail.Status = detail.CompleteNum == detail.VisitNum ? ScheduleStatusEnum.已完成 : ScheduleStatusEnum.进行中;

            return new APIResultDto() { Code = 0, Msg = "保存数据成功" };
        }

        [AbpAllowAnonymous]
        public Task GenerateWatermarkImgTests()
        {
            return Task.FromResult(GenerateWatermarkImg("/visit/bbed0bd3-6435-44e9-b86e-89556982fdfd.jpg", "四川成都戛纳湾金棕榈", "烟技员","烟农"));
        }
        [AbpAllowAnonymous]
        [Audited]
        public async Task<APIResultDto> ValidateLocationAsync(double lat, double lon, double latGrower, double lonGrower)
        {
            var distance = AbpMapByGoogle.GetDistance(lat, lon, latGrower, lonGrower);
            var signRange = await _systemDataRepository.GetAll().Where(s => s.ModelId == ConfigModel.烟叶服务 && s.Type == ConfigType.烟叶公共 && s.Code == GYCode.SignRange).FirstOrDefaultAsync();
            var range = 500d;
            if (signRange != null)
            {
                range = double.Parse(signRange.Desc);
            }
            if (distance < range)
            {
                return new APIResultDto() { Code = 0, Msg = "ok"};
            }
            else
            {
                return new APIResultDto() { Code = 901, Msg = "当前位置不在拜访位置范围内" };
            }
        }

        [Audited]
        [AbpAllowAnonymous]
        public async Task<DingDingVisitRecordInputDto> GetDingDingVisitRecordAsync(Guid id)
        {
            var query = from vr in _visitrecordRepository.GetAll()
                        join sd in _scheduleDetailRepository.GetAll() on vr.ScheduleDetailId equals sd.Id
                        join t in _visitTaskRepository.GetAll() on sd.TaskId equals t.Id
                        join e in _employeeRepository.GetAll() on vr.EmployeeId equals e.Id
                        where vr.Id == id
                        select new DingDingVisitRecordInputDto()
                        {
                            ScheduleDetailId = sd.Id,
                            EmployeeId = sd.EmployeeId,
                            EmployeeName = sd.EmployeeName,
                            GrowerName = sd.GrowerName,
                            TaskDesc = t.Name + "（"+ t.Type.ToString() + "）",
                            SignTime = vr.SignTime,
                            Desc = vr.Desc,
                            ImgPath = vr.ImgPath,
                            Location = vr.Location,
                            Latitude = vr.Latitude,
                            Longitude = vr.Longitude,
                            EmployeeImg = e.Avatar
                        };
            var dmdata = await query.FirstOrDefaultAsync();

            var examQuery = from ve in _visitExamineRepository.GetAll()
                            join te in _taskExamineRepository.GetAll() on ve.TaskExamineId equals te.Id
                            where ve.VisitRecordId == id
                            select new DingDingTaskExamineDto()
                            {
                                Name = te.Name,
                                Desc = te.Desc,
                                Score = ve.Score
                            };

            dmdata.Examines = await examQuery.ToListAsync();
            return dmdata;
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

    }
}


