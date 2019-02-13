
using AutoMapper;
using GYISMS.GrowerAreaRecords;
using GYISMS.GrowerAreaRecords.Dtos;

namespace GYISMS.GrowerAreaRecords.Mapper
{

	/// <summary>
    /// 配置GrowerAreaRecord的AutoMapper
    /// </summary>
	internal static class GrowerAreaRecordMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <GrowerAreaRecord,GrowerAreaRecordListDto>();
            configuration.CreateMap <GrowerAreaRecordListDto,GrowerAreaRecord>();

            configuration.CreateMap <GrowerAreaRecordEditDto,GrowerAreaRecord>();
            configuration.CreateMap <GrowerAreaRecord,GrowerAreaRecordEditDto>();

        }
	}
}
