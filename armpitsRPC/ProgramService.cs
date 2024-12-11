using System;
using System.Threading;
using ArmpitsRPC.Managers;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace ArmpitsRPC
{
    internal class ProgramService : IHostedService
    {
        public Task StartAsync(CancellationToken _)
        {
            AppsManager.ListenAsync(OnError, OnProcessNotFound, OnProcessFound);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken _)
        {
            Dispose();

            return Task.CompletedTask;
        }

        private static void Dispose()
        {
            AppsManager.Dispose();

            Logger.Info("Apps listener disposed.");

            if (DiscordManager.IsInitialized())
            {
                DiscordManager.Dispose();

                Logger.Info("Discord RPC Client disposed.");
            }
        }

        private static void OnError(Exception e)
        {
            Logger.Error(e);

            Environment.Exit(1);
        }

        private static void OnProcessNotFound()
        {
            if (DiscordManager.IsInitialized())
            {
                DiscordManager.Dispose();

                Logger.Info("No apps running. Current Discord RPC Client disposed.");
            }
        }

        private static void OnProcessFound()
        {
            if (DiscordManager.IsInitialized())
            {
                DiscordManager.Dispose();

                Logger.Info("New app found. Discord RPC Client will be reinstantiated.");
            }

            DiscordManager.Init();
        }
    }
}
