using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Shared;

namespace RS_01.Actors
{
    public class ConnectionActor : ReceiveActor
    {
        private IActorRef _clusterClient;

        public ConnectionActor(ClusterClientSettings clusterClientSettings)
        {
            var props = ClusterClient.Props(clusterClientSettings);
            _clusterClient = Context.ActorOf(props);

            Receive<Get>(msg => HandleGet(msg));
            Receive<GetAll>(msg => HandleGetAll(msg));
            Receive<GetAllResult>(res =>
            {
                Sender.Tell(res.JArray);
                Self.Tell(PoisonPill.Instance);
            });
            
            Receive<GetResult>(res =>
            {
                Sender.Tell(res.Json);
                Self.Tell(PoisonPill.Instance);
            });
        }

        private void HandleGetAll(GetAll msg)
        {
            // Ne možemo koristit Tell jer moramo očekivat poruku
            // _clusterClient.Tell(new ClusterClient.Send("/user/manager", msg));
            _clusterClient.Ask<GetAllResult>(new ClusterClient.Send("/user/manager", msg))
                .PipeTo(Self, Sender);
        }

        private void HandleGet(Get msg)
        {
            _clusterClient.Ask<GetResult>(new ClusterClient.Send("/user/manager", msg))
                .PipeTo(Self, Sender);
        }
    }
}