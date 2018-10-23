
using AutoMapper;
using GYISMS.Documents;
using GYISMS.Documents.Dtos;

namespace GYISMS.Documents.Mapper
{

	/// <summary>
    /// 配置Document的AutoMapper
    /// </summary>
	internal static class DocumentMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Document,DocumentListDto>();
            configuration.CreateMap <DocumentListDto,Document>();

            configuration.CreateMap <DocumentEditDto,Document>();
            configuration.CreateMap <Document,DocumentEditDto>();

        }
	}
}
