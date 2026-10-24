using SephiriaEnhancements.Runtime;

namespace SephiriaEnhancements.Configuration
{
    internal static class FeatureActivationSettings
    {
        private static FeatureActivation active = new(FeatureActivation.DefaultEnabled);

        internal static void Capture() => active = new FeatureActivation(Read);
        internal static bool IsActive(FeatureId feature) => active.IsEnabled(feature);
        internal static bool UsesShortcuts => active.UsesShortcuts;
        internal static bool Read(FeatureId feature) => OptionsBinding.Instance?.DeviceOptions?
            .GetBool(Key(feature), FeatureActivation.DefaultEnabled(feature)) ?? FeatureActivation.DefaultEnabled(feature);
        internal static void Write(FeatureId feature, bool value)
        {
            OptionsBinding.Instance?.DeviceOptions?.SetBool(Key(feature), value);
            EnhancementsSettings.Save();
        }

        private static string Key(FeatureId feature) => "SephiriaEnhancements.OptIn." + feature;
    }
}
