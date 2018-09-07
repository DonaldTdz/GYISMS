

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.Employees;

namespace GYISMS.Employees.Dtos
{
    public class CreateOrUpdateEmployeeInput
    {
        [Required]
        public EmployeeEditDto Employee { get; set; }



		//// custom codes
 
        //// custom codes end
    }
}