using System;
using EventStore.ClientAPI;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace EventStorePlayConsole
{
    public class SimpleInMemoryEventSource : IEventSource
    {
        private readonly Dictionary<Guid, List<Fact>> state = new Dictionary<Guid, List<Fact>>();

        public void SaveEventsFor(Aggregate aggregate, int version, System.Collections.Generic.IEnumerable<Fact> eventsThatHaveBeenReceived)
        {
            Guid id = aggregate.Id;
            if (!this.state.ContainsKey(id))
            {
                this.state[id] = new List<Fact>();
            }
            this.state[id].AddRange(eventsThatHaveBeenReceived);
        }

        public void LoadEventsFor(Aggregate aggregate, int version, Action<System.Collections.Generic.IEnumerable<Fact>> apply)
        {
            Guid id = aggregate.Id;
            if (!this.state.ContainsKey(id))
            {
                return;
            }
            var list = this.state[id];

            var result = list.SelectMany((o, i) => i > version ? new [] { o } : new Fact[0]);
            apply(result.ToArray());
        }
    }
}
