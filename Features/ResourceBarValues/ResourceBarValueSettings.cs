namespace SephiriaEnhancements.ResourceBarValues
{
    internal static class ResourceBarValueSettings
    {
        internal static bool Get(ResourceBarValueSetting setting)
        {
            const bool fallback = false;
            return OptionsBinding.Instance?.DeviceOptions?.GetBool(Key(setting), fallback) ?? fallback;
        }

        internal static void Set(ResourceBarValueSetting setting, bool value) =>
            OptionsBinding.Instance?.DeviceOptions?.SetBool(Key(setting), value);

        private static string Key(ResourceBarValueSetting setting) =>
            "SephiriaEnhancements.ResourceBarValues." + setting;

        internal static void Save()
        {
            OptionsBinding.Instance?.DeviceOptions?.Save();
        }
    }
}
