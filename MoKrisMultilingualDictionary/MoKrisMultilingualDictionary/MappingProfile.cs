using AutoMapper;
using Dictionary.Domain;
using Dictionary.Models.Dtos;

namespace MoKrisMultilingualDictionary
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Word, WordDto>();
            CreateMap<Tag, TagDto>();
        }
    }
}
