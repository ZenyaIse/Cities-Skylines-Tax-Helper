using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace TaxHelperMod
{
    public class TaxMultiplierPanel : UIPanel
    {
        private UILabel label;
        private UICheckBox checkBox;
        private UITextField textField;

        public override void Awake()
        {
            base.Awake();

            //this.backgroundSprite = "GenericPanel";
            this.backgroundSprite = "MenuPanel";
            //this.backgroundSprite = "ProgressBarFill";
            //this.color = new Color32(255, 0, 0, 100);
            this.canFocus = true;

            height = 32;
            width = 560;

            isVisible = true;
        }

        public override void Start()
        {
            base.Start();

            float y = -8;

            label = this.AddUIComponent<UILabel>();
            label.position = new Vector3(10, y);

            checkBox = UIHelper.CreateCheckBox(this, "Override with value: ");
            checkBox.position = new Vector3(240, y);
            checkBox.eventCheckChanged += CheckBox_eventCheckChanged;

            textField = UIHelper.CreateTextField(this);
            textField.position = new Vector3(460, y + 2);
            textField.text = "1.000";
            textField.eventTextSubmitted += TextField_eventTextSubmitted;
        }

        private void TextField_eventTextSubmitted(UIComponent component, string value)
        {
            float parsedValue;
            if (float.TryParse(value, out parsedValue))
            {
                Singleton<TaxMultiplierManager>.instance.TaxMultiplierUserValue = (int)(parsedValue * 10000);
            }
        }

        private void CheckBox_eventCheckChanged(UIComponent component, bool value)
        {
            Singleton<TaxMultiplierManager>.instance.IsTaxMultiplierOverrideEnabled = value;
        }

        public override void Update()
        {
            base.Update();

            int m = Singleton<TaxMultiplierManager>.instance.GetTaxMultiplier();
            label.text = string.Format("Tax multiplier: {0:0.000}", m * 0.0001);
        }
    }
}
