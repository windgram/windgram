using MassTransit;
using MediatR;
using System.Threading.Tasks;
using Windgram.Identity.ApplicationCore.IntegrationEvents.Events;

namespace Windgram.Blogging.ApplicationCore.IntegrationEvents.EventHandling
{
    public class UserCreatedIntegrationEventComsumer : IConsumer<UserCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        public UserCreatedIntegrationEventComsumer(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task Consume(ConsumeContext<UserCreatedIntegrationEvent> context)
        {
            //await _mediator.Send(new AddBlogCommand(context.Message.UserId, new Dto.BlogDto
            //{
            //    Alias = Guid.NewGuid().ToString("n"),
            //    Name = context.Message.UserId
            //}));
            await Task.FromResult(0);
        }
    }
}
