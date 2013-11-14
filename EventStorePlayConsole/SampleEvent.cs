using System;
using EventStore.ClientAPI;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace EventStorePlayConsole
{
    public class SampleEvent
    {
        public Guid Id { get; set; }

        public string SomeEventValue { get; set; }
    }
}
