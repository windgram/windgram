using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.Commands;
using Windgram.Blogging.ApplicationCore.Domain.Entities;
using Windgram.Blogging.ApplicationCore.Queries;
using Windgram.Caching;
using Windgram.Shared.Domain;
using Windgram.Shared.Domain.Exceptions;

namespace Windgram.Blogging.ApplicationCore.CommandHandling
{
    public class TagCommandHandler :
         IRequestHandler<AddTagCommand, Tag>,
         IRequestHandler<GetOrAddTagCommand, Tag>,
         IRequestHandler<UpdateTagCommand, Tag>,
         IRequestHandler<DeleteTagCommand, Unit>
    {
        private readonly IMapper _mapper;
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Tag> _repository;
        private readonly ITagQueries _tagQueries;
        public TagCommandHandler(
            IMapper mapper,
            ICacheManager cacheManager,
            IRepository<Tag> repository,
            ITagQueries tagQueries)
        {
            _mapper = mapper;
            _cacheManager = cacheManager;
            _repository = repository;
            _tagQueries = tagQueries;
        }
        public async Task<Tag> Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            var tag = _mapper.Map<Tag>(request.Tag);
            tag.CreatedBy = request.UserId;
            tag.CreatedDateTime = DateTime.Now;
            _repository.Add(tag);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            return tag;
        }

        public async Task<Tag> Handle(GetOrAddTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _tagQueries.GetByName(request.Name);
            if (tag == null)
            {
                return await Handle(new AddTagCommand(request.UserId, new Dto.TagDto
                {
                    Alias = Guid.NewGuid().ToString("N"),
                    Name = request.Name,
                }), cancellationToken);
            }
            return tag;
        }

        public async Task<Tag> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdAsync(request.Id);
            if (tag == null)
                throw new DomainException($"The tag {request.Id} was not found.");
            var oldName = tag.NormalizedName;
            var oldAlias = tag.Alias;
            _mapper.Map(request.Tag, tag);
            tag.LastModifiedDateTime = DateTime.Now;
            _repository.Update(tag);
            await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await _cacheManager.RemoveAsync(Tag.GetByAliasCacheKey(oldAlias));
            await _cacheManager.RemoveAsync(Tag.GetByNameCacheKey(oldName));
            return tag;
        }

        public async Task<Unit> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdAsync(request.Id);
            if (tag != null)
            {
                _repository.Delete(tag);
                await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);
            }
            await _cacheManager.RemoveAsync(Tag.GetByAliasCacheKey(tag.Alias));
            await _cacheManager.RemoveAsync(Tag.GetByNameCacheKey(tag.NormalizedName));
            return Unit.Value;
        }
    }
}
