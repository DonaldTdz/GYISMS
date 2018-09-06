

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.TaskExamines;

namespace GYISMS.TaskExamines.Dtos
{
    public class CreateOrUpdateTaskExamineInput
    {
        [Required]
        public TaskExamineEditDto TaskExamine { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}