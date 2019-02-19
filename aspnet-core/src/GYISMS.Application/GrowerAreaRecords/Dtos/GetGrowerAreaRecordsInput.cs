
using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.GrowerAreaRecords;

namespace GYISMS.GrowerAreaRecords.Dtos
{
    public class GetGrowerAreaRecordsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
    {
        public int GrowerId { get; set; }
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

    public class GetDingDingAreaRecordsInput
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
}
