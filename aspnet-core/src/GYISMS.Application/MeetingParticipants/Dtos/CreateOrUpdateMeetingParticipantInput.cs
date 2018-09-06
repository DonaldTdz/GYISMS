

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.MeetingParticipants;

namespace GYISMS.MeetingParticipants.Dtos
{
    public class CreateOrUpdateMeetingParticipantInput
    {
        [Required]
        public MeetingParticipantEditDto MeetingParticipant { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}