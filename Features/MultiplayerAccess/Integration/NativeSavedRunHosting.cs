using System;
using System.Collections;
using HeathenEngineering.SteamworksIntegration;
using Mirror;
using SephiriaEnhancements.Configuration;
using SephiriaEnhancements.Integration;
using Steamworks;
using UnityEngine;

namespace SephiriaEnhancements.MultiplayerAccess.Integration
{
    // Saved runs start a local server through the title menu, without a Steam room.
    // Own this lifecycle here so normal saves and disk checkpoints use the same path.
    internal sealed class NativeSavedRunHosting : MonoBehaviour
    {
        private Coroutine pending;

        private void OnEnable() => HorayModAPI.OnStartSessionServerside += OnSessionStarted;

        private void OnDisable()
        {
            HorayModAPI.OnStartSessionServerside -= OnSessionStarted;
            StopAllCoroutines();
            pending = null;
        }

        private void OnSessionStarted(bool isSavedSession)
        {
            if (pending != null) StopCoroutine(pending);
            pending = null;
            if (isSavedSession && EnhancementsSettings.Enabled && MidRunAdmissionSettings.ReconnectSupport)
                pending = StartCoroutine(Host());
        }

        private IEnumerator Host()
        {
            float deadline = Time.realtimeSinceStartup + 30f;
            while (NetworkServer.active && !Arrived() && Time.realtimeSinceStartup < deadline)
                yield return null;
            if (!NetworkServer.active) yield break;
            if (!Arrived())
            {
                yield break;
            }

            LobbyManager manager = null;
            bool requested = false;
            try
            {
                GameObject steam = SingletonObject.Find("SteamManager");
                if (steam == null || !steam.TryGetComponent(out manager))
                    throw new InvalidOperationException("Steam lobby manager is unavailable.");
                // Ignore checkpoint flags and old lobby preferences: every recovered
                // run gets two slots and friends-only access, never public discovery.
                manager.Create("Recovered Sephiria run", ELobbyType.k_ELobbyTypeFriendsOnly, 2);
                requested = true;
            }
            catch (Exception) { }
            if (!requested)
            {
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
                // Native created-event listeners can apply the multiplayer menu's
                // preferences. Enforce recovery's settings after those callbacks.
                lobby.Type = ELobbyType.k_ELobbyTypeFriendsOnly;
                lobby.MaxMembers = 2;
                lobby.Name = "Recovered Sephiria run";
                lobby.GameVersion = Application.version;
                lobby["Chapter"] = CurrentChapter();
                lobby["pw"] = "open";
                lobby.SetGameServer(UserData.Me.id);
                manager.SetJoinable(true);
            }
            catch (Exception) { }
        }

        private static string CurrentChapter()
        {
            PlayerLocalDataStorage player = LocalPlayerResolver.Resolve().localDataStorage;
            if (player.mainQuestProgress >= 11 && QuestDatabase.TryGetCurrentMainQuestChapter(
                    player.GetCurrentMainQuestNode(), out int chapter, out int subChapter))
                return subChapter > 0 ? chapter + "-" + subChapter : chapter.ToString();
            return RaceDatabase.FindById(DungeonManager.Instance.raceId).actualChapterNum.ToString();
        }

        private static bool Arrived()
        {
            PlayerAvatar player = LocalPlayerResolver.Resolve();
            return player != null && player.loadingScreenType == -1 &&
                !string.IsNullOrEmpty(player.currentFloorGuid) && DungeonManager.Instance != null;
        }

    }
}
