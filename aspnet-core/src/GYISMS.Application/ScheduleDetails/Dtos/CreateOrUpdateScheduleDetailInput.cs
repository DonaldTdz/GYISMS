

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.ScheduleDetails;

namespace GYISMS.ScheduleDetails.Dtos
{
    public class CreateOrUpdateScheduleDetailInput
    {
        [Required]
        public ScheduleDetailEditDto ScheduleDetail { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}