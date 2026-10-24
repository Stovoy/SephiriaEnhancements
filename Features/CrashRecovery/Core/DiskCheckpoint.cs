#nullable enable
using System.Runtime.Serialization;

namespace SephiriaEnhancements.CrashRecovery
{
    [DataContract]
    internal sealed class DiskCheckpoint
    {
        [DataMember] public int Format = 1;
        [DataMember] public string Profile = "";
        [DataMember] public string RunIdentity = "";
        [DataMember] public string AreaGuid = "";
        [DataMember] public string AreaName = "";
        [DataMember] public string CreatedUtc = "";
        [DataMember] public string GameVersion = "";
        [DataMember] public int SaveVersion;
        [DataMember] public string AccountHash = "";
        [DataMember] public string RunHash = "";
        [DataMember] public bool Multiplayer = false;
        [DataMember] public int LobbyCapacity = 4;
        [DataMember] public string LobbyChapter = "";
    }

    internal sealed class DiskCheckpointBundle
    {
        internal DiskCheckpoint Metadata { get; }
        internal byte[] Account { get; }
        internal byte[] Run { get; }
        internal int SkippedCheckpoints { get; }

        internal DiskCheckpointBundle(DiskCheckpoint metadata, byte[] account,
            byte[] run, int skippedCheckpoints)
        {
            Metadata = metadata;
            Account = account;
            Run = run;
            SkippedCheckpoints = skippedCheckpoints;
        }
    }
}
