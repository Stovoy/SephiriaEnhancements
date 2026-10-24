using System;
using System.Collections;
using HeathenEngineering.SteamworksIntegration;
using Mirror;
using SephiriaEnhancements.Configuration;
using SephiriaEnhancements.Diagnostics;
using SephiriaEnhancements.Integration;
using Steamworks;
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

        internal static IEnumerator Restore(DiskCheckpoint metadata)
        {
            float deadline = Time.realtimeSinceStartup + 30f;
            while (Time.realtimeSinceStartup < deadline && NetworkServer.active && !Arrived(metadata.AreaGuid))
                yield return null;
            if (!NetworkServer.active) yield break;
            if (!Arrived(metadata.AreaGuid))
            {
                SupportLogger.Record("checkpoint_arrival_timed_out", level: "ERROR");
                Announce(CrashRecoveryLocalization.Failed);
                yield break;
            }
            if (!metadata.Multiplayer)
            {
                Announce(CrashRecoveryLocalization.Restored);
                yield break;
            }

            LobbyManager manager = null;
            bool requested = false;
            try
            {
                GameObject steam = SingletonObject.Find("SteamManager");
                if (steam == null || !steam.TryGetComponent(out manager))
                    throw new InvalidOperationException("Steam lobby manager is unavailable.");
                if (!manager.HasLobby)
                    manager.Create("Recovered Sephiria run", ELobbyType.k_ELobbyTypePrivate,
                        Math.Max(2, Math.Min(4, metadata.LobbyCapacity)));
                requested = true;
            }
            catch (Exception exception) { SupportLogger.Failure("checkpoint_lobby_create_failed", exception); }
            if (!requested)
            {
                Announce(CrashRecoveryLocalization.LobbyFailed);
                yield break;
            }
            deadline = Time.realtimeSinceStartup + 10f;
            while (NetworkServer.active && !manager.HasLobby && Time.realtimeSinceStartup < deadline)
                yield return null;
            if (!NetworkServer.active) yield break;
            try
            {
                if (!manager.HasLobby || !manager.Lobby.IsOwner)
                    throw new InvalidOperationException("The recovered Steam lobby was not created.");
                LobbyData lobby = manager.Lobby;
                lobby.GameVersion = Application.version;
                lobby["Chapter"] = metadata.LobbyChapter;
                lobby["pw"] = "open";
                lobby.SetGameServer(UserData.Me.id);
                manager.SetJoinable(true);
                Announce(CrashRecoveryLocalization.Invite);
            }
            catch (Exception exception)
            {
                SupportLogger.Failure("checkpoint_lobby_restore_failed", exception);
                Announce(CrashRecoveryLocalization.LobbyFailed);
            }
        }

        private static void Announce(string key) =>
            NativeModNotifications.Important("disk-recovery", () => ModLocalization.Get(key), 8f);

        private static bool Arrived(string floor)
        {
            PlayerAvatar player = LocalPlayerResolver.Resolve();
            return player != null && player.loadingScreenType == -1 && player.currentFloorGuid == floor;
        }
    }
}
