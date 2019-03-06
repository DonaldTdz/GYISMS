
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

namespace GYISMS.Advises
{
    /// <summary>
    /// Advise应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class AdviseAppService : GYISMSAppServiceBase, IAdviseAppService
    {
        private readonly IRepository<Advise, Guid> _entityRepository;

        private readonly IAdviseManager _entityManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public AdviseAppService(
        IRepository<Advise, Guid> entityRepository
        ,IAdviseManager entityManager
        )
        {
            _entityRepository = entityRepository; 
             _entityManager=entityManager;
        }


        /// <summary>
        /// 获取Advise的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[AbpAuthorize(AdvisePermissions.Query)] 
        public async Task<PagedResultDto<AdviseListDto>> GetPaged(GetAdvisesInput input)
		{

		    var query = _entityRepository.GetAll();
			// TODO:根据传入的参数添加过滤条件
            

			var count = await query.CountAsync();

			var entityList = await query
					.OrderBy(input.Sorting).AsNoTracking()
					.PageBy(input)
					.ToListAsync();

			// var entityListDtos = ObjectMapper.Map<List<AdviseListDto>>(entityList);
			var entityListDtos =entityList.MapTo<List<AdviseListDto>>();

			return new PagedResultDto<AdviseListDto>(count,entityListDtos);
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
		[AbpAuthorize(AdvisePermissions.Create,AdvisePermissions.Edit)]
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
		[AbpAuthorize(AdvisePermissions.Create,AdvisePermissions.Edit)]
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
            var entity=input.MapTo<Advise>();
			

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
            entity = await _entityRepository.InsertAsync(entity);
            return new APIResultDto() { Code = 0, Msg = "ok" };
        }
    }
}


