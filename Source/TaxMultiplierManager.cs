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

        public bool IsTaxMultiplierDisabled = false;
        private int counter = 0;
        private const string taxMultiplierLabelName = "taxMultiplierLabel";
        private const string disableTaxMultiplierCheckboxName = "disableTaxMultiplierCheckbox";

        public void AddControls()
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
                        taxMultiplierLabel.position = new Vector3(10, -taxesPanel.height + 70);
                        taxMultiplierLabel.text = "";

                        taxesPanel.eventVisibilityChanged += delegate (UIComponent component, bool value)
                        {
                            if (value)
                            {
                                //counter = 100;
                                //Debug.Log(string.Format("Tax multiplier: {0:0.000}", getTaxMultiplier() * 0.0001));
                                taxMultiplierLabel.text = string.Format("Tax multiplier: {0:0.000}", getTaxMultiplier() * 0.0001);
                            }
                        };

                        //UICheckBox disableTaxMultiplierChkbox = taxesPanel.AddUIComponent<UICheckBox>();
                        //disableTaxMultiplierChkbox.name = disableTaxMultiplierChkboxName;
                        //disableTaxMultiplierChkbox.position = new Vector3(10, -taxesPanel.height + 65);
                        //disableTaxMultiplierChkbox.size = new Vector2(40, 20);

                        //UISprite sprite = disableTaxMultiplierChkbox.AddUIComponent<UISprite>();
                        //sprite.spriteName = "ToggleBase";
                        //sprite.size = new Vector2(16f, 16f);
                        //sprite.relativePosition = new Vector3(2f, 2f);

                        //disableTaxMultiplierChkbox.checkedBoxObject = sprite.AddUIComponent<UISprite>();
                        //((UISprite)disableTaxMultiplierChkbox.checkedBoxObject).spriteName = "ToggleBaseFocused";
                        //disableTaxMultiplierChkbox.checkedBoxObject.size = new Vector2(16f, 16f);
                        //disableTaxMultiplierChkbox.checkedBoxObject.relativePosition = Vector3.zero;

                        //UILabel label = disableTaxMultiplierChkbox.AddUIComponent<UILabel>();
                        //label.relativePosition = new Vector3(22f, 2f);
                        //label.text = "Off";
                        //disableTaxMultiplierChkbox.label = label;

                        //disableTaxMultiplierChkbox.eventCheckChanged += delegate (UIComponent component, bool value)
                        //{
                        //    IsTaxMultiplierDisabled = value;

                        //    if (value)
                        //    {
                        //        setTaxMultiplier(10000);
                        //    }
                        //    else
                        //    {
                        //        setTaxMultiplier(calculateTaxMultiplier());
                        //    }

                        //    counter = 100;
                        //    updateTaxMultiplierLabel();
                        //};
                    }
                }
            }
        }

        private int getTaxMultiplier()
        {
            EconomyManager em = Singleton<EconomyManager>.instance;
            FieldInfo field = em.GetType().GetField("m_taxMultiplier", BindingFlags.NonPublic | BindingFlags.Instance);
            return (int)field.GetValue(em);
        }

        private void setTaxMultiplier(int value)
        {
            EconomyManager em = Singleton<EconomyManager>.instance;
            FieldInfo field = em.GetType().GetField("m_taxMultiplier", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(em, value);
        }

        public void OnBeforeSimulationFrame()
        {
            if (IsTaxMultiplierDisabled)
            {
                setTaxMultiplier(10000);
            }
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
            UITabContainer economyContainer = ToolsModifierControl.economyPanel.component.Find<UITabContainer>("EconomyContainer");
            if (economyContainer != null)
            {
                UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");
                if (taxesPanel != null && taxesPanel.isVisible)
                {
                    UILabel taxMultiplierLabel = taxesPanel.Find<UILabel>(taxMultiplierLabelName);
                    if (taxMultiplierLabel != null)
                    {
                        taxMultiplierLabel.text = string.Format("Tax multiplier: {0:0.000}", getTaxMultiplier() * 0.0001);
                    }
                }
            }
        }

        private UILabel findTaxMultiplierLabel()
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

        // Copy from EconomyManager.SimulationStepImpl()
        private int calculateTaxMultiplier()
        {
            EconomyManager em = Singleton<EconomyManager>.instance;

            FieldInfo field;
            field = em.GetType().GetField("m_lastCashDelta", BindingFlags.NonPublic | BindingFlags.Instance);
            long m_lastCashDelta = (long)field.GetValue(em);

            field = em.GetType().GetField("m_cashAmount", BindingFlags.NonPublic | BindingFlags.Instance);
            long m_cashAmount = (long)field.GetValue(em);

            field = em.GetType().GetField("m_totalExpenses", BindingFlags.NonPublic | BindingFlags.Instance);
            long[] m_totalExpenses = (long[])field.GetValue(em);

            long totalExpenses = 0L;
            for (int l = 0; l < 16; l++)
            {
                totalExpenses += m_totalExpenses[l];
            }
            long result;
            if (totalExpenses != 0L)
            {
                long num17 = 200000L;
                long num18 = (m_lastCashDelta - num17) * 20000L / totalExpenses;
                if (num18 > 0L)
                {
                    result = 5000L + 50000000L / (num18 + 10000L);
                }
                else if (num18 < 0L)
                {
                    result = 20000L + 200000000L / (num18 - 20000L);
                }
                else
                {
                    result = 10000L;
                }
                long num20 = m_cashAmount;
                long num21 = (long)Singleton<DistrictManager>.instance.m_districts.m_buffer[0].m_populationData.m_finalCount;
                if (num20 < 0L)
                {
                    num20 = 0L;
                }
                if (num21 < 0L)
                {
                    num21 = 0L;
                }
                result = result * 1000000000L / (1000000000L + num20);
                result = result * 1000000L / (1000000L + num21);
                if (result < 100L)
                {
                    result = 100L;
                }
                else if (result > 30000L)
                {
                    result = 30000L;
                }
            }
            else
            {
                result = 10000L;
            }

            return (int)result;
        }
    }
}
