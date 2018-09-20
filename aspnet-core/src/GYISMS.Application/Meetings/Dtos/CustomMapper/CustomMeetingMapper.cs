

using AutoMapper;

namespace GYISMS.Meetings.Dtos.CustomMapper
{

    /// <summary>
    /// 配置Meeting的AutoMapper
    ///</summary>
    internal static class CustomerMeetingMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <Meeting, MeetingListDto>
    ();
    configuration.CreateMap <MeetingEditDto, Meeting>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
