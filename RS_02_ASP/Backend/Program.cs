using System;
using Akka.Actor;
using Akka.Cluster.Tools.Client;
using Akka.Configuration;
using AkkaConfigProvider;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            var configProvider = new ConfigProvider();
            var config = configProvider.GetAkkaConfig<AkkaConfig>();

            var port = args.Length > 0 ? int.Parse(args[0]) : 0;
            Console.WriteLine(port);

            var akkaConfig = ConfigurationFactory.ParseString($"akka.remote.dot-netty.tcp.port={port}")
                .WithFallback(config);
            // var akkaConfig = config.WithFallback($"akka.remote.dot-netty.tcp.port={port}");

            using (var system = ActorSystem.Create("Cluster", akkaConfig))
            {
                var props = Props.Create(() => new ManagerActor());
                var managerActor = system.ActorOf(props, "manager");
                
                var receptionist = ClusterClientReceptionist.Get(system);
                receptionist.RegisterService(managerActor);

                Console.ReadLine();
                CoordinatedShutdown.Get(system).Run(CoordinatedShutdown.ActorSystemTerminateReason.Instance)
                    .Wait();
            }
        }
    }
}