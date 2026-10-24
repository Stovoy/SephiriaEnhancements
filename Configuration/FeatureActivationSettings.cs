using SephiriaEnhancements.Runtime;

namespace SephiriaEnhancements.Configuration
{
    internal static class FeatureActivationSettings
    {
        private static FeatureActivation active = new(_ => false);

        internal static void Capture() => active = new FeatureActivation(Read);
        internal static bool IsActive(FeatureId feature) => active.IsEnabled(feature);
        internal static bool UsesShortcuts => active.UsesShortcuts;
        internal static bool Read(FeatureId feature) => OptionsBinding.Instance?.DeviceOptions?
            .GetBool(Key(feature), false) ?? false;
        internal static void Write(FeatureId feature, bool value)
        {
            OptionsBinding.Instance?.DeviceOptions?.SetBool(Key(feature), value);
            EnhancementsSettings.Save();
        }

        private static string Key(FeatureId feature) => "SephiriaEnhancements.OptIn." + feature;
    }
}
