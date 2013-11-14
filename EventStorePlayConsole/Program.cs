using System;
using EventStore.ClientAPI;
using System.Net;
using System.Linq;
using System.Threading;

namespace EventStorePlayConsole
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var connectionSettings = ConnectionSettings.Create();
            connectionSettings.SetDefaultUserCredentials(new EventStore.ClientAPI.SystemData.UserCredentials("admin", "changeit"));
            connectionSettings.OnConnected((c, ip) => Console.WriteLine("Connected"));

            ConnectionSettings settings = connectionSettings;

            var endpoint = new IPEndPoint(IPAddress.Loopback, 1113);
            using (var connection = EventStore.ClientAPI.EventStoreConnection.Create(connectionSettings, endpoint))
            {
                connection.Connect();

                var store = new EventStoreEventSource(connection, EventStoreConcurrencyModel.IgnoreStreamVersion);

                var agg = SimpleAggregate.CreateNew();
                agg.ChangeMyName("Thomas James");
                agg.BookMeIn("Three oclock please");
                agg.BookMeIn("Actually changed my mind, 2pm please");

                if (new Random().Next(2) > 0){
                    agg.ChangeMyName("Thomas V James");
                }

                var other = new SimpleAggregate(agg.Id);
                other.Replay(store);
                other.BookMeIn("Final value");
                other.Commit(store);

                // hey sorry, the data has changed since you viewed it
                // could inform the user, show them what's different
                // could just replay and hope for the best
                // 
                //agg.Replay(store); 
                agg.Commit(store);

                System.Diagnostics.Debug.Assert(agg.SomeValue == other.SomeValue);
                System.Diagnostics.Debug.Assert(agg.MyName == other.MyName);

                // projections? need to use the RESTful api...
                var client = new RestSharp.RestClient(string.Format("http://{0}:{1}", endpoint.Address, 2113));
                client.Authenticator = new RestSharp.HttpBasicAuthenticator(settings.DefaultUserCredentials.Login, settings.DefaultUserCredentials.Password);

                var requestSingle = new RestSharp.RestRequest("projection/{id}/state", RestSharp.Method.GET);
                requestSingle.AddUrlSegment("id", "simpleAggregate");

                var responseSingle = client.Execute(requestSingle);
                Console.WriteLine(responseSingle.Content);

                var requestMultiple = new RestSharp.RestRequest("projection/{id}/state", RestSharp.Method.GET);
                requestMultiple.AddParameter("partition", EventStoreEventSource.AggregateToStream(agg), RestSharp.ParameterType.QueryString);
                requestMultiple.AddUrlSegment("id", "simpleAggregateState");

                Thread.Sleep(TimeSpan.FromSeconds(1));

                var responseMultiple = client.Execute(requestMultiple);
                Console.WriteLine(responseMultiple.Content);

                Console.WriteLine(EventStoreEventSource.AggregateToStream(agg));
            }
        }
    }
}
