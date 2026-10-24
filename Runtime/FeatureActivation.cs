#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;

namespace SephiriaEnhancements.Runtime
{
    // Capture once at load: changing an opt-in cannot partially activate a live feature.
    internal sealed class FeatureActivation
    {
        private readonly HashSet<FeatureId> enabled = new();

        internal static FeatureId[] OptionalFeatures => Enum.GetValues(typeof(FeatureId))
            .Cast<FeatureId>().Where(feature => feature != FeatureId.Settings &&
                feature != FeatureId.Gameplay && feature != FeatureId.MultiplayerAccess &&
                Owner(feature) == feature).ToArray();

        internal static bool DefaultEnabled(FeatureId feature) => feature == FeatureId.CrashRecovery;

        internal FeatureActivation(Func<FeatureId, bool> readOptIn)
        {
            enabled.Add(FeatureId.Settings);
            enabled.Add(FeatureId.MultiplayerAccess);
            foreach (FeatureId feature in OptionalFeatures)
                if (readOptIn(feature)) enabled.Add(feature);
            if (enabled.Count > 2) enabled.Add(FeatureId.Gameplay);
        }

        internal bool IsEnabled(FeatureId feature) => enabled.Contains(Owner(feature));

        internal bool UsesShortcuts => new[] { FeatureId.Inventory, FeatureId.CombatInsights,
            FeatureId.CombatTargeting, FeatureId.AutoCasting, FeatureId.MapEnhancements,
            FeatureId.KeyboardUiNavigation, FeatureId.ItemCommunication }
            .Any(IsEnabled);

        private static FeatureId Owner(FeatureId feature) => feature switch
        {
            FeatureId.CharacterPanelNavigation or FeatureId.OptionsNavigation or
                FeatureId.RewardNavigation or FeatureId.WorldMapKeyboardScrolling => FeatureId.KeyboardUiNavigation,
            _ => feature
        };
    }
}
