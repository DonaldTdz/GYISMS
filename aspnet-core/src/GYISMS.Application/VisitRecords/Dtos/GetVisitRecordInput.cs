

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.VisitRecords;

namespace GYISMS.VisitRecords.Dtos
{
    public class GetVisitRecordsInput : PagedAndSortedInputDto, IShouldNormalize
    {
          /// <summary>
		 /// 模糊搜索使用的关键字
		 ///</summary>
        public string Filter { get; set; }

        public int GrowerId { get; set; }
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
