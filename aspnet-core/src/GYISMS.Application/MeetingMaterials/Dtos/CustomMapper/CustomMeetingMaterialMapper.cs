

using AutoMapper;

namespace GYISMS.MeetingMaterials.Dtos.CustomMapper
{

    /// <summary>
    /// 配置MeetingMaterial的AutoMapper
    ///</summary>
    internal static class CustomerMeetingMaterialMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <MeetingMaterial, MeetingMaterialListDto>
    ();
    configuration.CreateMap <MeetingMaterialEditDto, MeetingMaterial>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
