
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


using GYISMS.DocCategories;
using GYISMS.DocCategories.Dtos;
using GYISMS.DocCategories.DomainService;
using GYISMS.Documents.Dtos;
using GYISMS.Documents;
using GYISMS.Employees;
using GYISMS.Dtos;
using Abp.Domain.Uow;

namespace GYISMS.DocCategories
{
    /// <summary>
    /// DocCategory应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DocCategoryAppService : GYISMSAppServiceBase, IDocCategoryAppService
    {
        private readonly IRepository<DocCategory, int> _entityRepository;
        private readonly IRepository<Document, Guid> _documentRepository;

        private readonly IDocCategoryManager _entityManager;
        private readonly IRepository<Employee, string> _employeeRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DocCategoryAppService(
        IRepository<DocCategory, int> entityRepository
        , IDocCategoryManager entityManager
            , IRepository<Document, Guid> documentRepository
            , IRepository<Employee, string> employeeRepository

        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _documentRepository = documentRepository;
            _employeeRepository = employeeRepository;
        }


        /// <summary>
        /// 获取DocCategory的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DocCategoryListDto>> GetPaged(GetDocCategorysInput input)
        {

            var query = _entityRepository.GetAll();
            // TODO:根据传入的参数添加过滤条件


            var count = await query.CountAsync();

            var entityList = await query
                    .OrderBy(input.Sorting).AsNoTracking()
                    .PageBy(input)
                    .ToListAsync();

            // var entityListDtos = ObjectMapper.Map<List<DocCategoryListDto>>(entityList);
            var entityListDtos = entityList.MapTo<List<DocCategoryListDto>>();

            return new PagedResultDto<DocCategoryListDto>(count, entityListDtos);
        }


        /// <summary>
        /// 通过指定id获取DocCategoryListDto信息
        /// </summary>

        public async Task<DocCategoryListDto> GetById(EntityDto<int> input)
        {
            var entity = await _entityRepository.GetAsync(input.Id);

            return entity.MapTo<DocCategoryListDto>();
        }

        /// <summary>
        /// 获取编辑 DocCategory
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<GetDocCategoryForEditOutput> GetForEdit(NullableIdDto<int> input)
        {
            var output = new GetDocCategoryForEditOutput();
            DocCategoryEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _entityRepository.GetAsync(input.Id.Value);

                editDto = entity.MapTo<DocCategoryEditDto>();

                //docCategoryEditDto = ObjectMapper.Map<List<docCategoryEditDto>>(entity);
            }
            else
            {
                editDto = new DocCategoryEditDto();
            }

            output.DocCategory = editDto;
            return output;
        }


        /// <summary>
        /// 添加或者修改DocCategory的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task CreateOrUpdate(CreateOrUpdateDocCategoryInput input)
        {
            input.DocCategory.ParentId = input.DocCategory.ParentId ?? 0;
            if (input.DocCategory.Id != 0 && input.DocCategory.Id != null)
            {
                await Update(input.DocCategory);
            }
            else
            {
                await Create(input.DocCategory);
            }
        }


        /// <summary>
        /// 新增DocCategory
        /// </summary>

        protected virtual async Task<DocCategoryEditDto> Create(DocCategoryEditDto input)
        {
            //TODO:新增前的逻辑判断，是否允许新增

            // var entity = ObjectMapper.Map <DocCategory>(input);
            var entity = input.MapTo<DocCategory>();


            entity = await _entityRepository.InsertAsync(entity);
            return entity.MapTo<DocCategoryEditDto>();
        }

        /// <summary>
        /// 编辑DocCategory
        /// </summary>

        protected virtual async Task Update(DocCategoryEditDto input)
        {
            //TODO:更新前的逻辑判断，是否允许更新

            var entity = await _entityRepository.GetAsync(input.Id.Value);
            input.MapTo(entity);

            // ObjectMapper.Map(input, entity);
            await _entityRepository.UpdateAsync(entity);
        }



        /// <summary>
        /// 删除DocCategory信息的方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task Delete(EntityDto<int> input)
        {
            //TODO:删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(input.Id);
        }



        /// <summary>
        /// 批量删除DocCategory的方法
        /// </summary>

        public async Task BatchDelete(List<int> input)
        {
            // TODO:批量删除前的逻辑判断，是否允许删除
            await _entityRepository.DeleteAsync(s => input.Contains(s.Id));
        }

        private List<CategoryTreeNode> GetTrees(int pid, List<DocCategory> categoryList)
        {
            var catQuery = categoryList.Where(c => c.ParentId == pid)
                            .Select(c => new CategoryTreeNode()
                            {
                                key = c.Id.ToString(),
                                title = c.Name,
                                ParentId = c.ParentId,
                                children = GetTrees(c.Id, categoryList)
                            });
            return catQuery.ToList();
        }

        public async Task<List<CategoryTreeNode>> GetTreeAsync(long? deptId)
        {
            if (!deptId.HasValue)
            {
                return new List<CategoryTreeNode>();
            }
            var categoryList = await _entityRepository.GetAll().WhereIf(deptId.HasValue, e => e.DeptId == deptId).ToListAsync();
            return GetTrees(0, categoryList);
        }

        /// <summary>
        /// 钉钉获取知识库类别
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<GridListDto>> GetCategoryListAsync(string host, string userId)
        {
            var user = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();
            var departmentId = user.Department.Replace('[', ' ').Replace(']', ' ').Trim();
            var query = _entityRepository.GetAll().Where(v => v.ParentId == 0).OrderBy(v => v.Id).AsNoTracking();
            var entityList = from c in query
                             select new GridListDto()
                             {
                                 Id = c.Id,
                                 Text = c.Name,
                                 Icon = host + "knowledge/homePageNew.png"
                             };
            List<GridListDto> list = new List<GridListDto>();
            foreach (var item in entityList)
            {
                int count = await _documentRepository.GetAll().Where(v => ("," + v.CategoryCode + ",").Contains("," + item.Id.ToString() + ",") && (v.IsAllUser == true || v.DeptIds.Contains(departmentId) || v.EmployeeIds.Contains(userId) || v.DocRoleIds.Contains(user.DocRole))).AsNoTracking().CountAsync();
                if (count > 0)
                {
                    list.Add(item);
                }
            }
            return list;
        }


        /// <summary>
        /// 钉钉获取Tab子列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<List<TabListDto>> GetTabChildListByIdAsync(int id)
        {
            var query = _entityRepository.GetAll().Where(v => v.ParentId == id);
            List<TabListDto> list = new List<TabListDto>();
            TabListDto item = new TabListDto();
            item.Id = id;
            item.ParentId = 0;
            item.Title = "全部";
            list.Add(item);
            var result = await (from t in query
                                select new TabListDto()
                                {
                                    Id = t.Id,
                                    ParentId = t.ParentId,
                                    Title = t.Name
                                }).ToListAsync();
            list.AddRange(result);
            return list;
        }


        /// <summary>
        /// 递归获取父级信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private void GetCurrentName(int id, ref string result)
        {
            var entity = _entityRepository.GetAll().Where(v => v.Id == id).AsNoTracking().FirstOrDefault();
            result = $"{entity.Name} / " + result;
            if (entity.ParentId.Value != 0)
            {
                GetCurrentName(entity.ParentId.Value, ref result);
            }
        }

        /// <summary>
        /// 获取层级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> GetParentName(int id)
        {
            string result = "";
            var doc = await _entityRepository.GetAsync(id);
            result = doc.Name;
            if (doc.ParentId != 0)
            {
                GetCurrentName(doc.ParentId.Value, ref result);
            }
            return result;
        }

        /// <summary>
        /// 删除分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResultDto> CategoryRemoveById(EntityDto<int> id)
        {
            int childCount = await _entityRepository.GetAll().Where(v => v.ParentId == id.Id).CountAsync();
            int docCount = await _documentRepository.GetAll().Where(v => v.CategoryId == id.Id).CountAsync();
            if (childCount != 0)
            {
                return new APIResultDto() { Code = 1, Msg = "存在子分类" };
            }
            else if (docCount != 0)
            {
                return new APIResultDto() { Code = 2, Msg = "存在文档" };
            }
            else
            {
                await _entityRepository.DeleteAsync(id.Id);
                return new APIResultDto() { Code = 0, Msg = "删除成功" };
            }
        }

        /// <summary>
        /// 复制分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        //public async Task<APIResultDto> CopyCategoryByDeptIdAsync(CopyInput input)
        //{
        //    long deptId = long.Parse(input.DeptId);
        //    var rootCategory = await _entityRepository.GetAll().Where(v => v.Id == input.CategoryId).AsNoTracking().FirstOrDefaultAsync();
        //    DocCategory entity = new DocCategory();
        //    entity.Name = rootCategory.Name;
        //    entity.DeptId = deptId;
        //    entity.Desc = rootCategory.Desc;
        //    entity.ParentId = Convert.ToInt32(input.ParentId);
        //    //var result = await _entityRepository.InsertAsync(entity);
        //    var resultId = await _entityRepository.InsertAndGetIdAsync(entity);
        //    await CurrentUnitOfWork.SaveChangesAsync();
        //    await GetCopyChild(rootCategory.Id, resultId, deptId);
        //    return new APIResultDto() { Code = 0, Msg = "操作成功" };
        //}

        /// <summary>
        /// 递归子分类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="insertList"></param>
        /// <returns></returns>
        //private async Task GetCopyChild(int id, int parentId, long deptId)
        //{
        //    var list = await _entityRepository.GetAll().Where(v => v.ParentId == id).AsNoTracking().ToListAsync();
        //    if (list.Count() > 0)
        //    {
        //        foreach (var item in list)
        //        {
        //            DocCategory entity = new DocCategory();
        //            entity.Name = item.Name;
        //            entity.DeptId = deptId;
        //            entity.Desc = item.Desc;
        //            entity.ParentId = parentId;
        //            //var curEntity = await _entityRepository.InsertAsync(entity);
        //            var curEntityId = await _entityRepository.InsertAndGetIdAsync(entity);
        //            await CurrentUnitOfWork.SaveChangesAsync();
        //            await GetCopyChild(item.Id, curEntityId, deptId);
        //        }
        //    }
        //}

        public async Task<APIResultDto> CopyCategoryByDeptIdAsync(CopyInput input)
        {
            long deptId = long.Parse(input.DeptId);
            var tempList = new List<DocCategory>();
            var rootCategory = await _entityRepository.GetAll().Where(v => v.Id == input.CategoryId).AsNoTracking().FirstOrDefaultAsync();
            tempList.Add(rootCategory);
            GetCopyChild(rootCategory.Id, tempList);
            DocCategory entity = new DocCategory();
            entity.Name = rootCategory.Name;
            entity.DeptId = deptId;
            entity.Desc = rootCategory.Desc;
            entity.ParentId = Convert.ToInt32(input.ParentId);
            var resultId = _entityRepository.InsertAndGetId(entity);
            CurrentUnitOfWork.SaveChanges();
            CopyChild(rootCategory.Id, resultId, deptId, tempList);
            return new APIResultDto() { Code = 0, Msg = "操作成功" };
        }
        private void GetCopyChild(int id, List<DocCategory> docList)
        {
            var list = _entityRepository.GetAll().Where(v => v.ParentId == id).AsNoTracking().ToList();
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    docList.Add(item);
                    GetCopyChild(item.Id, docList);
                }
            }
        }


        private void CopyChild(int id, int parentId,long deptId, List<DocCategory> tempList)
        {
            var list = tempList.Where(v => v.ParentId == id).ToList();
            if (list.Count() > 0)
            {
                foreach (var item in list)
                {
                    DocCategory entity = new DocCategory();
                    entity.Name = item.Name;
                    entity.DeptId = deptId;
                    entity.Desc = item.Desc;
                    entity.ParentId = parentId;
                    var curEntityId =  _entityRepository.InsertAndGetId(entity);
                    CurrentUnitOfWork.SaveChanges();
                    CopyChild(item.Id, curEntityId, deptId, tempList);
                }
            }
        }

        /// <summary>
        /// 分类复制树
        /// </summary>
        /// <param name="deptId"></param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task<List<CategoryTreeNode>> GetCopyTreeWithRootAsync(long? deptId)
        {
            List<CategoryTreeNode> list = new List<CategoryTreeNode>();
            CategoryTreeNode item = new CategoryTreeNode();
            item.title = "根目录";
            item.key = "0";
            item.isLeaf = false;
            item.expanded = true;
            item.children = new List<CategoryTreeNode>();
            var categoryList = await _entityRepository.GetAll().WhereIf(deptId.HasValue, e => e.DeptId == deptId).ToListAsync();
            var child = GetTrees(0, categoryList);
            item.children.AddRange(child);
            list.Add(item);
            return list;
        }
    }
}