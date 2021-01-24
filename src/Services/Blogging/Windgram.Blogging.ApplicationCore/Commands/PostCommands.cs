using MediatR;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.Dto;

namespace Windgram.Blogging.ApplicationCore.Commands
{
    public class AddPostCommand : IRequest<Post>
    {
        public string UserId { get; set; }
        public PostDto Post { get; set; }
        public AddPostCommand(string userId, PostDto dto)
        {
            UserId = userId;
            Post = dto;
        }
    }
    public class UpdatePostCommand : IRequest<Post>
    {
        public int Id { get; set; }
        public PostDto Post { get; set; }
        public UpdatePostCommand(int id, PostDto dto)
        {
            Id = id;
            Post = dto;
        }
    }
    public class DeletePostCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public DeletePostCommand(int id)
        {
            Id = id;
        }
    }
}
