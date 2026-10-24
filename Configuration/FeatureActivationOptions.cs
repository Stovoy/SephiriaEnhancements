using SephiriaEnhancements.Runtime;
using UnityEngine;
using static SephiriaEnhancements.Configuration.NativeOptionsRows;

namespace SephiriaEnhancements.Configuration
{
    internal static class FeatureActivationOptions
    {
        internal static void Inject(UI_OptionsPanel panel, UI_OptionBox_PartyMemberDamage template,
            Transform section)
        {
            if (panel.GetComponentInChildren<FeatureActivationOption>(true) != null) return;
            foreach (FeatureId feature in FeatureActivation.OptionalFeatures)
            {
                GameObject row = CloneRow(template, section, "Option_SephiriaEnhancements_OptIn_" + feature,
                    FeatureFailureLocalization.FeatureKey(feature), FeatureActivationLocalization.Help, 2,
                    out UI_HorizontalSelectionBox box, out UI_LocalizationStringText text);
                row.AddComponent<FeatureActivationOption>().Configure(feature, box, text);
                NativeSettingsInteraction.Bind(row, box, text, SettingInteractionKind.Master, template.valueText.text);
                MarkCategory(row, OptionsCategory.General);
                row.SetActive(true);
            }
        }
    }

    internal sealed class FeatureActivationOption : MonoBehaviour
    {
        private FeatureId feature;
        private UI_HorizontalSelectionBox box;
        private UI_LocalizationStringText text;

        internal void Configure(FeatureId featureId, UI_HorizontalSelectionBox selection,
            UI_LocalizationStringText valueText)
        { feature = featureId; box = selection; text = valueText; }

        private void OnEnable()
        {
            if (box == null) return;
            box.numberOfElements = 2;
            box.overflowType = UI_HorizontalSelectionBox.OverflowType.Repeat;
            box.OnValueChanged += Changed;
            Refresh();
        }

        private void OnDisable()
        {
            if (box != null) box.OnValueChanged -= Changed;
        }

        private void Changed(int value)
        {
            if (NativeSettingsInteraction.CanEdit(SettingInteractionKind.Master))
                FeatureActivationSettings.Write(feature, value == 1);
            Refresh();
        }

        private void Refresh()
        {
            bool enabled = FeatureActivationSettings.Read(feature);
            box.ChangeValueWithoutNotify(enabled ? 1 : 0);
            text.UpdateKey(enabled ? ModLocalization.SuiteOn : ModLocalization.SuiteOff);
        }
    }
}
