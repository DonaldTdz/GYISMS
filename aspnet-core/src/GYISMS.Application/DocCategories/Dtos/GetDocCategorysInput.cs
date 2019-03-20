
using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.DocCategories;

namespace GYISMS.DocCategories.Dtos
{
    public class GetDocCategorysInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }

    }

    /// <summary>
    /// 分类复制Input
    /// </summary>
    public class CopyInput
    {
        public string DeptId { get; set; } 
        public int CategoryId { get; set; }
        public string ParentId { get; set; }

    }
}
