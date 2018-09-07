

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.Schedules;

namespace GYISMS.Schedules.Dtos
{
    public class CreateOrUpdateScheduleInput
    {
        [Required]
        public ScheduleEditDto Schedule { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}