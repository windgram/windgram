using System;

namespace Windgram.EventBus
{
    public abstract class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDateTime = DateTime.Now;
        }
        public IntegrationEvent(Guid id, DateTime creationDateTime)
        {
            Id = id;
            CreationDateTime = creationDateTime;
        }
        public Guid Id { get; private set; }
        public DateTime CreationDateTime { get; private set; }
    }
}
