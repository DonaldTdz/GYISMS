

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.VisitTasks;

namespace GYISMS.VisitTasks.Dtos
{
    public class CreateOrUpdateVisitTaskInput
    {
        [Required]
        public VisitTaskEditDto VisitTask { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}