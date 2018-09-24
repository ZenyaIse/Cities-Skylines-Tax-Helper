using ICities;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace TaxHelperMod
{
    public class LoadingExtension : LoadingExtensionBase
    {
        private bool taxControlsAlreadyAdded = false;

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame || mode == LoadMode.NewGameFromScenario)
            {
                Singleton<TaxMultiplierManager>.instance.AddTaxMultiplierLabel();
                addTaxControls();
            }
        }

        private void addTaxControls()
        {
            if (taxControlsAlreadyAdded)
            {
                return;
            }
            else
            {
                taxControlsAlreadyAdded = true;

                EconomyPanel ep = ToolsModifierControl.economyPanel;
                UITabContainer economyContainer = ep.component.Find<UITabContainer>("EconomyContainer");
                UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");

                UITaxSetPanel taxSetPanel1 = taxesPanel.AddUIComponent<UITaxSetPanel>();
                taxSetPanel1.position = new Vector3(10, -40);
                taxSetPanel1.SetTaxValues(new int[6] { 29, 29, 29, 29, 29, 29 });

                UITaxSetPanel taxSetPanel2 = taxesPanel.AddUIComponent<UITaxSetPanel>();
                taxSetPanel2.position = new Vector3(10, -140);
                taxSetPanel2.SetTaxValues(new int[6] { 13, 13, 13, 13, 13, 13 });

                UITaxSetPanel taxSetPanel3 = taxesPanel.AddUIComponent<UITaxSetPanel>();
                taxSetPanel3.position = new Vector3(10, -240);
                taxSetPanel3.SetTaxValues(new int[6] { 9, 9, 9, 9, 9, 9 });
            }
        }
    }
}
