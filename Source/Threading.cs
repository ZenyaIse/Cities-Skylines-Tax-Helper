using ICities;
using ColossalFramework;

namespace TaxHelperMod
{
    public class Threading : ThreadingExtensionBase
    {
        public override void OnAfterSimulationFrame()
        {
            Singleton<TaxMultiplierManager>.instance.OnAfterSimulationFrame();
        }
    }
}
