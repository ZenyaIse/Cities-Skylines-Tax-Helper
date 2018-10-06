using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace TaxHelperMod
{
    public static class TaxMultiplierManager
    {
        //EconomyPanel ep = ToolsModifierControl.economyPanel;
        //Title(ColossalFramework.UI.UILabel)
        //BackgroundImage(ColossalFramework.UI.UITextureSprite)
        //Close(ColossalFramework.UI.UIButton)
        //EconomyTabstrip(ColossalFramework.UI.UITabstrip)
        //EconomyContainer(ColossalFramework.UI.UITabContainer)
        //IncomesExpensesPanel(ColossalFramework.UI.UIPanel)
        //IncomeTooltipResidential(ColossalFramework.UI.UIPanel)
        //IncomeTooltipPublicTransport(ColossalFramework.UI.UIPanel)
        //IncomeTooltipCommercialTotal(ColossalFramework.UI.UIPanel)
        //IncomeTooltipOffice(ColossalFramework.UI.UIPanel)
        //IncomeTooltipIndustrial(ColossalFramework.UI.UIPanel)
        //Sprite(ColossalFramework.UI.UISprite)

        //UIComponent incomesExpensesPanel = ep.component.Find("IncomesExpensesPanel");
        //TitleBar(ColossalFramework.UI.UISlicedSprite)
        //Tabstrip(ColossalFramework.UI.UITabstrip)
        //TabContainer(ColossalFramework.UI.UITabContainer)

        public static bool IsTaxMultiplierDisabled = false;
        private static int counter = 0;
        private const string taxMultiplierLabelName = "taxMultiplierLabel";
        private const string disableTaxMultiplierChkboxName = "disableTaxMultiplierChkbox";

        public static void AddControls()
        {
            if (findTaxMultiplierLabel() == null)
            {
                UITabContainer economyContainer = ToolsModifierControl.economyPanel.component.Find<UITabContainer>("EconomyContainer");
                if (economyContainer != null)
                {
                    UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");
                    if (taxesPanel != null)
                    {
                        UILabel taxMultiplierLabel = taxesPanel.AddUIComponent<UILabel>();
                        taxMultiplierLabel.name = taxMultiplierLabelName;
                        taxMultiplierLabel.position = new Vector3(10, -taxesPanel.height + 85);

                        taxesPanel.eventVisibilityChanged += delegate (UIComponent component, bool value)
                        {
                            if (value)
                            {
                                counter = 100;
                                updateTaxMultiplierLabel();
                            }
                        };

                        UICheckBox disableTaxMultiplierChkbox = taxesPanel.AddUIComponent<UICheckBox>();
                        disableTaxMultiplierChkbox.name = disableTaxMultiplierChkboxName;
                        disableTaxMultiplierChkbox.position = new Vector3(10, -taxesPanel.height + 65);
                        disableTaxMultiplierChkbox.size = new Vector2(40, 20);

                        UISprite sprite = disableTaxMultiplierChkbox.AddUIComponent<UISprite>();
                        sprite.spriteName = "ToggleBase";
                        sprite.size = new Vector2(16f, 16f);
                        sprite.relativePosition = new Vector3(2f, 2f);

                        disableTaxMultiplierChkbox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
                        ((UISprite)disableTaxMultiplierChkbox.checkedBoxObject).spriteName = "ToggleBaseFocused";
                        disableTaxMultiplierChkbox.checkedBoxObject.size = new Vector2(16f, 16f);
                        disableTaxMultiplierChkbox.checkedBoxObject.relativePosition = Vector3.zero;

                        UILabel label = disableTaxMultiplierChkbox.AddUIComponent<UILabel>();
                        label.relativePosition = new Vector3(22f, 2f);
                        label.text = "Off";
                        disableTaxMultiplierChkbox.label = label;

                        disableTaxMultiplierChkbox.eventCheckChanged += delegate (UIComponent component, bool value)
                        {
                            IsTaxMultiplierDisabled = value;
                        };
                    }
                }
            }
        }

        private static float getTaxMultiplier()
        {
            EconomyManager em = Singleton<EconomyManager>.instance;
            FieldInfo field = em.GetType().GetField("m_taxMultiplier", BindingFlags.NonPublic | BindingFlags.Instance);
            return ((int)field.GetValue(em)) * 0.0001f;
        }

        private static void setTaxMultiplier(float value)
        {
            EconomyManager em = Singleton<EconomyManager>.instance;
            FieldInfo field = em.GetType().GetField("m_taxMultiplier", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(em, (int)(value * 10000));
        }

        public static void OnBeforeSimulationFrame()
        {
            if (IsTaxMultiplierDisabled)
            {
                setTaxMultiplier(1);
            }
        }
        
        public static void OnAfterSimulationFrame()
        {
            if (!ToolsModifierControl.economyPanel.component.isVisible) return;

            if (counter-- > 0) return;

            counter = 100;

            updateTaxMultiplierLabel();
        }

        private static void updateTaxMultiplierLabel()
        {
            UITabContainer economyContainer = ToolsModifierControl.economyPanel.component.Find<UITabContainer>("EconomyContainer");
            if (economyContainer != null)
            {
                UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");
                if (taxesPanel != null && taxesPanel.isVisible)
                {
                    UILabel taxMultiplierLabel = taxesPanel.Find<UILabel>(taxMultiplierLabelName);
                    if (taxMultiplierLabel != null)
                    {
                        taxMultiplierLabel.text = string.Format("Tax multiplier: {0:0.000}", getTaxMultiplier());
                    }
                }
            }
        }

        private static UILabel findTaxMultiplierLabel()
        {
            UITabContainer economyContainer = ToolsModifierControl.economyPanel.component.Find<UITabContainer>("EconomyContainer");
            if (economyContainer != null)
            {
                UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");
                if (taxesPanel != null)
                {
                    return taxesPanel.Find<UILabel>(taxMultiplierLabelName);
                }
            }

            return null;
        }
    }
}
