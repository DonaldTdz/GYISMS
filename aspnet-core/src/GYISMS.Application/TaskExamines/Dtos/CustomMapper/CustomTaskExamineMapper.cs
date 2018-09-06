

using AutoMapper;
using GYISMS.TaskExamines;
using GYISMS.TaskExamines;

namespace GYISMS.TaskExamines.Dtos.CustomMapper
{

	/// <summary>
    /// 配置TaskExamine的AutoMapper
    ///</summary>
	internal static class CustomerTaskExamineMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <TaskExamine, TaskExamineListDto>
    ();
    configuration.CreateMap <TaskExamineEditDto, TaskExamine>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
