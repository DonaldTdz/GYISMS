
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

using GYISMS.Growers.Authorization;
using GYISMS.Growers.Dtos;
using GYISMS.Growers;
using GYISMS.Authorization;

namespace GYISMS.Growers
{
    /// <summary>
    /// Grower应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize(AppPermissions.Pages)]
    public class GrowerAppService : GYISMSAppServiceBase, IGrowerAppService
    {
        private readonly IRepository<Grower, string>
        _growerRepository;


        private readonly IGrowerManager _growerManager;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public GrowerAppService(
        IRepository<Grower, string>
    growerRepository
            , IGrowerManager growerManager
            )
        {
            _growerRepository = growerRepository;
            _growerManager = growerManager;
        }


        /// <summary>
        /// 获取Grower的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GrowerListDto>> GetPagedGrowersAsync(GetGrowersInput input)
        {

            var query = _growerRepository.GetAll().Where(v => v.IsDeleted == false)
                .WhereIf(!string.IsNullOrEmpty(input.Name), u => u.Name.Contains(input.Name));
            // TODO:根据传入的参数添加过滤条件

            var growerCount = await query.CountAsync();

            var growers = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var growerListDtos = ObjectMapper.Map<List <GrowerListDto>>(growers);
            var growerListDtos = growers.MapTo<List<GrowerListDto>>();

            return new PagedResultDto<GrowerListDto>(
                        growerCount,
                        growerListDtos
                );
        }


        /// <summary>
        /// 添加或者修改Grower的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdateGrower(CreateOrUpdateGrowerInput input)
        {

            if (input.Grower.Id != null)
            {
                await UpdateGrowerAsync(input.Grower);
            }
            else
            {
                await CreateGrowerAsync(input.Grower);
            }
        }


        /// <summary>
        /// 新增Grower
        /// </summary>
        protected virtual async Task<GrowerEditDto> CreateGrowerAsync(GrowerEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            var entity = ObjectMapper.Map<Grower>(input);
            entity.IsDeleted = false;

            var id = await _growerRepository.InsertAndGetIdAsync(entity);
            return entity.MapTo<GrowerEditDto>();
        }

        /// <summary>
        /// 编辑Grower
        /// </summary>
        protected virtual async Task<GrowerEditDto> UpdateGrowerAsync(GrowerEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新      
            var entity = await _growerRepository.GetAsync(input.Id);
            input.MapTo(entity);
            // ObjectMapper.Map(input, entity);
            var result = await _growerRepository.UpdateAsync(entity);
            return result.MapTo<GrowerEditDto>();
        }



        /// <summary>
        /// 删除Grower信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(GrowerAppPermissions.Grower_Delete)]
        public async Task DeleteGrower(EntityDto<string> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _growerRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除Grower的方法
        /// </summary>
        [AbpAuthorize(GrowerAppPermissions.Grower_BatchDelete)]
        public async Task BatchDeleteGrowersAsync(List<string> input)
        {
            //TODO:批量删除前的逻辑判断，是否允许删除
            await _growerRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        /// <summary>
        /// 新增或修改烟农信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GrowerEditDto> CreateOrUpdateGrowerAsycn(GrowerEditDto input)
        {
            if (input.Id!=null)
            {
                return await UpdateGrowerAsync(input);
            }
            else
            {
                return await CreateGrowerAsync(input);
            }
        }

        /// <summary>
        /// 根据id获取烟农信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GrowerListDto> GetGrowerByIdAsync(string id)
        {
            var entity = await _growerRepository.GetAsync(id);
            return entity.MapTo<GrowerListDto>();
        }

        /// <summary>
        /// 删除烟农信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task GrowerDeleteByIdAsync(GrowerEditDto input)
        {
            var entity = await _growerRepository.GetAsync(input.Id);
            input.MapTo(entity);
            entity.IsDeleted = true;
            entity.DeletionTime = DateTime.Now;
            entity.DeleterUserId = AbpSession.UserId;
            await _growerRepository.UpdateAsync(entity);
        }
    }
}


