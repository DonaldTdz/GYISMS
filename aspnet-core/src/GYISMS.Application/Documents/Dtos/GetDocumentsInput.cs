
using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.Documents;

namespace GYISMS.Documents.Dtos
{
    public class GetDocumentsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public int? CategoryId { get; set; }

        public string CategoryCode
        {
            get
            {
                if (CategoryId.HasValue)
                {
                    return "," + CategoryId.ToString() + ",";
                }
                return null;
            }
        }

        public string KeyWord { get; set; }

        public long DeptId { get; set; }

        /// <summary>
        /// 正常化排序使用
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime Desc";
            }
        }

    }
}
