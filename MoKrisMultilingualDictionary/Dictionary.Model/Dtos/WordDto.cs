using Dictionary.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Models.Dtos
{
    public class WordDto
    {
        public int WordId { get; set; }
        public string? Article { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? Plural { get; set; }
        public int Type { get; set; }
        public string? Conjugation { get; set; }
        public int LanguageCode { get; set; }
    }
}
