using Sirenix.OdinInspector;
using UI.TabsSystem;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using Utils;

namespace UI.Menu.InventoryMenu
{
    public class ExtraTabController : CurrentInstanced<ExtraTabController>
    {
        [SerializeField, Required] 
        private GameObject extraTabButton;

        [SerializeField, Required] 
        private LocalizeStringEvent extraTabButtonTitle;
        
        [SerializeField, Required] 
        private TabManager tabManager;

        private GameObject _enabledTabContent;

        private void Start()
        {
            extraTabButton.SetActive(false);
        }

        public void EnableExtraTab(LocalizedString tabTitle, GameObject tabContent, bool switchToTab = false)
        {
            extraTabButton.SetActive(true);
            extraTabButtonTitle.StringReference = tabTitle;
            extraTabButtonTitle.RefreshString();

            if (_enabledTabContent != null)
            {
                _enabledTabContent.SetActive(false);
            }
            tabContent.SetActive(true);
            _enabledTabContent = tabContent;
            
            if (switchToTab)
            {
                tabManager.ClickedTab(2);
            }
        }

        public void DisableExtraTab()
        {
            if (tabManager.SelectedTab == 2)
            {
                tabManager.SwitchTabNoAnimation(0);
            }
            
            extraTabButton.SetActive(false);
        }
    }
}