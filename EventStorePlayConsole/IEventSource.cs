using System;
using EventStore.ClientAPI;
using System.Net;
using System.Collections.Generic;

namespace EventStorePlayConsole
{
	public interface IEventSource
	{
        void SaveEventsFor(Aggregate aggregate, int version, IEnumerable<Fact> eventsThatHaveBeenReceived);

        void LoadEventsFor(Aggregate aggregate, int version, Action<IEnumerable<Fact>> apply);
	}
}
