using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.Commands;
using Windgram.Blogging.ApplicationCore.Dto;
using Windgram.Blogging.ApplicationCore.Queries;
using Windgram.Blogging.ApplicationCore.ViewModels;
using Windgram.Shared.Web.Services;

namespace Windgram.Blogging.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUserContext _userContext;
        private readonly IPostQueries _postQueries;
        public PostsController(
            IMapper mapper,
            IMediator mediator,
            IUserContext userContext,
            IPostQueries postQueries)
        {
            _mapper = mapper;
            _mediator = mediator;
            _userContext = userContext;
            _postQueries = postQueries;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PostViewModel>> Get(int id)
        {
            var model = await _postQueries.GetById(id);
            if (model == null)
                return NotFound();
            return Ok(model);
        }
        [HttpPost]
        public async Task<ActionResult<PostViewModel>> Post([FromBody] PostDto dto)
        {
            var post = await _mediator.Send(new AddPostCommand(_userContext.UserId, dto));
            return CreatedAtAction(nameof(Get), new { id = post.Id }, _mapper.Map<PostViewModel>(post));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<PostViewModel>> Put(int id, [FromBody] PostDto dto)
        {
            var post = await _postQueries.GetById(id);
            if (post == null)
                return NotFound();
            if (post.CreatedBy != _userContext.UserId)
                return Forbid();
            await _mediator.Send(new UpdatePostCommand(id, dto));
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<PostViewModel>> Delete(int id)
        {
            var post = await _postQueries.GetById(id);
            if (post == null)
                return NotFound();
            if (post.CreatedBy != _userContext.UserId)
                return Forbid();
            await _mediator.Send(new DeletePostCommand(id));
            return NoContent();
        }
    }
}
