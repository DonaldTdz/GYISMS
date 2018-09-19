

using AutoMapper;

namespace GYISMS.Growers.Dtos.CustomMapper
{

    /// <summary>
    /// 配置Grower的AutoMapper
    ///</summary>
    internal static class CustomerGrowerMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Grower, GrowerListDto>
    ();
    configuration.CreateMap <GrowerEditDto, Grower>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
