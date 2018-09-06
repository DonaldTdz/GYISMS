

using AutoMapper;
using GYISMS.MeetingRooms;
using GYISMS.MeetingRooms;

namespace GYISMS.MeetingRooms.Dtos.CustomMapper
{

	/// <summary>
    /// 配置MeetingRoom的AutoMapper
    ///</summary>
	internal static class CustomerMeetingRoomMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap <MeetingRoom, MeetingRoomListDto>
    ();
    configuration.CreateMap <MeetingRoomEditDto, MeetingRoom>
        ();



        //// custom codes
         
        //// custom codes end

        }
        }
        }
