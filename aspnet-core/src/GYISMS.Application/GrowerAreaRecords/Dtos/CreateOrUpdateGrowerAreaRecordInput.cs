

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.GrowerAreaRecords;

namespace GYISMS.GrowerAreaRecords.Dtos
{
    public class CreateOrUpdateGrowerAreaRecordInput
    {
        [Required]
        public GrowerAreaRecordEditDto GrowerAreaRecord { get; set; }

    }
}