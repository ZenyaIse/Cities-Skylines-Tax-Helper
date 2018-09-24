using ColossalFramework.UI;
using UnityEngine;

namespace TaxHelperMod
{
    public class UITaxSetPanel : UIPanel
    {
        //UITabContainer economyContainer = ep.component.Find<UITabContainer>("EconomyContainer");
        //Taxes (ColossalFramework.UI.UIPanel)
        //Budget(ColossalFramework.UI.UIPanel)
        //Loans(ColossalFramework.UI.UIPanel)

        //UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");
        //TaxesItemContainer (ColossalFramework.UI.UIPanel)
        //UIButton(ColossalFramework.UI.UIButton)

        //UIPanel taxesItemContainer = taxesPanel.Find<UIPanel>("TaxesItemContainer");
        //ResidentialLow (ColossalFramework.UI.UIPanel)
        //ResidentialHigh(ColossalFramework.UI.UIPanel)
        //CommercialLow(ColossalFramework.UI.UIPanel)
        //CommercialHigh(ColossalFramework.UI.UIPanel)
        //Industrial(ColossalFramework.UI.UIPanel)
        //Office(ColossalFramework.UI.UIPanel)
        //UIButton(ColossalFramework.UI.UIButton)

        //UIComponent taxesItem = taxesItemContainer.components[i];
        //UISlider slider = taxesItem.Find<UISlider>("Slider");
        //Sprite (ColossalFramework.UI.UISprite)
        //Icon(ColossalFramework.UI.UISprite)
        //Slider(ColossalFramework.UI.UISlider)
        //Percentage(ColossalFramework.UI.UILabel)
        //Singleton<EconomyManager>.instance.Tax

        //StringBuilder sb = new StringBuilder("NoTaxMultiplierMod:\n");
        //foreach (UIComponent c in residentialLow.components)
        //{
        //    sb.AppendLine(c.ToString());
        //}
        //Debug.Log(sb.ToString());

        private UIButton rememberBtn;
        private UIButton applyBtn;
        public int TaxValuesStorageIndex = 0;

        public override void Awake()
        {
            base.Awake();

            this.size = new Vector2(150, 80);
            this.m_AutoLayout = false;
        }

        public override void Start()
        {
            base.Start();

            applyBtn = AddUIComponent<UIButton>();
            applyBtn.name = "rememberBtn";
            applyBtn.position = new Vector3(0, 0);
            applyBtn.size = new Vector2(70, 70);
            applyBtn.textColor = Color.white;
            applyBtn.normalBgSprite = "ButtonMenu";
            applyBtn.hoveredBgSprite = "ButtonMenuHovered";
            applyBtn.eventClick += ApplyBtn_eventClick;
            updateApplyBtnText();

            rememberBtn = AddUIComponent<UIButton>();
            rememberBtn.name = "applyBtn";
            rememberBtn.position = new Vector3(80, 0);
            rememberBtn.size = new Vector2(40, 70);
            rememberBtn.textColor = Color.white;
            rememberBtn.text = "<<\n<<\n<<";
            rememberBtn.normalBgSprite = "ButtonMenu";
            rememberBtn.hoveredBgSprite = "ButtonMenuHovered";
            rememberBtn.eventClick += RememberBtn_eventClick;
        }

        private void RememberBtn_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            UIComponent taxesItemContainer = ToolsModifierControl.economyPanel.component.Find("TaxesItemContainer");
            for (int i = 0; i < 6; i++)
            {
                UIComponent taxesItem = taxesItemContainer.components[i];
                UISlider slider = taxesItem.Find<UISlider>("Slider");
                SavedTaxValues.taxValues[TaxValuesStorageIndex][i] = (int)slider.value;
            }

            updateApplyBtnText();
        }

        private void ApplyBtn_eventClick(UIComponent component, UIMouseEventParameter eventParam)
        {
            UIComponent taxesItemContainer = ToolsModifierControl.economyPanel.component.Find("TaxesItemContainer");
            for (int i = 0; i < 6; i++)
            {
                UIComponent taxesItem = taxesItemContainer.components[i];
                if (taxesItem.isEnabled)
                {
                    UISlider slider = taxesItem.Find<UISlider>("Slider");
                    slider.value = SavedTaxValues.taxValues[TaxValuesStorageIndex][i];
                }
            }
        }

        private void updateApplyBtnText()
        {
            if (applyBtn != null)
            {
                int[] t = SavedTaxValues.taxValues[TaxValuesStorageIndex];
                applyBtn.text = string.Format("{0}  {1}\n{2}  {3}\n{4}  {5}", t[0], t[1], t[2], t[3], t[4], t[5]);
            }
        }

        #region Static

        private static bool taxControlsAlreadyAdded = false;

        public static void AddTaxControls()
        {
            if (taxControlsAlreadyAdded)
            {
                return;
            }
            else
            {
                taxControlsAlreadyAdded = true;

                EconomyPanel ep = ToolsModifierControl.economyPanel;
                UITabContainer economyContainer = ep.component.Find<UITabContainer>("EconomyContainer");
                UIPanel taxesPanel = economyContainer.Find<UIPanel>("Taxes");

                UITaxSetPanel taxSetPanel1 = taxesPanel.AddUIComponent<UITaxSetPanel>();
                taxSetPanel1.position = new Vector3(10, -40);
                taxSetPanel1.TaxValuesStorageIndex = 0;

                UITaxSetPanel taxSetPanel2 = taxesPanel.AddUIComponent<UITaxSetPanel>();
                taxSetPanel2.position = new Vector3(10, -140);
                taxSetPanel2.TaxValuesStorageIndex = 1;

                UITaxSetPanel taxSetPanel3 = taxesPanel.AddUIComponent<UITaxSetPanel>();
                taxSetPanel3.position = new Vector3(10, -240);
                taxSetPanel3.TaxValuesStorageIndex = 2;
            }
        }

        #endregion
    }
}
