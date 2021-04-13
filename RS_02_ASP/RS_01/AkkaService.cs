using System;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using AkkaConfigProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared;

namespace RS_01
{
    public class AkkaService : IHostedService
    {
        private IConfiguration _configuration;
        private ImmutableHashSet<ActorPath> _initialContacts;
        public static ActorSystem ActorSys { get; private set; }
        public static ClusterClientSettings CClientSettings { get; set; }

        public AkkaService(IConfiguration configuration)
        {
            _configuration = configuration;
            _initialContacts = ImmutableHashSet<ActorPath>.Empty
                .Add(ActorPath.Parse("akka.tcp://Cluster@localhost:12000/system/receptionist"))
                .Add(ActorPath.Parse("akka.tcp://Cluster@localhost:12001/system/receptionist"));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var provider = new ConfigProvider();
            var config = provider.GetAkkaConfig<AkkaConfig>();

            ActorSys = ActorSystem.Create("webapi", config);

            CClientSettings = ClusterClientSettings.Create(ActorSys).WithInitialContacts(_initialContacts);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return ActorSys.Terminate()
                .ContinueWith(_ => Console.WriteLine("Actor system has terminated!"), cancellationToken);
        }
    }
}