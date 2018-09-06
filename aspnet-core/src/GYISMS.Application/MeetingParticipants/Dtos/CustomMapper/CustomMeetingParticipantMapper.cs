

using AutoMapper;
using GYISMS.MeetingParticipants;
using GYISMS.MeetingParticipants;

namespace GYISMS.MeetingParticipants.Dtos.CustomMapper
{

	/// <summary>
    /// 配置MeetingParticipant的AutoMapper
    ///</summary>
	internal static class CustomerMeetingParticipantMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <MeetingParticipant, MeetingParticipantListDto>
    ();
    configuration.CreateMap <MeetingParticipantEditDto, MeetingParticipant>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
