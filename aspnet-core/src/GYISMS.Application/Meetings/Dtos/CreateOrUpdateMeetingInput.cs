

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.Meetings;

namespace GYISMS.Meetings.Dtos
{
    public class CreateOrUpdateMeetingInput
    {
        [Required]
        public MeetingEditDto Meeting { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}