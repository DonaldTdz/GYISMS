

using Abp.Runtime.Validation;
using GYISMS.Dtos;
using GYISMS.GYEnums;
using GYISMS.SystemDatas;

namespace GYISMS.SystemDatas.Dtos
{
    public class GetSystemDatasInput : PagedAndSortedInputDto, IShouldNormalize
    {
          /// <summary>
		 /// 模糊搜索使用的关键字
		 ///</summary>
        public string Filter { get; set; }

        /// <summary>
        /// 所属模块
        /// </summary>
        public ConfigModel? ModelId { get; set; }

        /// <summary>
        /// 配置类型
        /// </summary>
        public ConfigType Type { get; set; }
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
