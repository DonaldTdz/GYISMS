

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.Documents;

namespace GYISMS.Documents.Dtos
{
    public class CreateOrUpdateDocumentInput
    {
        [Required]
        public DocumentEditDto Document { get; set; }

    }
}