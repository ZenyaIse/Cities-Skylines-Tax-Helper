using ICities;
using ColossalFramework;

namespace TaxHelperMod
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame || mode == LoadMode.NewGameFromScenario)
            {
                Singleton<TaxMultiplierManager>.instance.AddTaxMultiplierLabel();
                UITaxSetPanel.AddTaxControls();
            }
        }
    }
}
