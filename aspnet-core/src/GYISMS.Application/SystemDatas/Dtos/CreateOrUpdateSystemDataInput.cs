

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.SystemDatas;

namespace GYISMS.SystemDatas.Dtos
{
    public class CreateOrUpdateSystemDataInput
    {
        [Required]
        public SystemDataEditDto SystemData { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}