using System.ComponentModel.DataAnnotations;

namespace GYISMS.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}