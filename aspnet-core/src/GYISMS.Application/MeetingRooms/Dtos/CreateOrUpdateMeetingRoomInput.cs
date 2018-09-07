

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.MeetingRooms;

namespace GYISMS.MeetingRooms.Dtos
{
    public class CreateOrUpdateMeetingRoomInput
    {
        [Required]
        public MeetingRoomEditDto MeetingRoom { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}