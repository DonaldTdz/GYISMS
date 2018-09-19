

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.TaskExamines.Dtos;
using GYISMS.VisitTasks;

namespace GYISMS.VisitTasks.Dtos
{
    public class CreateOrUpdateVisitTaskInput
    {
        //[Required]
        public VisitTaskEditDto VisitTask { get; set; }

        //public List<TaskExamineEditDto> TaskExamineList { get; set; }
    }
}