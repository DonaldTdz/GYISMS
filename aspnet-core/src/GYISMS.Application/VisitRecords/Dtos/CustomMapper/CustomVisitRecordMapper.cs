

using AutoMapper;
using GYISMS.VisitRecords;
using GYISMS.VisitRecords;

namespace GYISMS.VisitRecords.Dtos.CustomMapper
{

	/// <summary>
    /// 配置VisitRecord的AutoMapper
    ///</summary>
	internal static class CustomerVisitRecordMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <VisitRecord, VisitRecordListDto>
    ();
    configuration.CreateMap <VisitRecordEditDto, VisitRecord>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
