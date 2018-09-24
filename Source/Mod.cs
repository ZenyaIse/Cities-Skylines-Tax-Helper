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
            get { return "Taxes helper mod."; }
        }


        #region Options UI
        
        public void OnSettingsUI(UIHelperBase helper)
        {
        }

        #endregion
    }
}
