using System;
using EventStore.ClientAPI;
using System.Net;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace EventStorePlayConsole
{
    public enum EventStoreConcurrencyModel
    {
        DefaultOptimisticConcurrency,
        IgnoreStreamVersion
    }

    public class EventStoreEventSource : IEventSource
    {
        private readonly IEventStoreConnection connection;
        private readonly EventStoreConcurrencyModel concurrencyModel;

        public EventStoreEventSource(IEventStoreConnection connection, EventStoreConcurrencyModel concurrencyModel)
        {
            this.connection = connection;
            this.concurrencyModel = concurrencyModel;
        }

        public void SaveEventsFor(Aggregate aggregate, int version, System.Collections.Generic.IEnumerable<Fact> eventsThatHaveBeenReceived)
        {
            var data = from e in eventsThatHaveBeenReceived
                let type = e.GetType()
                let json = Newtonsoft.Json.JsonConvert.SerializeObject(e)
                    let bytes = System.Text.Encoding.UTF8.GetBytes(json)
                    let meta = "{ $correlationId: '"+aggregate.Id+"', nativeTypeOfEvent: '"+type.FullName+"', nativeTypeOfAggregate: '"+aggregate.GetType().FullName+"'}"
                    select new EventData(e.Id, type.Name, true, bytes, System.Text.Encoding.UTF8.GetBytes(meta));

            // log an event
            var stream = AggregateToStream(aggregate);
            connection.AppendToStream(
                stream, 
                this.concurrencyModel == EventStoreConcurrencyModel.IgnoreStreamVersion ? ExpectedVersion.Any : version, 
                data.ToArray());
        }

        public void LoadEventsFor(Aggregate aggregate, int version, Action<System.Collections.Generic.IEnumerable<Fact>> apply)
        {
            // read out the events oldest to newest
            var stream = AggregateToStream(aggregate);

            var startPosition = version < 0 ? 0 : version;
            var pageSize = 10;
            var eventsToRead = true;
            while (eventsToRead)
            {
                var slice = connection.ReadStreamEventsForward(stream, startPosition, pageSize, false);

                var events = from e in slice.Events
                    let objectJson = System.Text.Encoding.UTF8.GetString(e.Event.Data)
                    let metaJson = System.Text.Encoding.UTF8.GetString(e.Event.Metadata)
                    let metadata = JObject.Parse(metaJson)
                    let type = Type.GetType((string)metadata["nativeTypeOfEvent"])
                    select (Fact)Newtonsoft.Json.JsonConvert.DeserializeObject(objectJson, type);

                apply(events.ToArray());


                eventsToRead = !slice.IsEndOfStream;
            }
        }

        public static string AggregateToStream(Aggregate aggregate)
        {
            var category = aggregate.GetType().Name;
            category = category.Substring(0, 1).ToLowerInvariant() + category.Substring(1);
            return string.Format("{0}-{1:N}", category, aggregate.Id);
        }
    }
}
