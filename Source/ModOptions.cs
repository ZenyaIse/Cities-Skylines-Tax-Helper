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
                    instance = new ModOptions();
                }

                return instance;
            }
        }

        public bool IsShowTaxMultiplierPanel = false;
    }
}
