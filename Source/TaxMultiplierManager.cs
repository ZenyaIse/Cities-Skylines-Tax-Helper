using ColossalFramework;
using ColossalFramework.UI;
using System.Reflection;
using UnityEngine;

namespace TaxHelperMod
{
    public class TaxMultiplierManager : Singleton<TaxMultiplierManager>
    {
        private bool _isTaxMultiplierOverrideEnabled = false;
        public int TaxMultiplierUserValue = 10000;

        public void RefreshTaxMultiplierPanelState()
        {
            UITabContainer economyContainer = ToolsModifierControl.economyPanel.component.Find<UITabContainer>("EconomyContainer");
            UIPanel taxesPanel = null;
            TaxMultiplierPanel taxMultiplierPanel = null;

            if (economyContainer != null)
            {
                taxesPanel = economyContainer.Find<UIPanel>("Taxes");
                if (taxesPanel != null)
                {
                    taxMultiplierPanel = taxesPanel.Find<TaxMultiplierPanel>("TaxMultiplierPanel");
                }
            }

            if (taxMultiplierPanel == null)
            {
                if (ModOptions.Instance.IsShowTaxMultiplierPanel)
                {
                    if (taxesPanel != null)
                    {
                        taxMultiplierPanel = taxesPanel.AddUIComponent<TaxMultiplierPanel>();
                        taxMultiplierPanel.name = "TaxMultiplierPanel";
                        taxMultiplierPanel.position = new Vector3(10, -475);
                    }
                }
            }
            else
            {
                if (!ModOptions.Instance.IsShowTaxMultiplierPanel)
                {
                    if (taxesPanel != null)
                    {
                        taxesPanel.RemoveUIComponent(taxMultiplierPanel);
                        taxMultiplierPanel = null;
                    }
                }
            }
        }

        public bool IsTaxMultiplierOverrideEnabled
        {
            get
            {
                return _isTaxMultiplierOverrideEnabled;
            }

            set
            {
                _isTaxMultiplierOverrideEnabled = value;

                if (!value)
                {
                    setTaxMultiplier(calculateVanillaTaxMultiplier());
                }
            }
        }

        private bool isTaxControlsNotCreated()
        {
            UITabContainer economyContainer = ToolsModifierControl.economyPanel.component.Find<UITabContainer>("EconomyContainer");
            if (economyContainer != null)
            {
                UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");
                if (taxesPanel != null)
                {
                    return taxesPanel.Find<UITaxSetPanel>("TaxMultiplierPanel") == null;
                }
            }

            return true;
        }

        public int GetTaxMultiplier()
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
            if (_isTaxMultiplierOverrideEnabled)
            {
                setTaxMultiplier(TaxMultiplierUserValue);
            }
        }

        // Copy from EconomyManager.SimulationStepImpl()
        private int calculateVanillaTaxMultiplier()
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
