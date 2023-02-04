using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Systems.Inv;
using UI.TabsSystem;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using Utils;

namespace UI.Menu.InventoryMenu
{
    public class ExtraTabController : CurrentInstanced<ExtraTabController>
    {
        [SerializeField, Required] private GameObject extraTabButton;
        [SerializeField, Required] private LocalizeStringEvent extraTabButtonTitle;
        [SerializeField, Required] private TabManager tabManager;
        [SerializeField, Required] private Transform extraTabsArea;

        private bool _isCurrentContentPrefab;
        private GameObject _enabledTabContent;
        public GameObject EnabledTabContent => _enabledTabContent;

        private Inventory _openedInventory;
        public Inventory OpenedInventory => _openedInventory;
        
        private void Start()
        {
            extraTabButton.SetActive(false);

            foreach (Transform child in extraTabsArea)
            {
                child.gameObject.SetActive(false);
            }
        }

        [CanBeNull]
        public GameObject EnableExtraTab(LocalizedString tabTitle, GameObject tabContent, bool isContentPrefab, bool switchToTab = false, Inventory openedInventory = null)
        {
            if (tabContent == _enabledTabContent)
            {
                DisableExtraTab();
                return null;
            }

            _openedInventory = openedInventory;
            extraTabButton.SetActive(true);
            extraTabButtonTitle.StringReference = tabTitle;
            extraTabButtonTitle.RefreshString();

            if (_enabledTabContent != null)
            {
                DisableExtraTab();
            }

            var inventoryMenuController = InventoryMenuController.Current;
            if (!inventoryMenuController.IsActive)
            {
                inventoryMenuController.ToggleMenu();
            }

            if (switchToTab)
            {
                tabManager.ClickedTab(2);
            }

            _isCurrentContentPrefab = isContentPrefab;
            if (!_isCurrentContentPrefab)
            {
                _enabledTabContent = tabContent;
                tabContent.SetActive(true);
                return tabContent;
            }

            var instantiated = Instantiate(tabContent, extraTabsArea);
            _enabledTabContent = instantiated;

            return instantiated;
        }

        public void DisableExtraTab()
        {
            if (tabManager.SelectedTab == 2)
            {
                tabManager.SwitchTabNoAnimation(0);
            }

            _openedInventory = null;
            extraTabButton.SetActive(false);
            
            if (_enabledTabContent != null)
            {
                if (_isCurrentContentPrefab)
                {
                    Destroy(_enabledTabContent);
                }
                else
                {
                    _enabledTabContent.SetActive(false);
                }

                _enabledTabContent = null;
            }
        }
    }
}