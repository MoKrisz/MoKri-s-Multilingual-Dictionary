using AutoMapper;
using Dictionary.Models.Dtos;
using Dictionary.Models;

namespace MoKrisMultilingualDictionary
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Word, WordDto>();
        }
    }
}
