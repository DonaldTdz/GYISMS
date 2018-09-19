

using AutoMapper;

namespace GYISMS.Schedules.Dtos.CustomMapper
{

    /// <summary>
    /// 配置Schedule的AutoMapper
    ///</summary>
    internal static class CustomerScheduleMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Schedule, ScheduleListDto>
    ();
    configuration.CreateMap <ScheduleEditDto, Schedule>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
