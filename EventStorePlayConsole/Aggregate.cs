using System;
using EventStore.ClientAPI;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace EventStorePlayConsole
{
    public abstract class Aggregate
    {
        private readonly List<Fact> EventsThatHaveBeenApplied = new List<Fact>();
        private readonly List<Fact> EventsThatHaveBeenReceived = new List<Fact>();

        protected Aggregate(Guid id)
        {
            this.Id = id;
            this.Version = -1;
            this.Revision = Guid.Empty;

            Console.WriteLine("Aggregate [{0}]: Created", this.Id);
        }

        public Guid Id { get; private set; }

        public int Version { get; private set; }

        public Guid Revision { get; private set; }

        protected void Record(Fact fact)
        {
            Console.WriteLine("Aggregate [{0}]: Event Store    -> {1}", this.Id, fact);

            this.EventsThatHaveBeenReceived.Add(fact);

            this.Dispatch(fact);
        }

        public void Commit(IEventSource source)
        {
            source.SaveEventsFor(this, this.Version, this.EventsThatHaveBeenReceived);
            this.Replay(source);
        }

        public void Replay(IEventSource source)
        {
            source.LoadEventsFor(this, this.Version, events => events.ToList().ForEach(x => this.Rebase(x)));
        }

        private void Rebase(dynamic @event)
        {
            Console.WriteLine("Aggregate [{0}]: Event Rebase   -> {1}", this.Id, @event);

            this.Version++;
            this.Revision = @event.Id;

            this.Dispatch(@event);
        }

        private void Dispatch(dynamic @event)
        {
            Console.WriteLine("Aggregate [{0}]: Event Dispatch -> {1}", this.Id, @event);

            this.EventsThatHaveBeenApplied.Add(@event);

            dynamic @this = this;
            @this.Apply(@event);
            Console.WriteLine("Aggregate [{0}]: {1}", this.Id, this);
        }

        public override string ToString()
        {
            return string.Format("[Aggregate: Id={0}, Version={1}, Revision={2}]", Id, Version, Revision);
        }
    }
}
