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
            get { return "Save and restore tax values (ver. 2019/9/16)"; }
        }
    }
}
