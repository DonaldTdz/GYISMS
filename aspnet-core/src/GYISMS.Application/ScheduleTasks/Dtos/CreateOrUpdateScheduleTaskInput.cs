

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.ScheduleTasks;

namespace GYISMS.ScheduleTasks.Dtos
{
    public class CreateOrUpdateScheduleTaskInput
    {
        [Required]
        public ScheduleTaskEditDto ScheduleTask { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}