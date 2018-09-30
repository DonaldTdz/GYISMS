
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

using GYISMS.SystemDatas.Authorization;
using GYISMS.SystemDatas.Dtos;
using GYISMS.SystemDatas;
using GYISMS.Authorization;
using GYISMS.GYEnums;
using GYISMS.Schedules;
using Abp.Auditing;

namespace GYISMS.SystemDatas
{
    /// <summary>
    /// SystemData应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]

    public class SystemDataAppService : GYISMSAppServiceBase, ISystemDataAppService
    {
        private readonly IRepository<SystemData, int> _systemdataRepository;
        private readonly ISystemDataManager _systemdataManager;
        private readonly IRepository<Schedule, Guid> _scheduleRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public SystemDataAppService(IRepository<SystemData, int> systemdataRepository
            , ISystemDataManager systemdataManager
            , IRepository<Schedule, Guid> scheduleRepository)
        {
            _systemdataRepository = systemdataRepository;
            _systemdataManager = systemdataManager;
            _scheduleRepository = scheduleRepository;
        }


        /// <summary>
        /// 获取SystemData的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SystemDataListDto>> GetPagedSystemDatas(GetSystemDatasInput input)
        {

            var query = _systemdataRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件

            var systemdataCount = await query.CountAsync();

            var systemdatas = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var systemdataListDtos = ObjectMapper.Map<List <SystemDataListDto>>(systemdatas);
            var systemdataListDtos = systemdatas.MapTo<List<SystemDataListDto>>();

            return new PagedResultDto<SystemDataListDto>(
systemdataCount,
systemdataListDtos
                );
        }


        /// <summary>
        /// 通过指定id获取SystemDataListDto信息
        /// </summary>
        public async Task<SystemDataListDto> GetSystemDataByIdAsync(EntityDto<int> input)
        {
            var entity = await _systemdataRepository.GetAsync(input.Id);

            return entity.MapTo<SystemDataListDto>();
        }

        /// <summary>
        /// MPA版本才会用到的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetSystemDataForEditOutput> GetSystemDataForEdit(NullableIdDto<int> input)
        {
            var output = new GetSystemDataForEditOutput();
            SystemDataEditDto systemdataEditDto;

            if (input.Id.HasValue)
            {
                var entity = await _systemdataRepository.GetAsync(input.Id.Value);

                systemdataEditDto = entity.MapTo<SystemDataEditDto>();

                //systemdataEditDto = ObjectMapper.Map<List <systemdataEditDto>>(entity);
            }
            else
            {
                systemdataEditDto = new SystemDataEditDto();
            }

            output.SystemData = systemdataEditDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改SystemData的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateSystemData(CreateOrUpdateSystemDataInput input)
        {

            if (input.SystemData.Id.HasValue)
            {
                await UpdateSystemDataAsync(input.SystemData);
            }
            else
            {
                await CreateSystemDataAsync(input.SystemData);
            }
        }


        /// <summary>
        /// 新增SystemData
        /// </summary>
        //[AbpAuthorize(SystemDataAppPermissions.SystemData_Create)]
        protected virtual async Task<SystemDataEditDto> CreateSystemDataAsync(SystemDataEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<SystemData>(input);
            entity.CreationTime = new DateTime();
            entity = await _systemdataRepository.InsertAsync(entity);
            return entity.MapTo<SystemDataEditDto>();
        }

        /// <summary>
        /// 编辑SystemData
        /// </summary>
        //[AbpAuthorize(SystemDataAppPermissions.SystemData_Edit)]
        protected virtual async Task UpdateSystemDataAsync(SystemDataEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _systemdataRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _systemdataRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除SystemData信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(SystemDataAppPermissions.SystemData_Delete)]
        public async Task DeleteSystemData(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _systemdataRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除SystemData的方法
        /// </summary>
        [AbpAuthorize(SystemDataAppPermissions.SystemData_BatchDelete)]
        public async Task BatchDeleteSystemDatasAsync(List<int> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _systemdataRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 获取会议室配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<CheckBoxGroup>> GetRoomDevicesAsync()
        {
            var entity = await _systemdataRepository.GetAll().Where(v => v.Type == ConfigType.会议物资 && v.ModelId == ConfigModel.会议管理).ToListAsync();
            if (entity != null)
            {
                List<CheckBoxGroup> list = new List<CheckBoxGroup>();
                //string[] arry = entity.Split(',');
                foreach (var item in entity)
                {
                    CheckBoxGroup checkboxGroup = new CheckBoxGroup();
                    checkboxGroup.Label = item.Desc;
                    checkboxGroup.Value = item.Code;
                    list.Add(checkboxGroup);
                }
                return list;
            }
            else
            {
                return new List<CheckBoxGroup>();
            }
        }

        //public async Task<List<CheckBoxGroup>> GetRoomDevicesAsync(int? id)
        //{
        //    string[] deviceArry = null;
        //    string entity = await _systemdataRepository.GetAll().Where(v => v.Type == ConfigType.会议物资 && v.ModelId == ConfigModel.会议管理).Select(v => v.Code).FirstOrDefaultAsync();
        //    if (entity != null)
        //    {
        //        string roomDevice = await _meetingroomRepository.GetAll().Where(v => v.Id == id).Select(v => v.Devices).FirstOrDefaultAsync();
        //        if (roomDevice != null)
        //        {
        //            deviceArry = roomDevice.Split(',');
        //        }
        //        List<CheckBoxGroup> list = new List<CheckBoxGroup>();
        //        string[] arry = entity.Split(',');
        //        int i = 0;
        //        foreach (var item in arry)
        //        {
        //            CheckBoxGroup checkboxGroup = new CheckBoxGroup();
        //            checkboxGroup.Label = item;
        //            checkboxGroup.Value = item;
        //            if (roomDevice != null)
        //            {
        //                if (checkboxGroup.Label == deviceArry[i])
        //                {
        //                    checkboxGroup.Checked = true;
        //                }
        //                if (i < deviceArry.Length)
        //                {
        //                    i++;
        //                }
        //            }

        //            list.Add(checkboxGroup);
        //        }
        //        return list;
        //    }
        //    else
        //    {
        //        return new List<CheckBoxGroup>();
        //    }
        //}

        /// <summary>
        /// 根据所属模块和配置类型获取系统数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<SystemDataListDto>> GetPagedSystemDatasByType(GetSystemDatasInput input)
        {

            var query = _systemdataRepository.GetAll()
                .Where(s => s.ModelId == input.ModelId)
                .WhereIf(input.Type.HasValue, s => s.Type == input.Type);
            // TODO:根据传入的参数添加过滤条件

            var systemdataCount = await query.CountAsync();

            var systemdatas = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var systemdataListDtos = ObjectMapper.Map<List <SystemDataListDto>>(systemdatas);
            var systemdataListDtos = systemdatas.MapTo<List<SystemDataListDto>>();

            return new PagedResultDto<SystemDataListDto>(
                 systemdataCount,
                 systemdataListDtos
                );
        }

        /// <summary>
        /// 更新或修改系统数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateSystemDataNew(SystemDataEditDto input)
        {
            if (input.Id.HasValue)
            {
                await UpdateSystemDataAsync(input);
            }
            else
            {
                input.CreationTime = DateTime.Now;
                await CreateSystemDataAsync(input);
            }
        }

        public async Task<SystemDataListDto> GetSystemDataById(int id)
        {
            var entity = await _systemdataRepository.GetAsync(id);

            return entity.MapTo<SystemDataListDto>();
        }


        /// <summary>
        /// 获取烟农单位Select
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectGroup>> GetUnitType()
        {
            var entity = await (from c in _systemdataRepository.GetAll().Where(v => v.ModelId == ConfigModel.烟叶服务 && v.Type == ConfigType.烟农单位)
                                select new
                                {
                                    text = c.Desc,
                                    value = c.Code,
                                    seq = c.Seq
                                }).OrderBy(v => v.seq).AsNoTracking().ToListAsync();

            return entity.MapTo<List<SelectGroup>>();
        }

        /// <summary>
        /// 获取烟农区县Radio
        /// </summary>
        /// <returns></returns>
        public async Task<List<RadioGroup>> GetCountyCodes()
        {
            var entity = await (from c in _systemdataRepository.GetAll().Where(v => v.ModelId == ConfigModel.烟叶服务 && v.Type == ConfigType.烟农村组)
                                select new
                                {
                                    text = c.Desc,
                                    value = c.Code,
                                    seq = c.Seq
                                }).OrderBy(v => v.seq).AsNoTracking().ToListAsync();
            return entity.MapTo<List<RadioGroup>>();
        }

        /// <summary>
        /// 获取烟农区县Select
        /// </summary>
        /// <returns></returns>
        public async Task<List<SelectGroup>> GetCountyCodesSelectGroup()
        {
            var entity = await (from c in _systemdataRepository.GetAll().Where(v => v.ModelId == ConfigModel.烟叶服务 && v.Type == ConfigType.烟农村组)
                                select new
                                {
                                    text = c.Desc,
                                    value = c.Code,
                                    seq = c.Seq
                                }).OrderBy(v => v.seq).AsNoTracking().ToListAsync();

            return entity.MapTo<List<SelectGroup>>();
        }

        /// <summary>
        /// 获取周计划时间
        /// </summary>
        /// <returns></returns>
        public List<SelectGroup> GetWeekOfMonth()
        //public async Task<List<SelectGroup>> GetWeekOfMonthAsync(Guid? Id)
        {
            List<SelectGroup> list = new List<SelectGroup>();
            DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);  //该月第一天  
            int dayOfWeek = 7;
                if (Convert.ToInt32(startMonth.DayOfWeek.ToString("d")) > 0)
                    dayOfWeek = Convert.ToInt32(startMonth.DayOfWeek.ToString("d"));  //该月第一天为星期几  
                DateTime startWeek = startMonth.AddDays(1 - dayOfWeek);  //该月第一周开始日期  
                for (int i = 1; i <= 4; i++)
                {
                    SelectGroup item = new SelectGroup();
                    DateTime startDayOfWeeks = startWeek.AddDays(i * 7);  //index周的起始日期 
                    DateTime endDayOfWeeks = startWeek.AddDays((i * 7) + 6);  //index周的结束日期 
                    item.text = DateTime.Now.Month + "月第" + i + "周(" + startDayOfWeeks.ToString("D") + " - " + endDayOfWeeks.ToString("D") + ")";
                    item.value = string.Format($"{startDayOfWeeks.ToString("yyyy-MM-dd")},{endDayOfWeeks.ToString("yyyy-MM-dd")}");
                    list.Add(item);
                }
                return list;
        }
    }
}


