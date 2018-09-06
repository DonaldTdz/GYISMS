

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.MeetingMaterials;

namespace GYISMS.MeetingMaterials.Dtos
{
    public class CreateOrUpdateMeetingMaterialInput
    {
        [Required]
        public MeetingMaterialEditDto MeetingMaterial { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}