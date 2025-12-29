using Autofac;
using TagCloudGenerator.Core.Interfaces;
using TagCloudGenerator.DI;

namespace TagCloudUIClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var builder = new ContainerBuilder();
            builder.RegisterModule<TagCloudModule>();
            builder.RegisterType<WinFormsClient>().As<IClient>().SingleInstance();
            var container = builder.Build();

            using var scope = container.BeginLifetimeScope();
            var client = scope.Resolve<IClient>();
            client.Run(Array.Empty<string>());
        }
    }
}