using System.Text.Json;
using SephiriaEnhancements.CrashRecovery;
using SephiriaEnhancements.MultiplayerAccess;

namespace SephiriaEnhancements.ModelChecks.Features.MultiplayerAccess;

internal static class SavedPlayerIdentityChecks
{
    internal static void Run()
    {
        var players = new[]
        {
            new SavedPlayerIdentity("host-before-crash", 100),
            new SavedPlayerIdentity("friend-before-crash", 200)
        };
        Require(SavedPlayerIdentity.Resolve(players, "friend-before-crash", 200) == 1);
        Require(SavedPlayerIdentity.Resolve(players, "friend-after-crash", 200) == 1);
        Require(SavedPlayerIdentity.Resolve(players, "host-after-crash", 100) == 0);
        Require(SavedPlayerIdentity.Resolve(players, "new-friend", 300) == -1);
        Require(SavedPlayerIdentity.Resolve(players, "friend-before-crash", 300) == -2);
        Require(SavedPlayerIdentity.Resolve(players, "friend-before-crash", 0) == 1);
        Require(SavedPlayerIdentity.Resolve(players, "", 0) == -1);
        Require(SavedPlayerIdentity.Resolve(new[] { players[1], players[1] }, "new-id", 200) == -2);
        Require(SavedPlayerIdentity.Resolve(players, "host-before-crash", 200) == -2);

        string root = Path.Combine(Path.GetTempPath(), "sephiria-player-recovery-" + Guid.NewGuid().ToString("N"));
        try
        {
            var run = new Dictionary<string, string>
            {
                ["Player0Guid"] = players[0].Guid,
                ["Player0SteamID"] = "100",
                ["Player1Guid"] = players[1].Guid,
                ["Player1SteamID"] = "200",
                ["Player0Inventory0,0_Entity"] = "111",
                ["Player1Inventory0,0_Entity"] = "222",
                ["Player1Inventory0,0_InstanceID"] = "987",
                ["Player1Inventory0,0_Quantity"] = "3",
                ["Player1Level"] = "19",
                ["Player1Money"] = "2563"
            };
            new DiskCheckpointStore(root).Commit(new DiskCheckpoint
            {
                Profile = "host",
                RunIdentity = "run",
                GameVersion = "test",
                SaveVersion = 2,
                AreaGuid = "area",
                Multiplayer = true
            }, path => File.WriteAllText(path, "account"),
                path => File.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(run)));
            var disk = new DiskCheckpointStore(root).Read("host", "test", 2)!;
            var restored = JsonSerializer.Deserialize<Dictionary<string, string>>(disk.Run)!;
            var restoredPlayers = Enumerable.Range(0, 2).Select(slot => new SavedPlayerIdentity(
                restored[$"Player{slot}Guid"], ulong.Parse(restored[$"Player{slot}SteamID"]))).ToArray();
            int friend = SavedPlayerIdentity.Resolve(restoredPlayers, "new-reconnect-id", 200);
            Require(friend == 1);
            Require(restored[$"Player{friend}Inventory0,0_Entity"] == "222");
            Require(restored[$"Player{friend}Inventory0,0_InstanceID"] == "987");
            Require(restored[$"Player{friend}Inventory0,0_Quantity"] == "3");
            Require(restored[$"Player{friend}Level"] == "19" && restored[$"Player{friend}Money"] == "2563");
            Require(restored["Player0Inventory0,0_Entity"] == "111");
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    private static void Require(bool condition)
    {
        if (!condition) throw new InvalidOperationException("Saved player identity recovery failed.");
    }
}
