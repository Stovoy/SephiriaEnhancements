using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SephiriaEnhancements.CrashRecovery
{
    internal static class NativeCheckpointCodec
    {
        internal static void Write(SaveData source, string path)
        {
            SaveData snapshot = source.Copy();
            snapshot.enableSave = true;
            snapshot.enableCloudSave = false;
            snapshot.Save(path);
            SaveData actual = Decode(File.ReadAllBytes(path), source.BindedFileName);
            var values = actual.BakedData.ToDictionary(pair => pair.Key, pair => pair.Value);
            var expected = snapshot.BakedData.ToDictionary(pair => pair.Key, pair => pair.Value);
            if (values.Count != expected.Count || !expected.All(pair =>
                values.TryGetValue(pair.Key, out object value) && Equals(value, pair.Value)))
                throw new InvalidDataException("Checkpoint readback differs from the captured save.");
        }

        internal static SaveData Decode(byte[] bytes, string fileName)
        {
            var data = new SaveData(useEncryption: true) { enableSave = false };
            data.CreateNew(fileName);
            if (!data.LoadFromString(Encoding.UTF8.GetString(bytes)))
                throw new InvalidDataException("The game could not decode this checkpoint.");
            data.enableSave = true;
            return data;
        }

        internal static void ValidateRun(SaveData run, DiskCheckpoint metadata)
        {
            string identity = run.GetInt("Seed", 0) + ":" + run.GetInt("CurrentGame", -1);
            if (!run.GetBool("RunStarted", false) || identity != metadata.RunIdentity ||
                run.GetString("LastFloorGuid", "") != metadata.AreaGuid ||
                run.GetInt("SaveVersion", -1) != metadata.SaveVersion ||
                run.GetInt("SavedPlayerCount", 0) <= 0)
                throw new InvalidDataException("Checkpoint run metadata does not match its save.");
        }
    }
}
