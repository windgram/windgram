using AutoMapper;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.Dto;
using System;

namespace Windgram.Blogging.ApplicationCore.MapperProfiles
{
    public class TagMapperProfile : Profile
    {
        public TagMapperProfile()
        {
            CreateMap<TagDto, Tag>(MemberList.Source)
                .ForMember(d => d.NormalizedName, o => o.MapFrom(s => s.Name.ToUpperNormalized()));
        }
    }
}
