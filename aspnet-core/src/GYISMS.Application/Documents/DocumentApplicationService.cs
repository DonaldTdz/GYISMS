
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


using GYISMS.Documents.Dtos;
using GYISMS.Documents.DomainService;
using GYISMS.Dtos;
using GYISMS.DocAttachments;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using GYISMS.Helpers;
using GYISMS.DocCategories.DomainService;
using GYISMS.Employees;
using System.Text.RegularExpressions;
using GYISMS.DocDingTalks;
using GYISMS.Employees.Dtos;
using GYISMS.Organizations;

namespace GYISMS.Documents
{
    /// <summary>
    /// Document应用层服务的接口实现方法  
    ///</summary>
    [AbpAuthorize]
    public class DocumentAppService : GYISMSAppServiceBase, IDocumentAppService
    {
        private readonly IRepository<Document, Guid> _entityRepository;
        private readonly IRepository<DocAttachment, Guid> _docAttachmentRepository;
        private readonly IRepository<DocDingTalk, Guid> _docDocDingTalkRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDocumentManager _entityManager;
        private readonly IDocCategoryManager _docCategoryManager;
        private readonly IRepository<Employee, string> _employeeRepository;
        private readonly IRepository<Organization, long> _organizationRepository;

        /// <summary>
        /// 构造函数 
        ///</summary>
        public DocumentAppService(
        IRepository<Document, Guid> entityRepository
        , IDocumentManager entityManager
            , IRepository<DocAttachment, Guid> docAttachmentRepository
        , IHostingEnvironment hostingEnvironment
        , IDocCategoryManager docCategoryManager
        , IRepository<Employee, string> employeeRepository
        , IRepository<DocDingTalk, Guid> docDocDingTalkRepository
        , IRepository<Organization, long> organizationRepository
        )
        {
            _entityRepository = entityRepository;
            _entityManager = entityManager;
            _docAttachmentRepository = docAttachmentRepository;
            _hostingEnvironment = hostingEnvironment;
            _docCategoryManager = docCategoryManager;
            _employeeRepository = employeeRepository;
            _docDocDingTalkRepository = docDocDingTalkRepository;
            _organizationRepository = organizationRepository;
        }


        /// <summary>
        /// 获取Document的分页列表信息
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<PagedResultDto<DocumentListDto>> GetPaged(GetDocumentsInput input)
        {

            var query = _entityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.CategoryCode), e => ("," + e.CategoryCode + ",").Contains(input.CategoryCode))
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
            var categoryList = await _docCategoryManager.GetHierarchyCategories(input.CategoryId);
            entity.CategoryCode = string.Join(',', categoryList.Select(c => c.Id).ToArray());
            entity.CategoryDesc = string.Join(',', categoryList.Select(c => c.Name).ToArray());
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

        public Task DownloadZipFileTest()
        {
            return Task.Run(() =>
            {
                ZipHelper.ZipFileDirectory(@"F:\zipfiles", @"F:\zipfiles.zip");
            });
        }

        public async Task<APIResultDto> DownloadQRCodeZip(GetDocumentsInput input)
        {
            var query = _entityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.CategoryCode), e => ("," + e.CategoryCode + ",").Contains(input.CategoryCode))
                .WhereIf(!string.IsNullOrEmpty(input.KeyWord), e => e.Name.Contains(input.KeyWord) || e.Summary.Contains(input.KeyWord));
            var docs = await query.Select(q => new { q.Id, q.Name, q.CategoryDesc }).ToListAsync();

            string webRootPath = _hostingEnvironment.WebRootPath;
            string filePath = webRootPath + "/docqrcodes";
            if (Directory.Exists(filePath))
            {
                Directory.Delete(filePath, true);
                Directory.CreateDirectory(filePath);
            }
            else
            {
                Directory.CreateDirectory(filePath);
            }
            foreach (var item in docs)
            {
                QRCodeHelper.GenerateQRCode(item.Id.ToString(), string.Format("{0}/{1}-{2}.jpg", filePath, item.CategoryDesc.Replace(',', '-'), item.Name), QRCoder.QRCodeGenerator.ECCLevel.Q, 20);
            }
            var zipFiles = "/downloads/资料二维码.zip";
            var zipPath = webRootPath + "/downloads";
            if (!Directory.Exists(zipPath))
            {
                Directory.CreateDirectory(zipPath);
            }
            ZipHelper.ZipFileDirectory(filePath, string.Format("{0}{1}", webRootPath, zipFiles));
            return new APIResultDto() { Code = 0, Msg = "生成二维码成功", Data = zipFiles };
        }

        /// <summary>
        /// 钉钉资料详情&扫码搜索
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<DocumentListDto> GetDocInfoByScanAsync(Guid id, string host, string uid)
        {
            var doc = await _entityRepository.GetAll().Where(v => v.Id == id).AsNoTracking().FirstOrDefaultAsync();
            var result = doc.MapTo<DocumentListDto>();
            if (result.CategoryDesc.Contains(','))
            {
                result.CategoryDesc = result.CategoryDesc.Replace(",", " > ");
            }

            var query = from a in _docAttachmentRepository.GetAll().Where(v => v.DocId == id)
                        join d in _docDocDingTalkRepository.GetAll().Where(d => d.UserId == uid) on a.Id equals d.DocAttId into temp
                        from ld in temp.DefaultIfEmpty()
                        select new GridDocListDto(host)
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Path = a.Path,
                            SpaceId = ld == null ? "" : ld.SpaceId,
                            FileId = ld == null ? "" : ld.FileId,
                            FileName = ld == null ? "" : ld.FileName,
                            FileSize = ld == null ? 0 : ld.FileSize,
                            FileType = ld == null ? "" : ld.FileType
                        };
            //var gg = query.ToList();
            var gridList = await query.ToListAsync();

            result.FileList = gridList;
            return result;
        }

        /// <summary>
        /// 判断用户是否拥有权限
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<bool> GetHasDocPermissionFromScanAsync(Guid id, string userId)
        {
            var user = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();
            if (!string.IsNullOrEmpty(user.Department) && user.Department.Contains('['))
            {
                var departmentId = user.Department.Replace('[', ' ').Replace(']', ' ').Trim();
                int count = await _entityRepository.GetAll().Where(v => v.Id == id && (v.IsAllUser == true || v.DeptIds.Contains(departmentId) || v.EmployeeIds.Contains(userId))).AsNoTracking().CountAsync();
                if (count != 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取文件列表项
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<List<DocumentListDto>> GetDocListByParentIdAsync(string categoryCode, string userId, int pageIndex, int pageSize)
        {
            var user = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();
            var departmentId = user.Department.Replace('[', ' ').Replace(']', ' ').Trim();
            var query = _entityRepository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(categoryCode), e => ("," + e.CategoryCode + ",").Contains("," + categoryCode + ","));
            var list = await (from d in query
                              select new DocumentListDto()
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  Summary = d.Summary.Length > 20 ? d.Summary.Substring(0, 20) + "..." : d.Summary,
                                  ReleaseDate = d.ReleaseDate,
                                  IsAllUser = d.IsAllUser,
                                  DeptIds = d.DeptIds,
                                  EmployeeIds = d.EmployeeIds
                              }).Where(v => v.IsAllUser == true || v.DeptIds.Contains(departmentId) || v.EmployeeIds.Contains(userId))
                              .OrderBy(v => v.Id).AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return list;
        }


        /// <summary>
        /// 搜索标题和摘要
        /// </summary>
        [AbpAllowAnonymous]
        public async Task<List<DocumentListDto>> GetDocListByInputAsync(string input, string catId, string userId, int pageIndex, int pageSize)
        {
            var user = await _employeeRepository.GetAll().Where(v => v.Id == userId).FirstOrDefaultAsync();
            var departmentId = user.Department.Replace('[', ' ').Replace(']', ' ').Trim();
            var query = _entityRepository.GetAll();
            var list = await (from d in query
                              select new DocumentListDto()
                              {
                                  Id = d.Id,
                                  Name = d.Name,
                                  Summary = d.Summary.Length > 20 ? d.Summary.Substring(0, 20) + "..." : d.Summary,
                                  ReleaseDate = d.ReleaseDate,
                                  CategoryCode = d.CategoryCode,
                                  IsAllUser = d.IsAllUser,
                                  DeptIds = d.DeptIds,
                                  EmployeeIds = d.EmployeeIds
                              }).Where(v => v.IsAllUser == true || v.DeptIds.Contains(departmentId) || v.EmployeeIds.Contains(userId))
                              .WhereIf(!string.IsNullOrEmpty(catId), v => ("," + v.CategoryCode + ",").Contains("," + catId + ","))
                              .WhereIf(!string.IsNullOrEmpty(input), v => v.Name.Contains(input) || v.Summary.Contains(input))
                              .OrderBy(v => v.Id).AsNoTracking().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return list;
        }

        [AbpAllowAnonymous]
        public async Task SaveDocDingTalkAsync(DocDingTalkInput input)
        {
            var dingDoc = _docDocDingTalkRepository.GetAll().Where(d => d.UserId == input.UserId && d.DocAttId == input.DocAttId).FirstOrDefault();
            if (dingDoc == null)//新增
            {
                var addEnitiy = input.MapTo<DocDingTalk>();
                await _docDocDingTalkRepository.InsertAsync(addEnitiy);
            }
            else
            {
                input.MapTo(dingDoc);
                await _docDocDingTalkRepository.UpdateAsync(dingDoc);
            }
        }

        #region 文档发布部门树

        /// <summary>
        /// 构建子部门树
        /// </summary>
        private List<DocNzTreeNode> getDeptChildTree(long pid, List<Organization> depts)
        {
            var trees = depts.Where(d => d.ParentId == pid).Select(d => new DocNzTreeNode()
            {
                key = d.Id.ToString(),
                title = d.DepartmentName,
                children = getDeptChildTree(d.Id, depts)
            });

            return trees.ToList();
        }

        /// <summary>
        /// 构建部门树
        /// </summary>
        private async Task<List<DocNzTreeNode>> getDeptTreeAsync(long[] deptids)
        {
            var trees = new List<DocNzTreeNode>();
            var depts = await _organizationRepository.GetAll().AsNoTracking().ToListAsync();
            foreach (var id in deptids)
            {
                if (id == 1)//顶级市
                {
                    trees.AddRange(getDeptChildTree(id, depts));
                }
                else
                {
                    var dept = depts.Where(d => d.Id == id).First();
                    trees.Add(new DocNzTreeNode()
                    {
                        key = dept.Id.ToString(),
                        title = dept.DepartmentName,
                        children = getDeptChildTree(id, depts)
                    });
                }
            }
            return trees;
        }

        public async Task<List<DocNzTreeNode>> GetDeptDocNzTreeNodesAsync()
        {
            var docDeptList = new List<DocNzTreeNode>();
            var root = new DocNzTreeNode()
            {
                key = "0",
                title = "资料维护部门"
            };
            
            //当前用户角色
            var roles = await GetUserRolesAsync();
            //如果包含市级管理员 和 系统管理员 全部架构
            if (roles.Contains(RoleCodes.Admin) || roles.Contains(RoleCodes.CityAdmin))
            {
                root.children = await getDeptTreeAsync(new long[] { 1 });//顶级部门
            }
            else if (roles.Contains(RoleCodes.EnterpriseAdmin))//本部门架构
            {
                var user = await GetCurrentUserAsync();
                if (!string.IsNullOrEmpty(user.EmployeeId))
                {
                    var employee = await _employeeRepository.GetAsync(user.EmployeeId);
                    var depts = employee.Department.Substring(1, employee.Department.Length - 2).Split("][");//多部门拆分
                    root.children = await getDeptTreeAsync(Array.ConvertAll(depts, d => long.Parse(d)));
                }
            }
            if (root.children.Count == 0)
            {
                root.children.Add(new DocNzTreeNode()
                {
                    key = "-1",
                    title = "没有任何部门权限"
                });
            }
            else
            {
                root.children[0].selected = true;
            }
            docDeptList.Add(root);
            return docDeptList;
        }

        #endregion
    }
}


