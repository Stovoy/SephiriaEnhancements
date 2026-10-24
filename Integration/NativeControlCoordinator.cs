using System.Collections.Generic;
using SephiriaEnhancements.KeyboardUiNavigation;
using SephiriaEnhancements.CombatTargeting;
using UnityEngine.InputSystem;

namespace SephiriaEnhancements.Integration
{
    internal static class NativeControlCoordinator
    {
        private static readonly HashSet<int> SynchronizedRuntimeAssets =
            new HashSet<int>();

        internal static void Initialize()
        {
            if (!Configuration.FeatureActivationSettings.UsesShortcuts) return;
            InputActionAsset canonical = OptionsBinding.Instance?.actionAsset;
            if (canonical == null)
            {
                return;
            }

            NativeInputActions.EnsureShortcutMap(canonical);
            OfficialCombatBindings.RestoreSavedFallbackBindings(canonical);
            OfficialUiKeyboardBindings.RestoreSavedFallbackBindings(canonical);
            NativeInputActions.ReloadSavedOverrides(canonical);
            OfficialUiKeyboardBindings.EnsureFallbackBindingInitialization();
            OfficialCombatBindings.
                EnsureDefaultBindingInitializationIfEnabled();
        }

        internal static void PreparePlayerInput(PlayerInputController input)
        {
            if (!Configuration.FeatureActivationSettings.UsesShortcuts) return;
            OfficialCombatBindings.
                EnsureDefaultBindingInitializationIfEnabled();
            OfficialUiKeyboardBindings.EnsureFallbackBindingInitialization();
            InputActionAsset runtime = input?.playerInput?.actions;
            if (runtime == null)
            {
                return;
            }

            NativeInputActions.EnsureShortcutMap(runtime);
            int assetId = runtime.GetInstanceID();
            if (!SynchronizedRuntimeAssets.Contains(assetId))
            {
                InputActionAsset canonical = OptionsBinding.Instance?.actionAsset;
                OfficialCombatBindings.EnsureRuntimeBindings(canonical, runtime);
                OfficialUiKeyboardBindings.EnsureRuntimeBindings(canonical, runtime);
                NativeInputActions.SynchronizeOverrides(runtime);
                SynchronizedRuntimeAssets.Add(assetId);
            }

            NativeInputActions.EnableShortcuts(runtime);
        }

        internal static void OnTargetingSettingChanged(bool enabled)
        {
            OfficialCombatBindings.OnTargetingSettingChanged(enabled);
            InputActionAsset runtime =
                PlayerInputController.Instance?.playerInput?.actions;
            if (runtime == null)
            {
                return;
            }

            InputActionAsset canonical = OptionsBinding.Instance?.actionAsset;
            OfficialCombatBindings.EnsureRuntimeBindings(canonical, runtime);
            NativeInputActions.SynchronizeOverrides(runtime);
            NativeInputActions.EnableShortcuts(runtime);
        }

        internal static void ReloadOfficialBindings()
        {
            if (!Configuration.FeatureActivationSettings.UsesShortcuts) return;
            InputActionAsset canonical = OptionsBinding.Instance?.actionAsset;
            if (canonical == null)
            {
                return;
            }

            NativeInputActions.EnsureShortcutMap(canonical);
            OfficialCombatBindings.RestoreSavedFallbackBindings(canonical);
            OfficialUiKeyboardBindings.RestoreSavedFallbackBindings(canonical);
            NativeInputActions.ReloadSavedOverrides(canonical);

            InputActionAsset runtime =
                PlayerInputController.Instance?.playerInput?.actions;
            OfficialCombatBindings.EnsureRuntimeBindings(canonical, runtime);
            OfficialUiKeyboardBindings.EnsureRuntimeBindings(canonical, runtime);
            NativeInputActions.SynchronizeOverrides(runtime);
            NativeInputActions.EnableShortcuts(runtime);
        }
    }
}
