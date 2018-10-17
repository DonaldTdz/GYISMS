

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.DocAttachments;

namespace GYISMS.DocAttachments.Dtos
{
    public class CreateOrUpdateDocAttachmentInput
    {
        [Required]
        public DocAttachmentEditDto DocAttachment { get; set; }

    }
}