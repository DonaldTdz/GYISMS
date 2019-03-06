
using AutoMapper;
using GYISMS.Advises;
using GYISMS.Advises.Dtos;

namespace GYISMS.Advises.Mapper
{

	/// <summary>
    /// 配置Advise的AutoMapper
    /// </summary>
	internal static class AdviseMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Advise,AdviseListDto>();
            configuration.CreateMap <AdviseListDto,Advise>();

            configuration.CreateMap <AdviseEditDto,Advise>();
            configuration.CreateMap <Advise,AdviseEditDto>();

        }
	}
}
