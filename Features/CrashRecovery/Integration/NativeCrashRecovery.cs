using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using HarmonyLib;
using Mirror;
using SephiriaEnhancements.Configuration;
using SephiriaEnhancements.Diagnostics;
using SephiriaEnhancements.Integration;
using UnityEngine;

namespace SephiriaEnhancements.CrashRecovery
{
    internal sealed class NativeCrashRecovery : MonoBehaviour
    {
        private static readonly MethodInfo SerializeDungeon = AccessTools.Method(
            typeof(DungeonManager), "SaveCurrentSessionData", new[] { typeof(string) });
        private static readonly FieldInfo Account = AccessTools.Field(typeof(SaveManager), "current");
        private static readonly FieldInfo Run = AccessTools.Field(typeof(SaveManager), "currentRun");
        private static NativeCrashRecovery instance;
        private bool bypassTitle;
        private bool promptOpen;
        private DiskCheckpoint restoredLobby;

        private static DiskCheckpointStore Store => new DiskCheckpointStore(Path.Combine(
            SaveData.CommonPath, "Mods", "SephiriaEnhancements", "Checkpoints"));

        private void Awake() => instance = this;

        private void OnDestroy()
        {
            StopAllCoroutines();
            if (instance == this) instance = null;
        }

        internal static void Capture(DungeonManager dungeon, bool allowSave, string floorGuid, bool runStarted)
        {
            if (!EnhancementsSettings.Enabled || !allowSave || !runStarted || !NetworkServer.active ||
                dungeon.isGiveUpRun || SaveManager.Current == null || SaveManager.CurrentRun == null ||
                !dungeon.generatedFloors.TryGetValue(floorGuid, out FloorData floor) || floor.name == "MultiZone") return;
            if (SerializeDungeon == null) throw new MissingMethodException("Native dungeon save serializer is unavailable.");

            SerializeDungeon.Invoke(dungeon, new object[] { floorGuid });
            foreach (PlayerSpawner player in PlayerSpawner.MultiplayerList)
                player.SaveCurrentSessionData();
            SaveData run = SaveManager.CurrentRun.Copy();
            var metadata = new DiskCheckpoint
            {
                Profile = SaveManager.Binded,
                RunIdentity = run.GetInt("Seed", 0) + ":" + run.GetInt("CurrentGame", -1),
                AreaGuid = floorGuid,
                AreaName = floor.name,
                CreatedUtc = DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture),
                GameVersion = Application.version,
                SaveVersion = SaveManager.SaveVersion
            };
            NativeRecoveryLobby.Capture(metadata);
            if (instance?.restoredLobby != null && instance.restoredLobby.RunIdentity == metadata.RunIdentity &&
                metadata.LobbyChapter.Length == 0)
            {
                metadata.Multiplayer = instance.restoredLobby.Multiplayer;
                metadata.LobbyCapacity = instance.restoredLobby.LobbyCapacity;
                metadata.LobbyChapter = instance.restoredLobby.LobbyChapter;
            }
            NativeCheckpointCodec.ValidateRun(run, metadata);
            SaveData account = SaveManager.Current.Copy();
            Store.Commit(metadata, path => NativeCheckpointCodec.Write(account, path),
                path => NativeCheckpointCodec.Write(run, path));
            SupportLogger.Record("disk_checkpoint_committed", "players=" + run.GetInt("SavedPlayerCount", 0));
        }

        internal static void CloseRun()
        {
            if (EnhancementsSettings.Enabled && NetworkServer.active && !string.IsNullOrEmpty(SaveManager.Binded))
            {
                Store.Close(SaveManager.Binded);
                if (instance != null) instance.restoredLobby = null;
            }
        }

        internal static bool Offer(UI_TitleLobby title)
        {
            if (!EnhancementsSettings.Enabled || instance == null || instance.bypassTitle ||
                NetworkServer.active || NetworkClient.active || SteamInvitation.waitForExternalConnect ||
                ScreenFader.Instance.IsFading) return true;
            if (instance.promptOpen) return false;
            string profile = SelectedProfile();
            DiskCheckpointBundle bundle = Store.Read(profile, Application.version, SaveManager.SaveVersion);
            if (bundle == null) return true;
            string text = string.Format(ModLocalization.Get(CrashRecoveryLocalization.Resume),
                bundle.Metadata.AreaName, DateTime.Parse(bundle.Metadata.CreatedUtc,
                    CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind).ToLocalTime());
            if (bundle.SkippedCheckpoints != 0) text += "\n" + ModLocalization.Get(CrashRecoveryLocalization.OlderBackup);
            bool opened = NativeModNotifications.Choose(() => text,
                () => instance?.Recover(title, profile, bundle),
                () => instance?.ContinueNormally(title, profile));
            instance.promptOpen = opened;
            return false;
        }

        private void ContinueNormally(UI_TitleLobby title, string profile)
        {
            promptOpen = false;
            if (title == null || profile != SelectedProfile()) return;
            bypassTitle = true;
            try { title.NetworkHost(); }
            finally { bypassTitle = false; }
        }

        private void Recover(UI_TitleLobby title, string profile, DiskCheckpointBundle bundle)
        {
            promptOpen = false;
            if (title == null || profile != SelectedProfile() || NetworkServer.active || NetworkClient.active) return;
            try
            {
                if (Account == null || Run == null) throw new MissingFieldException("Native save binding is unavailable.");
                SaveData account = NativeCheckpointCodec.Decode(bundle.Account, profile);
                SaveData run = NativeCheckpointCodec.Decode(bundle.Run, profile + "TMP");
                NativeCheckpointCodec.ValidateRun(run, bundle.Metadata);
                SaveData previousAccount = SaveManager.Current;
                SaveData previousRun = SaveManager.CurrentRun;
                string previousProfile = SaveManager.Binded;
                try
                {
                    restoredLobby = bundle.Metadata;
                    SaveManager.Binded = profile;
                    Account.SetValue(null, account);
                    Run.SetValue(null, run);
                    SaveManager.ApplyPostLoadSaveFixes();
                    EOSLobbyManager.StartHostWhenReady();
                }
                catch
                {
                    restoredLobby = null;
                    if (!NetworkServer.active && !NetworkClient.active)
                    {
                        Account.SetValue(null, previousAccount);
                        Run.SetValue(null, previousRun);
                        SaveManager.Binded = previousProfile;
                    }
                    throw;
                }
                StartCoroutine(NativeRecoveryLobby.Restore(bundle.Metadata));
                SupportLogger.Record("disk_checkpoint_restored");
            }
            catch (Exception exception)
            {
                SupportLogger.Failure("disk_checkpoint_restore_failed", exception);
                NativeModNotifications.Confirm(() => ModLocalization.Get(CrashRecoveryLocalization.Failed), null);
            }
        }

        private static string SelectedProfile() =>
            OptionsBinding.Instance.Options.GetString("SelectedProfile", SaveManager.defaultSlotName);
    }
}
