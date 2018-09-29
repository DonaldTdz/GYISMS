
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

        /// <summary>
        /// 构造函数 
        ///</summary>
        public SystemDataAppService(IRepository<SystemData, int> systemdataRepository
            , ISystemDataManager systemdataManager)
        {
            _systemdataRepository = systemdataRepository;
            _systemdataManager = systemdataManager;
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
        //public async Task<List<SelectGroup>> GetWeekTimeSelectGroup()
        //{
        //    List<RadioGroup> list = new List<RadioGroup>();
        //    RadioGroup item = new RadioGroup();
        //    DateTime dt = new DateTime();
        //    DateTime dtn = DateTime.Now;
        //    item.text = dt.Month
        //    int dayInMonthBegin = dtn.Day;   //本周第一天
        //    int dayInMonthEnd = dtn.Day + 6;
        //    list.Add()

        //    return null;
        //}

        public int getWeekNumInMonth()
        {
            DateTime daytime = DateTime.Now;
            int dayInMonth = daytime.Day;   //本月第一天   
            DateTime firstDay = daytime.AddDays(1 - daytime.Day);  //本月第一天是周几 
            int weekday = (int)firstDay.DayOfWeek == 0 ? 7 : (int)firstDay.DayOfWeek;   //本月第一周有几天
            int firstWeekEndDay = 7 - (weekday - 1);            //当前日期和第一周之差 
            int diffday = dayInMonth - firstWeekEndDay;
            diffday = diffday > 0 ? diffday : 1;            //当前是第几周,如果整除7就减一天   
            int WeekNumInMonth = ((diffday % 7) == 0 ? (diffday / 7 - 1) : (diffday / 7)) + 1 + (dayInMonth > firstWeekEndDay ? 1 : 0);
            return WeekNumInMonth;
        }
    }
}


