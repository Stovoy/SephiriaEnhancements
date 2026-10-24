#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace SephiriaEnhancements.CrashRecovery
{
    internal sealed class DiskCheckpointStore
    {
        private readonly string root;
        private const int RetainedGenerations = 4;

        internal DiskCheckpointStore(string root) => this.root = Path.GetFullPath(root);

        internal void Commit(DiskCheckpoint metadata, Action<string> writeAccount,
            Action<string> writeRun)
        {
            if (metadata == null || string.IsNullOrWhiteSpace(metadata.Profile) ||
                string.IsNullOrEmpty(metadata.RunIdentity) || string.IsNullOrEmpty(metadata.AreaGuid))
                throw new ArgumentException("A checkpoint must identify its profile, run and area.");

            Publish(metadata.Profile, directory =>
            {
                string account = Path.Combine(directory, "account.sav");
                string run = Path.Combine(directory, "run.sav");
                writeAccount(account);
                writeRun(run);
                metadata.AccountHash = FlushAndHash(account);
                metadata.RunHash = FlushAndHash(run);
                using (var stream = NewFile(Path.Combine(directory, "manifest.json")))
                {
                    new DataContractJsonSerializer(typeof(DiskCheckpoint)).WriteObject(stream, metadata);
                    stream.Flush(true);
                }
            });
        }

        internal void Close(string profile)
        {
            if (!Directory.Exists(ProfileDirectory(profile))) return;
            Publish(profile, directory =>
            {
                using (var stream = NewFile(Path.Combine(directory, "closed"))) stream.Flush(true);
            });
        }

        internal DiskCheckpointBundle? Read(string profile, string gameVersion, int saveVersion)
        {
            int skipped = 0;
            foreach (string directory in Generations(profile))
            {
                if (File.Exists(Path.Combine(directory, "closed"))) return null;
                try
                {
                    DiskCheckpoint? metadata;
                    using (var stream = File.OpenRead(Path.Combine(directory, "manifest.json")))
                        metadata = (DiskCheckpoint?)new DataContractJsonSerializer(typeof(DiskCheckpoint)).ReadObject(stream);
                    if (metadata == null || metadata.Format != 1 || metadata.Profile != profile)
                        throw new SerializationException("Invalid checkpoint manifest.");
                    if (metadata.GameVersion != gameVersion || metadata.SaveVersion != saveVersion)
                        throw new InvalidOperationException("The checkpoint belongs to a different game/save version.");
                    byte[] account = ReadVerified(directory, "account.sav", metadata.AccountHash);
                    byte[] run = ReadVerified(directory, "run.sav", metadata.RunHash);
                    return new DiskCheckpointBundle(metadata, account, run, skipped);
                }
                catch (IOException) { skipped++; }
                catch (SerializationException) { skipped++; }
                catch (CryptographicException) { skipped++; }
            }
            if (skipped != 0) throw new InvalidDataException("No intact checkpoint remains for this profile.");
            return null;
        }

        private void Publish(string profile, Action<string> write)
        {
            string parent = ProfileDirectory(profile);
            Directory.CreateDirectory(parent);
            string pending = Path.Combine(parent, "pending-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(pending);
            try
            {
                write(pending);
                long previous = Generations(profile).Select(directory => new DirectoryInfo(directory).Name)
                    .Select(name => long.Parse(name.Substring(5, 19))).DefaultIfEmpty(0).Max();
                long sequence = Math.Max(DateTime.UtcNow.Ticks, checked(previous + 1));
                Directory.Move(pending, Path.Combine(parent, "save-" + sequence.ToString("D19") + "-" + Guid.NewGuid().ToString("N")));
            }
            finally
            {
                if (Directory.Exists(pending)) Directory.Delete(pending, true);
            }
            foreach (string obsolete in Generations(profile).Skip(RetainedGenerations))
            {
                try { Directory.Delete(obsolete, true); }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }
        }

        private IEnumerable<string> Generations(string profile)
        {
            string parent = ProfileDirectory(profile);
            if (!Directory.Exists(parent)) return Array.Empty<string>();
            return Directory.GetDirectories(parent, "save-*").Where(directory =>
            {
                string name = Path.GetFileName(directory);
                return name.Length == 57 && name[24] == '-' &&
                    long.TryParse(name.Substring(5, 19), out _) &&
                    Guid.TryParseExact(name.Substring(25), "N", out _);
            }).OrderByDescending(Path.GetFileName, StringComparer.Ordinal).ToArray();
        }

        private string ProfileDirectory(string profile) =>
            Path.Combine(root, Hash(Encoding.UTF8.GetBytes(profile)));

        private static FileStream NewFile(string path) =>
            new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);

        private static string FlushAndHash(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                if (stream.Length == 0) throw new InvalidDataException("Empty checkpoint save.");
                stream.Flush(true);
            }
            return Hash(File.ReadAllBytes(path));
        }

        private static byte[] ReadVerified(string directory, string name, string expectedHash)
        {
            byte[] bytes = File.ReadAllBytes(Path.Combine(directory, name));
            if (bytes.Length == 0 || Hash(bytes) != expectedHash)
                throw new CryptographicException("Checkpoint checksum mismatch.");
            return bytes;
        }

        private static string Hash(byte[] bytes)
        {
            using (var sha = SHA256.Create())
                return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "").ToLowerInvariant();
        }
    }
}
