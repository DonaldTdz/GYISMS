

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.Organizations;

namespace GYISMS.Organizations.Dtos
{
    public class CreateOrUpdateOrganizationInput
    {
        [Required]
        public OrganizationEditDto Organization { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}