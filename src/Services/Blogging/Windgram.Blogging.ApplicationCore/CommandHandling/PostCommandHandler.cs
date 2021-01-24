using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.Commands;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.Enums;
using Windgram.Caching;
using Windgram.Shared.Domain;
using Windgram.Shared.Domain.Exceptions;

namespace Windgram.Blogging.ApplicationCore.CommandHandling
{
    class PostCommandHandler :
        IRequestHandler<AddPostCommand, Post>,
        IRequestHandler<UpdatePostCommand, Post>,
        IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Post> _repository;
        public PostCommandHandler(
            IMapper mapper,
            IMediator mediator,
            ICacheManager cacheManager,
            IRepository<Post> repository)
        {
            _mapper = mapper;
            _mediator = mediator;
            _cacheManager = cacheManager;
            _repository = repository;
        }
        public async Task<Post> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Post>(request.Post); 
            entity.CreatedBy = request.UserId;
            entity.CreatedDateTime = DateTime.Now;
            if (entity.PostStatus == PostStatusType.Published)
            {
                entity.PublishedDateTime = entity.CreatedDateTime;
            }
            if (request.Post.Tags.Any())
            {
                await SavePostTags(entity, request.Post.Tags);
            }
            _repository.Add(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return entity;
        }

        public async Task<Post> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new DomainException($"The post was not found.");
            var entry = _repository.Entry(entity);
            await entry.Reference(x => x.PostContent).LoadAsync(cancellationToken);
            await entry.Collection(x => x.PostTags).LoadAsync(cancellationToken);
            _mapper.Map(request.Post, entity);
            entity.LastModifiedDateTime = DateTime.Now;
            if (entity.PostStatus == PostStatusType.Published && !entity.PublishedDateTime.HasValue)
            {
                entity.PublishedDateTime = entity.LastModifiedDateTime;
            }
            if (request.Post.Tags != null && request.Post.Tags.Any())
            {
                await SavePostTags(entity, request.Post.Tags);
            }
            else if (entity.PostTags.Any())
            {
                entity.PostTags.Clear();
            }
            _repository.Update(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                throw new DomainException($"The post was not found.");
            _repository.Delete(entity);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await _cacheManager.RemoveAsync(Post.GetByIdCacheKey(request.Id));
            return Unit.Value;
        }
        private async Task SavePostTags(Post post, List<string> tags)
        {
            foreach (var tagName in tags)
            {
                var tag = await _mediator.Send(new GetOrAddTagCommand(post.CreatedBy, tagName));
                if (!post.PostTags.Any(x => x.TagId == tag.Id))
                {
                    post.PostTags.Add(new PostTag
                    {
                        TagId = tag.Id
                    });
                }
            }
        }
    }
}
