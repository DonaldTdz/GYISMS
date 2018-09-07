

using AutoMapper;
using GYISMS.VisitExamines;
using GYISMS.VisitExamines;

namespace GYISMS.VisitExamines.Dtos.CustomMapper
{

	/// <summary>
    /// 配置VisitExamine的AutoMapper
    ///</summary>
	internal static class CustomerVisitExamineMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <VisitExamine, VisitExamineListDto>
    ();
    configuration.CreateMap <VisitExamineEditDto, VisitExamine>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
