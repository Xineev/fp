using Autofac;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.DI;
using TagCloudGenerator.Infrastructure.Filters;

namespace TagCloudConsoleClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule<TagCloudModule>();
            builder.RegisterType<BoringWordsFilter>().As<IFilter>();
            builder.RegisterType<ConsoleClient>().As<IClient>();

            var container = builder.Build();

            using var scope = container.BeginLifetimeScope();
            var client = scope.Resolve<IClient>();
            client.Run(args);
        }
    }
}