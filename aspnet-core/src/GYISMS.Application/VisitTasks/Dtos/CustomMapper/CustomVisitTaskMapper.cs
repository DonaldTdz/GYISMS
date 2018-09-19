

using AutoMapper;

namespace GYISMS.VisitTasks.Dtos.CustomMapper
{

    /// <summary>
    /// 配置VisitTask的AutoMapper
    ///</summary>
    internal static class CustomerVisitTaskMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <VisitTask, VisitTaskListDto>
    ();
    configuration.CreateMap <VisitTaskEditDto, VisitTask>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
