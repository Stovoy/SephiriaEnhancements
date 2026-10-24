#nullable enable
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SephiriaEnhancements.CrashRecovery;

namespace SephiriaEnhancements.ModelChecks.Features.CrashRecovery;

internal static class DiskCheckpointStoreChecks
{
    internal static void Run()
    {
        string root = Path.Combine(Path.GetTempPath(), "sephiria-checkpoints-" + Guid.NewGuid().ToString("N"));
        try
        {
            var store = new DiskCheckpointStore(root);
            Require(store.Read("host", "test", 2) == null, "fresh profile");
            Commit(store, "host", "area-one");
            Require(ReadArea(store, "host") == "area-one", "committed area");

            Expect<IOException>(() => store.Commit(Metadata("host", "area-two"),
                path => File.WriteAllText(path, "new-account"), _ => throw new IOException("interrupted")));
            Require(ReadArea(store, "host") == "area-one", "interrupted write");
            Expect<FileNotFoundException>(() => store.Commit(Metadata("host", "area-two"), _ => { },
                path => File.WriteAllText(path, "new-run")));
            Require(ReadArea(store, "host") == "area-one", "silent write failure");

            string profileDirectory = Directory.GetDirectories(root).Single();
            Directory.CreateDirectory(Path.Combine(profileDirectory, "pending-simulated-crash"));
            Require(ReadArea(store, "host") == "area-one", "abandoned staging directory");

            Worker(root, "interrupted", 17);
            Require(ReadArea(store, "host") == "area-one", "abrupt process exit before publication");
            Worker(root, "complete", 0);
            Require(ReadArea(store, "host") == "process-area", "checkpoint published by another process");
            Commit(store, "host", "area-one");

            Commit(store, "host", "area-two");
            string newest = Directory.GetDirectories(profileDirectory, "save-*")
                .OrderByDescending(Path.GetFileName, StringComparer.Ordinal).First();
            File.AppendAllText(Path.Combine(newest, "run.sav"), "corruption");
            var fallback = store.Read("host", "test", 2);
            Require(fallback != null && fallback.Metadata.AreaGuid == "area-one" &&
                fallback.SkippedCheckpoints == 1, "checksum fallback");
            Require(Encoding.UTF8.GetString(fallback!.Account) == "account-area-one" &&
                Encoding.UTF8.GetString(fallback.Run) == "run-area-one", "matching pair");

            store.Close("host");
            Require(store.Read("host", "test", 2) == null, "ended run cannot resurrect");
            Commit(store, "host", "new-run");
            newest = Directory.GetDirectories(profileDirectory, "save-*")
                .OrderByDescending(Path.GetFileName, StringComparer.Ordinal).First();
            File.Delete(Path.Combine(newest, "account.sav"));
            Require(store.Read("host", "test", 2) == null, "fallback stops at run boundary");

            Commit(store, "host", "area-three");
            Expect<InvalidOperationException>(() => store.Read("host", "different-game", 2));
            Expect<InvalidOperationException>(() => store.Read("host", "test", 3));
            Require(store.Read("guest", "test", 2) == null, "profile isolation");
            Commit(store, "../guest", "guest-area");
            Require(ReadArea(store, "../guest") == "guest-area" && ReadArea(store, "host") == "area-three",
                "profile names cannot traverse directories");

            for (int i = 0; i < 8; i++) Commit(store, "host", "retained-" + i);
            Require(Directory.GetDirectories(profileDirectory, "save-*").Length == 4,
                "bounded retention");
            Require(ReadArea(new DiskCheckpointStore(root), "host") == "retained-7",
                "reopening the store");
            Console.WriteLine("Disk checkpoint transaction, corruption, restart and profile checks passed.");
        }
        finally
        {
            if (Directory.Exists(root)) Directory.Delete(root, true);
        }
    }

    internal static void RunWorker(string[] args)
    {
        var store = new DiskCheckpointStore(args[1]);
        store.Commit(Metadata("host", "process-area"), path => File.WriteAllText(path, "process-account"),
            path =>
            {
                File.WriteAllText(path, "process-run");
                if (args[2] == "interrupted") Environment.Exit(17);
            });
    }

    private static void Worker(string root, string mode, int expectedExit)
    {
        var start = new ProcessStartInfo(Environment.ProcessPath!)
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        if (string.Equals(Path.GetFileNameWithoutExtension(Environment.ProcessPath), "dotnet", StringComparison.OrdinalIgnoreCase))
            start.ArgumentList.Add(Assembly.GetExecutingAssembly().Location);
        start.ArgumentList.Add("--disk-checkpoint-worker");
        start.ArgumentList.Add(root);
        start.ArgumentList.Add(mode);
        using var process = Process.Start(start)!;
        if (!process.WaitForExit(10000))
        {
            process.Kill(true);
            throw new InvalidOperationException("Checkpoint subprocess timed out.");
        }
        Require(process.ExitCode == expectedExit, "checkpoint worker exit: " + process.StandardError.ReadToEnd());
    }

    private static DiskCheckpoint Metadata(string profile, string area) => new()
    {
        Profile = profile,
        AreaGuid = area,
        AreaName = area,
        RunIdentity = "123:1",
        CreatedUtc = DateTime.UtcNow.ToString("O"),
        GameVersion = "test",
        SaveVersion = 2
    };

    private static void Commit(DiskCheckpointStore store, string profile, string area) =>
        store.Commit(Metadata(profile, area), path => File.WriteAllText(path, "account-" + area),
            path => File.WriteAllText(path, "run-" + area));

    private static string? ReadArea(DiskCheckpointStore store, string profile) =>
        store.Read(profile, "test", 2)?.Metadata.AreaGuid;

    private static void Expect<T>(Action action) where T : Exception
    {
        try { action(); }
        catch (T) { return; }
        throw new InvalidOperationException("Expected " + typeof(T).Name);
    }

    private static void Require(bool condition, string name)
    {
        if (!condition) throw new InvalidOperationException("Checkpoint check failed: " + name);
    }
}
