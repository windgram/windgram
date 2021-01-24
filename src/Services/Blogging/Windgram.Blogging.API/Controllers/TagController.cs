using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Windgram.Blogging.ApplicationCore.Commands;
using Windgram.Blogging.ApplicationCore.Dto;
using Windgram.Blogging.ApplicationCore.Queries;
using Windgram.Blogging.ApplicationCore.ViewModels;
using Windgram.Shared.Application.Models;
using Windgram.Shared.Web.Services;

namespace Windgram.Blogging.API.Controllers
{
    public class TagController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ITagQueries _tagQueries;
        private readonly IUserContext _userContext;
        public TagController(
            IMapper mapper,
            IMediator mediator,
            ITagQueries tagQueries,
            IUserContext userContext)
        {
            _mapper = mapper;
            _mediator = mediator;
            _tagQueries = tagQueries;
            _userContext = userContext;
        }
        [HttpGet("{alias}")]
        public async Task<ActionResult<TagViewModel>> Get(string alias)
        {
            var tag = await _tagQueries.GetByAlias(alias);
            if (tag == null)
                return NotFound();
            return Ok(tag);
        }
        [HttpGet("SelectList")]
        public async Task<ActionResult<SelectItemModel>> GetSelectList(string keywords = null)
        {
            var list = await _tagQueries.GetSelectList(keywords);
            return Ok(list);
        }
        [HttpPost]
        public async Task<ActionResult<TagViewModel>> Post([FromBody] TagDto dto)
        {
            var tag = await _mediator.Send(new AddTagCommand(_userContext.UserId, dto));
            return CreatedAtAction(nameof(Get), new { alias = tag.Alias }, _mapper.Map<TagViewModel>(tag));
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<TagViewModel>> Put(int id, [FromBody] TagDto dto)
        {
            var tag = await _tagQueries.GetById(id);
            if (tag == null)
                return NotFound();
            await _mediator.Send(new UpdateTagCommand(id, dto));
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<TagViewModel>> Delete(int id)
        {
            var tag = await _tagQueries.GetById(id);
            if (tag == null)
                return NotFound();
            await _mediator.Send(new DeleteTagCommand(id));
            return NoContent();
        }
    }
}
