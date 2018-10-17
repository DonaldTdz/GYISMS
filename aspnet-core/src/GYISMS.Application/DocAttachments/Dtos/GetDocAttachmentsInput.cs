
using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.DocAttachments;

namespace GYISMS.DocAttachments.Dtos
{
    public class GetDocAttachmentsInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
}
