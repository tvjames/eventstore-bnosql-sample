using System;
using EventStore.ClientAPI;
using System.Net;

namespace EventStorePlayConsole
{
    public abstract class Fact
    {
        protected Fact()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id
        {
            get;
            private set;
        }
    }
    
}
