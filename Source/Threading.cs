using ColossalFramework;
using ICities;
using UnityEngine;

namespace TaxHelperMod
{
    public class Threading : ThreadingExtensionBase
    {
        public override void OnBeforeSimulationFrame()
        {
            Singleton<TaxMultiplierManager>.instance.OnBeforeSimulationFrame();
        }
    }
}
