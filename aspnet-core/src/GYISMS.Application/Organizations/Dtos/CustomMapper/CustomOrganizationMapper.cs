

using AutoMapper;

namespace GYISMS.Organizations.Dtos.CustomMapper
{

    /// <summary>
    /// 配置Organization的AutoMapper
    ///</summary>
    internal static class CustomerOrganizationMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Organization, OrganizationListDto>
    ();
    configuration.CreateMap <OrganizationEditDto, Organization>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
