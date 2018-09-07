

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.VisitRecords;

namespace GYISMS.VisitRecords.Dtos
{
    public class CreateOrUpdateVisitRecordInput
    {
        [Required]
        public VisitRecordEditDto VisitRecord { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}