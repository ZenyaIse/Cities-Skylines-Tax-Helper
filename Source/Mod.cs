using ICities;

namespace TaxHelperMod
{
    public class Mod : IUserMod
    {
        public string Name
        {
            get { return "Taxes helper mod"; }
        }

        public string Description
        {
            get { return "Save and restore tax values (ver. 2022/7/30)"; }
        }

        #region Options UI

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddCheckbox("Show Tax Multiplier controls (advanced)", ModOptions.Instance.IsShowTaxMultiplierPanel, delegate (bool isChecked)
            {
                ModOptions.Instance.IsShowTaxMultiplierPanel = isChecked;
                TaxMultiplierManager.instance.RefreshTaxMultiplierPanelState();
                ModOptions.Instance.Save();
            });
        }

        #endregion
    }
}
