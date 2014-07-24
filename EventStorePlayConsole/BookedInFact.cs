using System;
using EventStore.ClientAPI;
using System.Net;

namespace EventStorePlayConsole
{
    public class BookedInFact:Fact
    {
        public BookedInFact(string atValue, string someValue)
        {
            this.OccurredAt = DateTime.UtcNow;
            this.BookedInAtValue = atValue;
            this.PreviousBookingValue = someValue;
        }

        public DateTime OccurredAt
        {
            get;
            private set;
        }

        public string BookedInAtValue
        {
            get;
            private set;
        }

        public string PreviousBookingValue
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return string.Format("[BookedInEvent: Id={0}, BookedInAtValue={1}, PreviousBookingValue={2}]", Id, BookedInAtValue, PreviousBookingValue);
        }
    }
}
