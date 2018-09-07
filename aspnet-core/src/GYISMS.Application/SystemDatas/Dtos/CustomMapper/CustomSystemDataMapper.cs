

using AutoMapper;
using GYISMS.SystemDatas;
using GYISMS.SystemDatas;

namespace GYISMS.SystemDatas.Dtos.CustomMapper
{

	/// <summary>
    /// 配置SystemData的AutoMapper
    ///</summary>
	internal static class CustomerSystemDataMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <SystemData, SystemDataListDto>
    ();
    configuration.CreateMap <SystemDataEditDto, SystemData>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
