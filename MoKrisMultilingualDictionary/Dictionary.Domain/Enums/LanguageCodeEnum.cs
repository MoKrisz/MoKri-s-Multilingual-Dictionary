using System.ComponentModel.DataAnnotations;

namespace Dictionary.Domain.Enums
{
    public enum LanguageCodeEnum
    {
        None,
        [Display(Name = "English")]
        EN,
        [Display(Name = "German")]
        DE,
        [Display(Name = "Hungarian")]
        HU
    }
}
