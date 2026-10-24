using HeathenEngineering.SteamworksIntegration;
using Mirror;
using UnityEngine;

namespace SephiriaEnhancements.CrashRecovery
{
    internal static class NativeRecoveryLobby
    {
        internal static void Capture(DiskCheckpoint metadata)
        {
            GameObject steam = SingletonObject.Find("SteamManager");
            if (steam == null || !steam.TryGetComponent<LobbyManager>(out var manager) || !manager.HasLobby)
            {
                metadata.Multiplayer = NetworkServer.connections.Count > 1 ||
                    SaveManager.CurrentRun.GetInt("SavedPlayerCount", 0) > 1;
                metadata.LobbyCapacity = 4;
                metadata.LobbyChapter = "";
                return;
            }
            metadata.Multiplayer = true;
            metadata.LobbyCapacity = manager.Lobby.MaxMembers;
            metadata.LobbyChapter = manager.Lobby["Chapter"];
        }

    }
}
