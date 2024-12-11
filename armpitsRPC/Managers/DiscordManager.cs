using System;
using DiscordRPC;

namespace ArmpitsRPC.Managers
{
    internal class DiscordManager
    {
        // RPC
        private static DiscordRpcClient? _client;

        // States
        private static bool _isWaiting = false;
        private static bool _isInitialized = false;

        public static void Init()
        {
            try
            {
                // Initialize client
                _client = new DiscordRpcClient(AppsManager.GetDiscordApplicationId());
                _client.Initialize();

                // Register events
                _client.OnReady += (sender, e) => OnClientReady();
                _client.OnClose += (sender, e) => OnClientNotReady();
                _client.OnConnectionFailed += (sender, e) => OnClientNotReady();
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        private static void OnClientReady()
        {
            if (!_isInitialized)
            {
                Logger.Info("Discord RPC Client is listening.");

                // Not waiting
                _isWaiting = false;

                // Allow activity updates
                _isInitialized = true;

                // Update current activity
                UpdateActivity();
            }
        }

        private static void OnClientNotReady()
        {
            // Dispose current client
            Dispose();

            // Waiting message
            if (!_isWaiting)
            {
                _isWaiting = true;

                Logger.Info("Waiting for Discord...");
            }

            // Init again
            Init();
        }

        private static void UpdateActivity()
        {
            if (_isInitialized)
            {
                // Update presence
                _client!.SetPresence(AppsManager.GetPresence());
            }
        }

        public static bool IsInitialized()
        {
            return _isInitialized;
        }

        public static void Dispose()
        {
            if (_client != null)
            {
                _client.ClearPresence();
                _client.Dispose();
                _isInitialized = false;
            }
        }
    }
}
