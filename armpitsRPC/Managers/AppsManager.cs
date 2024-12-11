using System;
using System.IO;
using DiscordRPC;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using ArmpitsRPC.Models;
using System.Diagnostics;
using System.Collections.Generic;

namespace ArmpitsRPC.Managers
{
    internal class AppsManager
    {
        private static AppsModel? _app;
        private static Process? _process;
        private static readonly CancellationTokenSource _listenerCts = new();

        public static async void ListenAsync(Action<Exception> OnError, Action OnProcessNotFound, Action OnProcessFound)
        {
            try
            {
                var apps = JsonSerializer.Deserialize<List<AppsModel>>(File.ReadAllText("apps.json", Encoding.UTF8));

                if (apps != null)
                {
                    Logger.Info($"Loaded apps list.");

                    var processNames = apps.Select(o => o.ProcessName.ToLowerInvariant());

                    Logger.Info($"Listening for apps...");

                    var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

                    while (!_listenerCts.IsCancellationRequested)
                    {
                        if (_process == null || _process.HasExited)
                        {
                            // Check new process
                            _process = Process.GetProcesses().FirstOrDefault(process => processNames
                                .Contains(process.ProcessName.ToLowerInvariant())
                            );

                            if (_process == null)
                            {
                                OnProcessNotFound();
                            }
                            else
                            {
                                // New app
                                _app = apps.FirstOrDefault(app => app.ProcessName
                                    .ToLowerInvariant()
                                    .Equals(_process.ProcessName.ToLowerInvariant())
                                );

                                if (_app != null)
                                {
                                    Logger.Info($"Current app is '{_app.Name}'.");
                                    Logger.Info($"DiscordApplicationId is {_app.DiscordApplicationId}.");

                                    OnProcessFound();
                                }
                            }
                        }

                        await timer.WaitForNextTickAsync(_listenerCts.Token);
                    }
                }
            }
            catch (Exception e)
            {
                if (e is not OperationCanceledException)
                {
                    OnError(e);
                }
            }
        }

        public static string GetDiscordApplicationId()
        {
            return _app!.DiscordApplicationId;
        }

        public static RichPresence GetPresence()
        {
            return new RichPresence()
            {
                Details = _app?.Details,
                State = _app?.State,
                Assets = new Assets()
                {
                    LargeImageKey = _app?.LargeImage,
                    LargeImageText = _app?.LargeText ?? $"{BuildInfo.Name} {BuildInfo.Version}",
                    SmallImageKey = _app?.SmallImage,
                    SmallImageText = _app?.SmallText,
                },
                Timestamps = new Timestamps()
                {
                    Start = _process?.StartTime.ToUniversalTime()
                }
            };
        }

        public static void Dispose()
        {
            _listenerCts.Cancel();
            _listenerCts.Dispose();
        }
    }
}
