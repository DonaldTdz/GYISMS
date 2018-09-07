

using AutoMapper;
using GYISMS.ScheduleDetails;
using GYISMS.ScheduleDetails;

namespace GYISMS.ScheduleDetails.Dtos.CustomMapper
{

	/// <summary>
    /// 配置ScheduleDetail的AutoMapper
    ///</summary>
	internal static class CustomerScheduleDetailMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ScheduleDetail, ScheduleDetailListDto>
    ();
    configuration.CreateMap <ScheduleDetailEditDto, ScheduleDetail>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
