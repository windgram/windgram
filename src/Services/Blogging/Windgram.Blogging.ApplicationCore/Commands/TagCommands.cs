using MediatR;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.Dto;

namespace Windgram.Blogging.ApplicationCore.Commands
{
    public class AddTagCommand : IRequest<Tag>
    {
        public string UserId { get; set; }
        public TagDto Tag { get; set; }
        public AddTagCommand(string userId, TagDto tag)
        {
            UserId = userId;
            Tag = tag;
        }
    }
    public class GetOrAddTagCommand : IRequest<Tag>
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public GetOrAddTagCommand(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }
    }
    public class UpdateTagCommand : IRequest<Tag>
    {
        public int Id { get; set; }
        public TagDto Tag { get; set; }
        public UpdateTagCommand(int id, TagDto tag)
        {
            Id = id;
            Tag = tag;
        }
    }
    public class DeleteTagCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DeleteTagCommand(int id)
        {
            Id = id;
        }
    }
}
