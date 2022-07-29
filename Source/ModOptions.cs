using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace TaxHelperMod
{
    public class ModOptions
    {
        private static ModOptions instance;

        public static ModOptions Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = CreateFromFile();

                    if (instance == null)
                    {
                        instance = new ModOptions();
                    }
                }

                return instance;
            }
        }

        public bool IsShowTaxMultiplierPanel = false;

        private const string optionsFileName = "TaxHelperModOptions.xml";

        public void Save()
        {
            XmlSerializer ser = new XmlSerializer(typeof(ModOptions));
            try
            {
                TextWriter writer = new StreamWriter(getOptionsFilePath());
                ser.Serialize(writer, this);
                writer.Close();
                Debug.Log("TaxHelperMod: Options file is saved.");
            }
            catch
            {
                Debug.Log("TaxHelperMod: Could not write options file.");
            }
        }

        public static ModOptions CreateFromFile()
        {
            string path = getOptionsFilePath();

            if (!File.Exists(path)) return null;

            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(ModOptions));
                TextReader reader = new StreamReader(path);
                ModOptions instance = (ModOptions)ser.Deserialize(reader);
                reader.Close();

                return instance;
            }
            catch
            {
                Debug.Log("TaxHelperMod: Error reading options file.");
                return null;
            }
        }

        private static string getOptionsFilePath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Colossal Order\\Cities_Skylines\\" + optionsFileName;
        }
    }
}
