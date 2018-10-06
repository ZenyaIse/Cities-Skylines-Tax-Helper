using ColossalFramework.IO;

namespace TaxHelperMod
{
    public static class SavedTaxValues
    {
        public class Data : IDataContainer
        {
            public void Serialize(DataSerializer s)
            {
                s.WriteBool(TaxMultiplierManager.IsTaxMultiplierDisabled);

                for (int i = 0; i < taxValues.Length; i++)
                {
                    s.WriteInt32Array(taxValues[i]);
                }
            }

            public void Deserialize(DataSerializer s)
            {
                TaxMultiplierManager.IsTaxMultiplierDisabled = s.ReadBool();

                for (int i = 0; i < taxValues.Length; i++)
                {
                    taxValues[i] = s.ReadInt32Array();
                }
            }

            public void AfterDeserialize(DataSerializer s)
            {
                UnityEngine.Debug.Log("TaxHelperMod data loaded");
            }
        }

        public static int[][] taxValues = {
            new int[6] { 29, 29, 29, 29, 29, 29 },
            new int[6] { 13, 13, 13, 13, 13, 13 },
            new int[6] { 9, 9, 9, 9, 9, 9 },
        };
    }
}
