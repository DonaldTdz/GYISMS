

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.Organizations;

namespace GYISMS.Organizations.Dtos
{
    public class GetOrganizationsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 模糊搜索使用的关键字
        ///</summary>
        public string Name { get; set; }


        //// custom codes

        //// custom codes end

        /// <summary>
        /// 正常化排序使用
        ///</summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Id";
            }
        }


    }
}
