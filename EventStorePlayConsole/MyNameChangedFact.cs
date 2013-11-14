using System;
using EventStore.ClientAPI;
using System.Net;

namespace EventStorePlayConsole
{
    public class MyNameChangedFact:Fact
    {
        public MyNameChangedFact(string toName, string fromName)
        {
            this.NameChangedTo = toName;
            this.PreviousKnownName = fromName;
        }

        public string NameChangedTo
        {
            get;
            private set;
        }

        public string PreviousKnownName
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return string.Format("[MyNameChangedEvent: Id={0}, NameChangedTo={1}, PreviousKnownName={2}]", Id, NameChangedTo, PreviousKnownName);
        }
    }
}
