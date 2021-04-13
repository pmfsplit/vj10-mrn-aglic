using System;
using Akka.Actor;
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

            var akkaConfig = config.WithFallback($"akka.remote.dot-netty.tcp.port={port}");

            using (var system = ActorSystem.Create("backend", akkaConfig))
            {
                
            }
        }
    }
}