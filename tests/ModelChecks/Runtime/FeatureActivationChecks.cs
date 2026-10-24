using HarmonyLib;
using System.Runtime.CompilerServices;
using SephiriaEnhancements.Runtime;

namespace SephiriaEnhancements.ModelChecks.Runtime;

internal static class FeatureActivationChecks
{
    internal static void Run()
    {
        var defaults = new FeatureActivation(_ => false);
        foreach (FeatureId feature in Enum.GetValues<FeatureId>())
            Require(defaults.IsEnabled(feature) == (feature is FeatureId.Settings or FeatureId.MultiplayerAccess),
                "Fresh defaults may only install settings and joining/reconnect support: " + feature);
        Require(!defaults.UsesShortcuts, "Fresh defaults must not modify input bindings");
        var preferences = new HashSet<FeatureId> { FeatureId.MultiplayerRules };
        var optedIn = new FeatureActivation(preferences.Contains);
        preferences.Clear();
        Require(optedIn.IsEnabled(FeatureId.MultiplayerRules) && !optedIn.IsEnabled(FeatureId.ItemCommunication),
            "An opt-in is isolated and remains stable until reload");
        Require(!new FeatureActivation(preferences.Contains).IsEnabled(FeatureId.MultiplayerRules),
            "Reload must apply a disabled feature");
        foreach (FeatureId feature in FeatureActivation.OptionalFeatures)
        {
            var single = new FeatureActivation(candidate => candidate == feature);
            Require(single.IsEnabled(feature), "Every optional subsystem must be explicitly selectable");
            foreach (FeatureId other in FeatureActivation.OptionalFeatures.Where(other => other != feature))
                Require(!single.IsEnabled(other), "Enabling one subsystem must not opt into another");
        }
        var keyboard = new FeatureActivation(feature => feature == FeatureId.KeyboardUiNavigation);
        Require(keyboard.IsEnabled(FeatureId.RewardNavigation) && keyboard.UsesShortcuts,
            "Keyboard page patches share their controller's opt-in");

        FeatureFailure.Reset();
        FeatureFailure.ExecutionAllowed = defaults.IsEnabled;
        int calls = 0;
        Require(!FeatureFailure.Run(FeatureId.CrashRecovery, () => calls++), "Disabled controllers/events must not run");
        Require(FeatureFailure.Run(FeatureId.MultiplayerAccess, () => calls++), "Multiplayer support must run");
        Require(calls == 1 && FeatureFailure.PendingNotices.Length == 0,
            "Opting out must not become a feature failure or notification");
        var patches = new FeaturePatchSet("SephiriaEnhancements.Tests.Defaults");
        try
        {
            Require(!patches.Install(FeatureId.ItemCommunication, typeof(OptionalPatch)),
                "Disabled hooks must not be installed");
            Require(Target() == 0, "Default behavior must remain original");
            FeatureFailure.ExecutionAllowed = new FeatureActivation(feature => feature == FeatureId.ItemCommunication).IsEnabled;
            Require(patches.Install(FeatureId.ItemCommunication, typeof(OptionalPatch)) && Target() == 1,
                "An explicit opt-in must install and execute its hook");
        }
        finally
        {
            patches.Remove(FeatureId.ItemCommunication);
            FeatureFailure.Reset();
        }
        Require(Target() == 0, "Unloading restores native behavior");
        Console.WriteLine("Feature activation: all defaults, independent opt-ins, reload, controller/event and actual hook execution passed.");
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int Target() => 0;

    [HarmonyPatch(typeof(FeatureActivationChecks), nameof(Target))]
    private static class OptionalPatch
    {
        private static void Postfix(ref int __result) => __result++;
    }

    private static void Require(bool condition, string message)
    {
        if (!condition) throw new InvalidOperationException(message);
    }
}
