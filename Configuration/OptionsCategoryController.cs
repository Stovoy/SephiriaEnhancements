using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using SephiriaEnhancements.CombatVisuals;

namespace SephiriaEnhancements.Configuration
{
    internal sealed class OptionsSectionMarker : MonoBehaviour
    {
    }

    internal sealed class MultiplayerSectionMarker : MonoBehaviour
    {
    }

    internal sealed class ModOptionsNavigationEntry : MonoBehaviour
    {
        internal UI_HorizontalSelectionBox Box { get; private set; }

        internal void Configure(UI_HorizontalSelectionBox box)
        {
            Box = box;
        }
    }

    internal sealed class OptionsCategoryMember : MonoBehaviour
    {
        internal OptionsCategory Category { get; private set; }

        internal void Configure(OptionsCategory category)
        {
            Category = category;
        }
    }

    internal sealed class OptionsCategoryController : MonoBehaviour
    {
        private UI_OptionsPanel panel;
        private UI_OptionBox_PartyMemberDamage template;
        private RectTransform content;
        private UI_HorizontalSelectionBox categoryBox;

        internal OptionsCategory SelectedCategory { get; private set; } =
            OptionsCategory.General;

        internal void Configure(UI_OptionsPanel optionsPanel,
            UI_OptionBox_PartyMemberDamage rowTemplate, Transform contentTransform)
        {
            panel = optionsPanel;
            template = rowTemplate;
            content = contentTransform as RectTransform;
        }

        internal void RegisterCategoryBox(UI_HorizontalSelectionBox box)
        {
            categoryBox = box;
        }

        internal void SelectCategory(OptionsCategory category)
        {
            if ((int)category < 0 ||
                (int)category >= OptionsCategoryLocalization.CategoryKeys.Length)
            {
                category = OptionsCategory.General;
            }
            SelectedCategory = category;
            RefreshVisibility();
        }


        internal void RefreshVisibility()
        {
            if (panel == null || template == null) return;

            Vector2 anchoredPosition = content != null
                ? content.anchoredPosition : Vector2.zero;
            GameObject selected = EventSystem.current?.currentSelectedGameObject;
            bool selectedWillHide = false;
            bool suiteEnabled = EnhancementsSettings.Enabled;
            var categoryOption = panel.GetComponentInChildren<OptionsCategoryOption>(true);
            if (categoryOption != null)
            {
                if (!suiteEnabled && selected != null &&
                    (selected == categoryOption.gameObject ||
                     selected.transform.IsChildOf(categoryOption.transform)))
                    selectedWillHide = true;
                categoryOption.gameObject.SetActive(suiteEnabled);
            }
            OptionsCategoryMember[] members =
                panel.GetComponentsInChildren<OptionsCategoryMember>(true);

            for (int index = 0; index < members.Length; index++)
            {
                OptionsCategoryMember member = members[index];
                bool visible = suiteEnabled ? OptionsCategoryVisibility.IsVisible(member.Category, SelectedCategory) :
                    member.GetComponentInChildren<ModInformation.Integration.ModInformationOption>(true)?.IsSupportAction == true;

                if (!visible && selected != null &&
                    (selected == member.gameObject ||
                     selected.transform.IsChildOf(member.transform)))
                {
                    selectedWillHide = true;
                }
                if (member.gameObject.activeSelf != visible)
                {
                    member.gameObject.SetActive(visible);
                }
            }

            if (content != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(content);
                Canvas.ForceUpdateCanvases();
                // Rows below the category selector change the content height. Keep
                // the current viewport anchor so the selected row does not jump.
                content.anchoredPosition = anchoredPosition;
            }
            OptionsPanelPatch.WireNavigation(panel, template);

            if (selectedWillHide && EventSystem.current != null)
            {
                var target = suiteEnabled ? categoryBox
                    : panel.GetComponentInChildren<MasterEnabledOption>(true)?.Box;
                if (target != null && target.gameObject.activeInHierarchy)
                    EventSystem.current.SetSelectedGameObject(target.gameObject);
            }

        }
    }

    internal sealed class OptionsCategoryOption : MonoBehaviour
    {
        private UI_HorizontalSelectionBox box;
        private UI_LocalizationStringText valueText;
        private OptionsCategoryController controller;

        internal void Configure(UI_HorizontalSelectionBox selectionBox,
            UI_LocalizationStringText text, OptionsCategoryController owner)
        {
            box = selectionBox;
            valueText = text;
            controller = owner;
            controller?.RegisterCategoryBox(box);
        }

        private void OnEnable()
        {
            if (box == null || controller == null) return;
            box.numberOfElements = OptionsCategoryLocalization.CategoryKeys.Length;
            box.overflowType = UI_HorizontalSelectionBox.OverflowType.Repeat;
            box.OnValueChanged += Changed;
            int value = (int)controller.SelectedCategory;
            box.ChangeValueWithoutNotify(value);
            valueText?.UpdateKey(
                OptionsCategoryLocalization.CategoryKeys[value]);
        }

        private void OnDisable()
        {
            if (box != null) box.OnValueChanged -= Changed;
        }

        private void Changed(int value)
        {
            OptionsCategory category = value >= 0 &&
                value < OptionsCategoryLocalization.CategoryKeys.Length
                ? (OptionsCategory)value : OptionsCategory.General;
            valueText?.UpdateKey(
                OptionsCategoryLocalization.CategoryKeys[(int)category]);
            controller?.SelectCategory(category);
        }
    }

    internal sealed class MasterEnabledOption : MonoBehaviour
    {
        private UI_HorizontalSelectionBox box;
        private UI_LocalizationStringText valueText;

        internal UI_HorizontalSelectionBox Box => box;

        internal void Configure(UI_HorizontalSelectionBox selectionBox,
            UI_LocalizationStringText text)
        {
            box = selectionBox;
            valueText = text;
        }

        private void OnEnable()
        {
            if (box == null) return;
            box.numberOfElements = 2;
            box.overflowType = UI_HorizontalSelectionBox.OverflowType.Repeat;
            box.OnValueChanged += Changed;
            int value = EnhancementsSettings.Enabled ? 1 : 0;
            box.ChangeValueWithoutNotify(value);
            valueText?.UpdateKey(value == 1
                ? ModLocalization.SuiteOn : ModLocalization.SuiteOff);
        }

        private void OnDisable()
        {
            if (box != null) box.OnValueChanged -= Changed;
        }

        private void Changed(int value)
        {
            if (!NativeSettingsInteraction.CanEdit(SettingInteractionKind.Master))
            {
                box.ChangeValueWithoutNotify(EnhancementsSettings.Enabled ? 1 : 0);
                return;
            }
            EnhancementsSettings.Enabled = value == 1;
            EnhancementsSettings.Save();
            CombatVisualRuntime.RefreshCompanionBodies();
            valueText?.UpdateKey(value == 1
                ? ModLocalization.SuiteOn : ModLocalization.SuiteOff);
            GetComponentInParent<OptionsCategoryController>()?.RefreshVisibility();
        }
    }

}
