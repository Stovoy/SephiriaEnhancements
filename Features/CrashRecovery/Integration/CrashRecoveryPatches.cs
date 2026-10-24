using HarmonyLib;
using Mirror;
using SephiriaEnhancements.Runtime;

namespace SephiriaEnhancements.CrashRecovery
{
    [HarmonyPatch(typeof(DungeonManager), "HandleStartFloorAfterSavingServerside")]
    internal static class DiskAreaCheckpointPatch
    {
        private static void Postfix(DungeonManager __instance, bool allowSave, string floorGuid, bool runStarted)
        {
            if (!FeatureFailure.IsAvailable(FeatureId.CrashRecovery)) return;
            try { PostfixCore(__instance, allowSave, floorGuid, runStarted); }
            catch (System.Exception exception) { FeatureFailure.Disable(FeatureId.CrashRecovery, exception); }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static void PostfixCore(DungeonManager dungeon, bool allowSave, string floorGuid, bool runStarted) =>
            NativeCrashRecovery.Capture(dungeon, allowSave, floorGuid, runStarted);
    }

    [HarmonyPatch(typeof(UI_TitleLobby), nameof(UI_TitleLobby.NetworkHost))]
    internal static class DiskCheckpointResumePatch
    {
        private static bool Prefix(UI_TitleLobby __instance)
        {
            if (!FeatureFailure.IsAvailable(FeatureId.CrashRecovery)) return true;
            try { return PrefixCore(__instance); }
            catch (System.Exception exception)
            {
                FeatureFailure.Disable(FeatureId.CrashRecovery, exception);
                return true;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static bool PrefixCore(UI_TitleLobby title) => NativeCrashRecovery.Offer(title);
    }

    [HarmonyPatch(typeof(DungeonManager), nameof(DungeonManager.LoadStageAndMove))]
    internal static class DiskCheckpointNewRunPatch
    {
        private static void Prefix(DungeonManager __instance)
        {
            if (!FeatureFailure.IsAvailable(FeatureId.CrashRecovery)) return;
            try { PrefixCore(__instance); }
            catch (System.Exception exception) { FeatureFailure.Disable(FeatureId.CrashRecovery, exception); }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static void PrefixCore(DungeonManager dungeon)
        {
            if (!dungeon.isRunStarted) NativeCrashRecovery.CloseRun();
        }
    }

    [HarmonyPatch(typeof(PlayerSpawner), nameof(PlayerSpawner.RpcGameOver))]
    internal static class DiskCheckpointConclusionPatch
    {
        private static void Prefix()
        {
            if (!FeatureFailure.IsAvailable(FeatureId.CrashRecovery)) return;
            try { PrefixCore(); }
            catch (System.Exception exception) { FeatureFailure.Disable(FeatureId.CrashRecovery, exception); }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        private static void PrefixCore()
        {
            if (!NetworkServer.active || DungeonManager.Instance == null) return;
            bool ended = DungeonManager.Instance.isGiveUpRun || DungeonManager.Instance.victoryType != 0;
            bool allDead = PlayerSpawner.MultiplayerList.Count > 0;
            foreach (PlayerSpawner player in PlayerSpawner.MultiplayerList)
                allDead &= player.PlayerAvatar != null && player.PlayerAvatar.IsDead;
            if (ended || allDead) NativeCrashRecovery.CloseRun();
        }
    }
}
