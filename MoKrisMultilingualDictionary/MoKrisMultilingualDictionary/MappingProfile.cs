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
            CreateMap<TranslationGroup, TranslationGroupDto>()
                .ForMember(dest => dest.Tags,
                           opt => opt.MapFrom(src => src.TranslationGroupTags.Select(tgt => tgt.Tag)));
        }
    }
}
