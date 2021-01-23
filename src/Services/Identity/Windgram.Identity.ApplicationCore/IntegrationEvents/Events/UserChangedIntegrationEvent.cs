using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windgram.EventBus;

namespace Windgram.Identity.ApplicationCore.IntegrationEvents.Events
{
    public class UserChangedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }
        public UserChangedIntegrationEvent(string id)
        {
            UserId = UserId;
        }
    }
}
