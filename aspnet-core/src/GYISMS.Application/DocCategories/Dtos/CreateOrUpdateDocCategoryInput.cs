

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GYISMS.DocCategories;

namespace GYISMS.DocCategories.Dtos
{
    public class CreateOrUpdateDocCategoryInput
    {
        [Required]
        public DocCategoryEditDto DocCategory { get; set; }

    }
}