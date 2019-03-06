

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.Advises;

namespace GYISMS.Advises.Dtos
{
    public class CreateOrUpdateAdviseInput
    {
        [Required]
        public AdviseEditDto Advise { get; set; }

    }
}