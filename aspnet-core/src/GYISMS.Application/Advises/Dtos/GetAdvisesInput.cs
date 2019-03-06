
using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.Advises;

namespace GYISMS.Advises.Dtos
{
    public class GetAdvisesInput : PagedSortedAndFilteredInputDto, IShouldNormalize
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
