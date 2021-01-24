using AutoMapper;
using System.Linq;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.Dto;
using Windgram.Blogging.ApplicationCore.ViewModels;

namespace Windgram.Blogging.ApplicationCore.MapperProfiles
{
    public class PostMapperProfile : Profile
    {
        public PostMapperProfile()
        {
            CreateMap<PostDto, Post>(MemberList.Source)
                .ForMember(d => d.PostTags, o => o.Ignore());

            CreateMap<PostContentDto, PostContent>(MemberList.Source);

            CreateMap<Post, PostViewModel>(MemberList.Destination)
                .ForMember(d => d.Tags, o => o.MapFrom(s => s.PostTags.Select(t => t.Tag == null ? null : t.Tag.Name)))
                .ForMember(d => d.MetaDescription, o => o.MapFrom(s => s.PostContent == null ? null : s.PostContent.MetaDescription))
                .ForMember(d => d.MetaKeyword, o => o.MapFrom(s => s.PostContent == null ? null : s.PostContent.MetaKeyword))
                .ForMember(d => d.HtmlContent, o => o.MapFrom(s => s.PostContent == null ? null : s.PostContent.HtmlContent));
        }
    }
}
