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

        public static bool TaxMultiplierOff = false;
        private static int counter = 0;
        private const string taxMultiplierLabelName = "taxMultiplierLabel";

        public static void AddTaxMultiplierLabel()
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
                        taxMultiplierLabel.position = new Vector3(10, -taxesPanel.height + 60);

                        taxesPanel.eventVisibilityChanged += delegate (UIComponent component, bool value)
                        {
                            if (value)
                            {
                                counter = 100;
                                updateTaxMultiplierLabel();
                            }
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
