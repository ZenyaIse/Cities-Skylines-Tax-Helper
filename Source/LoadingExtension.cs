using ColossalFramework;
using ICities;

namespace TaxHelperMod
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            if (mode == LoadMode.NewGame || mode == LoadMode.LoadGame || mode == LoadMode.NewGameFromScenario)
            {
                UITaxSetPanel.AddTaxControls();
                Singleton<TaxMultiplierManager>.instance.Init();
            }
        }
    }
}
