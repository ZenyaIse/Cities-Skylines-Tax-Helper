using System.IO;
using ColossalFramework.IO;
using ICities;
using UnityEngine;

namespace TaxHelperMod
{
    public class SerializableDataExtension : ISerializableDataExtension
    {
        public const string DataID = "TaxHelperMod";
        public const uint DataVersion = 0;
        private ISerializableData serializedData;

        public void OnCreated(ISerializableData serializedData)
        {
            this.serializedData = serializedData;
        }

        public void OnReleased()
        {
            serializedData = null;
        }

        public void OnLoadData()
        {
            byte[] data = serializedData.LoadData(DataID);

            if (data == null)
            {
                Debug.Log("TaxHelperMod >>> No saved data ");
                return;
            }

            using (var stream = new MemoryStream(data))
            {
                DataSerializer.Deserialize<SavedTaxValues.Data>(stream, DataSerializer.Mode.Memory);
            }
        }

        public void OnSaveData()
        {
            byte[] data;

            using (var stream = new MemoryStream())
            {
                DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new SavedTaxValues.Data());
                data = stream.ToArray();
            }

            serializedData.SaveData(DataID, data);
        }
    }
}
