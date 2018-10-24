
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


using GYISMS.DocAttachments;
using GYISMS.DocAttachments.Dtos;
using GYISMS.DocAttachments.DomainService;



namespace GYISMS.DocAttachments
{
    /// <summary>
    /// DocAttachment应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DocAttachmentAppService : GYISMSAppServiceBase, IDocAttachmentAppService
    {
        private readonly IRepository<DocAttachment, Guid> _entityRepository;

        private readonly IDocAttachmentManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DocAttachmentAppService(
        IRepository<DocAttachment, Guid> entityRepository
        , IDocAttachmentManager entityManager
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
        }


        /// <summary>
        /// 获取DocAttachment的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DocAttachmentListDto>> GetPaged(GetDocAttachmentsInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<DocAttachmentListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<DocAttachmentListDto>>();

            return new PagedResultDto<DocAttachmentListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取DocAttachmentListDto信息
        /// </summary>

        public async Task<DocAttachmentListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<DocAttachmentListDto>();
        }

        /// <summary>
        /// 获取编辑 DocAttachment
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetDocAttachmentForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetDocAttachmentForEditOutput();
            DocAttachmentEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<DocAttachmentEditDto>();

                //docAttachmentEditDto = ObjectMapper.Map<List<docAttachmentEditDto>>(entity);
            }
            else
            {
                editDto = new DocAttachmentEditDto();
            }

            output.DocAttachment = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改DocAttachment的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateDocAttachmentInput input)
        {

            if (input.DocAttachment.Id.HasValue)
            {
                await Update(input.DocAttachment);
            }
            else
            {
                await Create(input.DocAttachment);
            }
        }


        /// <summary>
        /// 新增DocAttachment
        /// </summary>

        protected virtual async Task<DocAttachmentEditDto> Create(DocAttachmentEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <DocAttachment>(input);
            var entity = input.MapTo<DocAttachment>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<DocAttachmentEditDto>();
        }

        /// <summary>
        /// 编辑DocAttachment
        /// </summary>

        protected virtual async Task Update(DocAttachmentEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除DocAttachment信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<Guid> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除DocAttachment的方法
        /// </summary>

        public async Task BatchDelete(List<Guid> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        public async Task<List<DocAttachmentListDto>> GetListByDocIdAsync(Guid? docId)
        {
            var query = _entityRepository.GetAll().Where(e => e.DocId == docId);
            return (await query.ToListAsync()).MapTo<List<DocAttachmentListDto>>();
        }

    }
}


