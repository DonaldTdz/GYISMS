

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.VisitExamines;

namespace GYISMS.VisitExamines.Dtos
{
    public class CreateOrUpdateVisitExamineInput
    {
        [Required]
        public VisitExamineEditDto VisitExamine { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}