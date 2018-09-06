

using AutoMapper;
using GYISMS.ScheduleTasks;
using GYISMS.ScheduleTasks;

namespace GYISMS.ScheduleTasks.Dtos.CustomMapper
{

	/// <summary>
    /// 配置ScheduleTask的AutoMapper
    ///</summary>
	internal static class CustomerScheduleTaskMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <ScheduleTask, ScheduleTaskListDto>
    ();
    configuration.CreateMap <ScheduleTaskEditDto, ScheduleTask>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
