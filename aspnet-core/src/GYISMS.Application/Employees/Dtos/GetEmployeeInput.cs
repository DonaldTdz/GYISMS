

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.Employees;

namespace GYISMS.Employees.Dtos
{
    public class GetEmployeesInput : PagedAndSortedInputDto, IShouldNormalize
    {
          /// <summary>
		 /// 模糊搜索使用的关键字
		 ///</summary>
        public string Name { get; set; }
        public string DepartId { get; set; }
        public string Mobile { get; set; }

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
