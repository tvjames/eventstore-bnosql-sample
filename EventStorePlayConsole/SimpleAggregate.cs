using System;
using EventStore.ClientAPI;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace EventStorePlayConsole
{
    public class SimpleAggregate:Aggregate
    {
        public static SimpleAggregate CreateNew()
        {
            return new SimpleAggregate(Guid.NewGuid());
        }

        public SimpleAggregate(Guid id):base(id)
        {

        }

        public string MyName { get; private set; }

        public string SomeValue { get; private set; }

        public void ChangeMyName(string toName)
        {
            var @event = new MyNameChangedFact(toName, this.MyName);

            this.Record(@event);
        }

        public void BookMeIn(string atValue)
        {
            var @event = new BookedInFact(atValue, this.SomeValue);

            this.Record(@event);
        }

        internal void Apply(MyNameChangedFact @event)
        {
            this.MyName = @event.NameChangedTo;
        }

        internal void Apply(BookedInFact @event)
        {
            this.SomeValue = @event.BookedInAtValue;
        }

        public override string ToString()
        {
            return string.Format("[SimpleAggregate: MyName={0}, SomeValue={1}]", MyName, SomeValue);
        }
    }
}
