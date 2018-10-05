using ICities;

namespace TaxHelperMod
{
    public class Threading : ThreadingExtensionBase
    {
        public override void OnAfterSimulationFrame()
        {
            TaxMultiplierManager.OnAfterSimulationFrame();
        }
    }
}
