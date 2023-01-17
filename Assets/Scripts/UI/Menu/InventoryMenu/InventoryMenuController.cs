using Sirenix.OdinInspector;
using UI.Managers;
using UnityEngine;
using Utils;

namespace UI.Menu.InventoryMenu
{
    public class InventoryMenuController : CurrentInstanced<InventoryMenuController>
    {
        [SerializeField, Required]
        private CanvasGroup menu;
        
        public bool IsActive => menu.gameObject.activeSelf;

        private void Start()
        {
            menu.Disable();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleMenu();
            }
        }

        public void ToggleMenu()
        {
            if (UIManager.ToggleUI(menu))
            {
                InventoryTabController.Current.SetOpenedInventory(null);
            }
        }
    }
}