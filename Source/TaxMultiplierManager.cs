using System.Reflection;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace TaxHelperMod
{
    public class TaxMultiplierManager : Singleton<TaxMultiplierManager>
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

        public bool TaxMultiplierOff = false;
        private bool taxMultiplierLabelAlreadyAdded = false;
        private int counter = 0;
        private UILabel taxMultiplierLabel;

        public void AddTaxMultiplierLabel()
        {
            if (taxMultiplierLabelAlreadyAdded)
            {
                return;
            }
            else
            {
                taxMultiplierLabelAlreadyAdded = true;

                EconomyPanel ep = ToolsModifierControl.economyPanel;
                UIComponent incomesExpensesPanel = ep.component.Find("IncomesExpensesPanel");
                UISlicedSprite titleBar = (UISlicedSprite)incomesExpensesPanel.Find("TitleBar");

                taxMultiplierLabel = titleBar.AddUIComponent<UILabel>();
                taxMultiplierLabel.position = new Vector3(titleBar.width - 200, -6);

                ep.component.eventVisibilityChanged += delegate (UIComponent component, bool value)
                {
                    if (value)
                    {
                        updateTaxMultiplierLabel();
                    }
                };
            }
        }

        private float getTaxMultiplier()
        {
            EconomyManager em = Singleton<EconomyManager>.instance;
            FieldInfo field = em.GetType().GetField("m_taxMultiplier", BindingFlags.NonPublic | BindingFlags.Instance);
            return ((int)field.GetValue(em)) * 0.0001f;
        }

        public void OnAfterSimulationFrame()
        {
            if (!ToolsModifierControl.economyPanel.component.isVisible) return;

            if (counter-- > 0) return;

            counter = 100;

            updateTaxMultiplierLabel();
        }

        private void updateTaxMultiplierLabel()
        {
            if (taxMultiplierLabel != null)
            {
                taxMultiplierLabel.text = string.Format("Tax multiplier: {0:0.000}", getTaxMultiplier());
            }
        }
    }
}
