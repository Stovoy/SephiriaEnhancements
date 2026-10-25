using Mirror;

namespace SephiriaEnhancements.MultiplayerAccess.Integration
{
    internal static class NativeSavedPlayerIdentity
    {
        internal static bool TryRestore(PlayerSpawner player, string playerGuid)
        {
            if (!NetworkServer.active || !MidRunAdmissionRuntime.CanEnableNativeReconnect() ||
                SaveManager.CurrentRun == null || SaveManager.SaveVersion == 0)
                return false;

            int count = SaveManager.CurrentRun.GetInt("SavedPlayerCount", 0);
            var identities = new SavedPlayerIdentity[count];
            for (int slot = 0; slot < count; slot++)
            {
                ulong.TryParse(SaveManager.CurrentRun.GetString($"Player{slot}SteamID", ""), out ulong steamId);
                identities[slot] = new SavedPlayerIdentity(
                    SaveManager.CurrentRun.GetString($"Player{slot}Guid", ""), steamId);
            }

            int savedSlot = SavedPlayerIdentity.Resolve(identities, playerGuid, player.steamID);
            if (savedSlot < 0) return false;

            var manager = (HorayNetworkManager)NetworkManager.singleton;
            if (string.IsNullOrWhiteSpace(playerGuid) ||
                manager.IsRejoinBanned(identities[savedSlot].Guid))
            {
                return false;
            }

            foreach (var other in PlayerSpawner.MultiplayerList)
            {
                if (other != null && other != player && other.connectionToClient != null &&
                    (other.playerGuid == identities[savedSlot].Guid ||
                     (player.steamID != 0 && other.steamID == player.steamID)))
                {
                    return false;
                }
            }

            player.NetworkcurrentPlayerIdxForSave = savedSlot;
            SaveManager.CurrentRun.SetString($"Player{savedSlot}Guid", playerGuid);
            manager.AddToRejoinWhitelist(playerGuid);
            manager.MarkPendingRejoin(playerGuid);
            MidRunAdmissionRuntime.RemoveConnection(player.connectionToClient);
            return true;
        }
    }
}
