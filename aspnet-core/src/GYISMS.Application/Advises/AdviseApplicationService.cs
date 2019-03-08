
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


using GYISMS.Advises;
using GYISMS.Advises.Dtos;
using GYISMS.Advises.DomainService;
using GYISMS.Advises.Authorization;
using GYISMS.Dtos;
using GYISMS.Organizations.Dtos;
using System.IO;
using Senparc.CO2NET.Helpers;
using System.Text;
using Senparc.CO2NET.HttpUtility;
using GYISMS.DingDing.Dtos;
using GYISMS.DingDing;
using GYISMS.SystemDatas;
using GYISMS.GYEnums;
using GYISMS.Employees;
using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using GYISMS.Documents;
using System.Collections;
using GYISMS.Organizations;
using GYISMS.DocCategories;

namespace GYISMS.Advises
{
    /// <summary>
    /// Advise应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class AdviseAppService : GYISMSAppServiceBase, IAdviseAppService
    {
        private readonly IRepository<Advise, Guid> _entityRepository;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IAdviseManager _entityManager;
        private readonly IDingDingAppService _dingDingAppService;
        private readonly IRepository<SystemData, int> _systemdataRepository;
        private readonly IRepository<Document, Guid> _docRepository;
        private readonly IRepository<Organization, long> _organizationRepository;
        private readonly IRepository<DocCategory, int> _categoryRepository;


        /// <summary>
        /// 构造函数 
        ///</summary>
        public AdviseAppService(
        IRepository<Advise, Guid> entityRepository
        , IAdviseManager entityManager
        , IDingDingAppService dingDingAppService
        , IRepository<SystemData, int> systemdataRepository
        , IRepository<Employee, string> employeeRepository
        , IRepository<Document, Guid> docRepository
        , IRepository<Organization, long> organizationRepository
        , IRepository<DocCategory, int> categoryRepository
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _dingDingAppService = dingDingAppService;
            _systemdataRepository = systemdataRepository;
            _employeeRepository = employeeRepository;
            _docRepository = docRepository;
            _organizationRepository = organizationRepository;
            _categoryRepository = categoryRepository;
        }


        /// <summary>
        /// 获取Advise的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AdviseDto>> GetPaged(GetAdvisesInput input)
        {
            var employee = _employeeRepository.GetAll();
            var advise = _entityRepository.GetAll();
            var doc = _docRepository.GetAll();
            //var org = _organizationRepository.GetAll().Select(v=>new { v.Id,v.DepartmentName});

            var result = await(from a in advise
                         join e in employee on a.EmployeeId equals e.Id
                         join d in doc on a.DocumentId equals d.Id
                         where e.Department.Contains(input.DeptId.ToString())                        
                         select new AdviseDto()
                         {
                             Id = a.Id,
                             Content = a.Content,
                             EmployeeName = a.EmployeeName,
                             CreationTime = a.CreationTime,
                             DocumentName = d.Name,
                             CategoryCode = d.CategoryCode
                             //DepartmentName = o.DepartmentName
                         }).WhereIf(!string.IsNullOrEmpty(input.KeyWord),v=>v.EmployeeName.Contains(input.KeyWord)||v.DocumentName.Contains(input.KeyWord)).OrderByDescending(v=>v.CreationTime).ToListAsync();

            foreach (var item in result)
            {               
                string[] tempCode = item.CategoryCode.Split(',');
                foreach (var temp in tempCode)
                {
                    string tempName = await _categoryRepository.GetAll().Where(v => v.Id == Convert.ToInt32(temp)).Select(v => v.Name).FirstOrDefaultAsync();
                    item.CategoryName = string.IsNullOrEmpty(item.CategoryName) == true? tempName : (item.CategoryName + "," + tempName);
                }
            }
            var count = result.Count();
            return new PagedResultDto<AdviseDto>(count, result);
        }


        /// <summary>
        /// 通过指定id获取AdviseListDto信息
        /// </summary>
        [AbpAuthorize(AdvisePermissions.Query)]
        public async Task<AdviseListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<AdviseListDto>();
        }

        /// <summary>
        /// 获取编辑 Advise
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdvisePermissions.Create, AdvisePermissions.Edit)]
        public async Task<GetAdviseForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetAdviseForEditOutput();
            AdviseEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<AdviseEditDto>();

                //adviseEditDto = ObjectMapper.Map<List<adviseEditDto>>(entity);
            }
            else
            {
                editDto = new AdviseEditDto();
            }

            output.Advise = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Advise的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdvisePermissions.Create, AdvisePermissions.Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateAdviseInput input)
        {

            if (input.Advise.Id.HasValue)
            {
                await Update(input.Advise);
            }
            else
            {
                await Create(input.Advise);
            }
        }


        /// <summary>
        /// 新增Advise
        /// </summary>
        [AbpAuthorize(AdvisePermissions.Create)]
        protected virtual async Task<AdviseEditDto> Create(AdviseEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Advise>(input);
            var entity = input.MapTo<Advise>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<AdviseEditDto>();
        }

        /// <summary>
        /// 编辑Advise
        /// </summary>
        [AbpAuthorize(AdvisePermissions.Edit)]
        protected virtual async Task Update(AdviseEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Advise信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdvisePermissions.Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Advise的方法
        /// </summary>
        [AbpAuthorize(AdvisePermissions.BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 新增意见反馈
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<APIResultDto> CreateAdviseAsync(AdviseEditDto input)
        {
            var entity = input.MapTo<Advise>();
            var id = await _entityRepository.InsertAndGetIdAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();
            await SendMessageToAdminAsync(input.EmployeeId,input.DocumentId,id);
            return new APIResultDto() { Code = 0, Msg = "ok" };
        }

        /// <summary>
        /// 发送钉钉工作通知
        /// </summary>
        public async Task<APIResultDto> SendMessageToAdminAsync(string employeeId,Guid docId,Guid adId)
        {
            try
            {
                //获取消息模板配置
                //string messageTitle = "您有新的意见反馈";
                string messageMediaId = await _systemdataRepository.GetAll().Where(v => v.ModelId == ConfigModel.钉钉配置 && v.Type == ConfigType.企业标准库 && v.Code == GYCode.DocMediaId).Select(v => v.Desc).FirstOrDefaultAsync();
                string docName = await _docRepository.GetAll().Where(v => v.Id == docId).Select(v => v.Name).FirstOrDefaultAsync();
                var deptdesc = await _systemdataRepository.GetAll().Where(v => v.ModelId == ConfigModel.钉钉配置 && v.Type == ConfigType.企业标准库 && v.Code == GYCode.DeptArry).Select(v => v.Desc).FirstOrDefaultAsync();
                string[] deptArry = deptdesc.Split(',');
                var userDept = await _employeeRepository.GetAll().Where(v => v.Id == employeeId).Select(v => v.Department).FirstOrDefaultAsync();
                string userDeptId = userDept.Split('[')[1].Split(']')[0];
                List<string> adminIds = new List<string>();
                foreach (var item in deptArry)
                {

                    adminIds.AddRange(await _employeeRepository.GetAll().Where(v => v.Department.Contains(userDeptId) && v.Position == item).Select(v => v.Id).ToListAsync());
                }

                DingDingAppConfig ddConfig = _dingDingAppService.GetDingDingConfigByApp(DingDingAppEnum.资料标准库);
                string accessToken = _dingDingAppService.GetAccessToken(ddConfig.Appkey, ddConfig.Appsecret);
                string tempIds = string.Join(",", adminIds);
                var msgdto = new DingMsgDto();
                msgdto.userid_list = tempIds;
                msgdto.to_all_user = false;
                msgdto.agent_id = ddConfig.AgentID;
                msgdto.msg.msgtype = "link";
                msgdto.msg.link.title = "您有新的意见反馈";
                //msgdto.msg.text.content = $"您有新的意见反馈[{docName}],请进入后台进行查看";
                msgdto.msg.link.picUrl = messageMediaId;
                msgdto.msg.link.text = $"您有新的意见反馈[{ docName}] " + DateTime.Now.ToString();
                msgdto.msg.link.messageUrl = "eapp://page/advise/advise?id=" + adId;
                //msgdto.msg.link.messageUrl = "eapp://";
                var url = string.Format("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={0}", accessToken);
                var jsonString = SerializerHelper.GetJsonString(msgdto, null);
                using (MemoryStream ms = new MemoryStream())
                {
                    var bytes = Encoding.UTF8.GetBytes(jsonString);
                    ms.Write(bytes, 0, bytes.Length);
                    ms.Seek(0, SeekOrigin.Begin);
                    var obj = Post.PostGetJson<object>(url, null, ms);
                };
                return new APIResultDto() { Code = 0, Msg = "钉钉消息发送成功" };
            }
            catch (Exception ex)
            {
                Logger.ErrorFormat("SendMessageToEmployeeAsync errormsg{0} Exception{1}", ex.Message, ex);
                return new APIResultDto() { Code = 901, Msg = "钉钉消息发送失败" };
            }
        }

        /// <summary>
        /// 上传图片并返回MeadiaId
        /// </summary>
        public object UpdateAndGetAdviseMediaId(string path)
        {
            IDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/media/upload");
            OapiMediaUploadRequest request = new OapiMediaUploadRequest();
            request.Type = "image";
            request.Media = new Top.Api.Util.FileItem($@"{path}");
            DingDingAppConfig ddConfig = _dingDingAppService.GetDingDingConfigByApp(DingDingAppEnum.资料标准库);
            string accessToken = _dingDingAppService.GetAccessToken(ddConfig.Appkey, ddConfig.Appsecret);
            OapiMediaUploadResponse response = client.Execute(request, accessToken);
            return response;
        }

        /// <summary>
        /// 钉钉获取意见
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<DDAdviseDto> GetDDAdviseByIdAsync(Guid id)
        {
            var entity = await _entityRepository.GetAll().Where(v=>v.Id ==id).FirstOrDefaultAsync();
            var dto = entity.MapTo<AdviseListDto>();
            string docName =await _docRepository.GetAll().Where(v => v.Id == dto.DocumentId).Select(v => v.Name).FirstOrDefaultAsync();
            var result = new DDAdviseDto();
            result.DocumentName = docName;
            result.CreationTime = dto.CreationTime;
            result.EmployeeName = dto.EmployeeName;
            result.Content = dto.Content;
            return result;
        }
    }
}


