using System.Threading;
using System.Threading.Tasks;

namespace Windgram.EventBus
{
    public interface IEventBus
    {
        Task Publish(IntegrationEvent @event, CancellationToken cancellationToken = default);
    }
}
