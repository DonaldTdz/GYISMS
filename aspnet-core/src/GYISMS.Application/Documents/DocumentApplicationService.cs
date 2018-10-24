
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


using GYISMS.Documents;
using GYISMS.Documents.Dtos;
using GYISMS.Documents.DomainService;
using GYISMS.DocAttachments;
using GYISMS.Dtos;

namespace GYISMS.Documents
{
    /// <summary>
    /// Document应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DocumentAppService : GYISMSAppServiceBase, IDocumentAppService
    {
        private readonly IRepository<Document, Guid> _entityRepository;
        private readonly IRepository<DocAttachment, int> _docAttachmentRepository;
        private readonly IDocumentManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DocumentAppService(
        IRepository<Document, Guid> entityRepository
        , IDocumentManager entityManager
            , IRepository<DocAttachment, int> docAttachmentRepository
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _docAttachmentRepository = docAttachmentRepository;
        }


        /// <summary>
        /// 获取Document的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DocumentListDto>> GetPaged(GetDocumentsInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(input.CategoryId.HasValue, e => e.CategoryId == input.CategoryId)
                .WhereIf(!string.IsNullOrEmpty(input.KeyWord), e => e.Name.Contains(input.KeyWord) || e.Summary.Contains(input.KeyWord));

            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<DocumentListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<DocumentListDto>>();

            return new PagedResultDto<DocumentListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取DocumentListDto信息
        /// </summary>

        public async Task<DocumentListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<DocumentListDto>();
        }

        /// <summary>
        /// 获取编辑 Document
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetDocumentForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetDocumentForEditOutput();
            DocumentEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<DocumentEditDto>();

                //documentEditDto = ObjectMapper.Map<List<documentEditDto>>(entity);
            }
            else
            {
                editDto = new DocumentEditDto();
            }

            output.Document = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改Document的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<APIResultDto> CreateOrUpdate(CreateOrUpdateDocumentInput input)
        {

            if (input.Document.Id.HasValue)
            {
                await Update(input.Document);
                return new APIResultDto() { Code = 0, Msg = "保存成功" };
            }
            else
            {
                var entity = await Create(input.Document);
                return new APIResultDto() { Code = 0, Msg = "保存成功", Data = entity.Id };
            }
        }


        /// <summary>
        /// 新增Document
        /// </summary>

        protected virtual async Task<DocumentEditDto> Create(DocumentEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <Document>(input);
            var entity = input.MapTo<Document>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<DocumentEditDto>();
        }

        /// <summary>
        /// 编辑Document
        /// </summary>

        protected virtual async Task Update(DocumentEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除Document信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Document的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 导出Document为excel表,等待开发。
        /// </summary>
        /// <returns></returns>
        //public async Task<FileDto> GetToExcel()
        //{
        //	var users = await UserManager.Users.ToListAsync();
        //	var userListDtos = ObjectMapper.Map<List<UserListDto>>(users);
        //	await FillRoleNames(userListDtos);
        //	return _userListExcelExporter.ExportToFile(userListDtos);
        //}


        /// <summary>
        /// 批量删除Document的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }


        /// <summary>
        /// 钉钉获取资料详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<DocumentListDto> GetDocInfoAsync(int id)
        {
            var doc = _entityRepository.GetAll().Where(v => v.CategoryId == id).AsNoTracking();
            var att = _docAttachmentRepository.GetAll().AsNoTracking();
            var result = await (from d in doc
                                join a in att on d.Id equals a.DocId into table
                                from t in table.DefaultIfEmpty()
                                select new DocumentListDto()
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    CategoryDesc = d.CategoryDesc,
                                    ReleaseDate = d.ReleaseDate,
                                    Summary = d.Summary,
                                    Content = d.Content,
                                    FileName = t.Name,
                                    FileUrl = t.Path,
                                    QrCodeUrl = d.QrCodeUrl
                                }).AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// 钉钉扫码搜索
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<DocumentListDto> GetDocInfoByScanAsync(Guid id)
        {
            var doc = _entityRepository.GetAll().Where(v => v.Id == id).AsNoTracking();
            var att = _docAttachmentRepository.GetAll().AsNoTracking();
            var result = await (from d in doc
                                join a in att on d.Id equals a.DocId into table
                                from t in table.DefaultIfEmpty()
                                select new DocumentListDto()
                                {
                                    Id = d.Id,
                                    Name = d.Name,
                                    CategoryDesc = d.CategoryDesc,
                                    ReleaseDate = d.ReleaseDate,
                                    Summary = d.Summary,
                                    Content = d.Content,
                                    FileName = t.Name,
                                    FileUrl = t.Path,
                                    QrCodeUrl = d.QrCodeUrl
                                }).AsNoTracking().FirstOrDefaultAsync();
            return result;
        }

        /// <summary>
        /// 获取文件列表项
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<DocumentListDto>> GetDocListByParentIdAsync(int parentId)
        {
            var entity = await _entityRepository.GetAll().Where(v => v.CategoryId == parentId).OrderBy(v => v.Id).AsNoTracking().ToListAsync();
            return entity.MapTo<List<DocumentListDto>>();
        }
    }
}


