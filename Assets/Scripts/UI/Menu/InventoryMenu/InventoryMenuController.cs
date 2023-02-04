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
            UIManager.Current.OnUIStatusChanged += UpdateStatus;
        }
        
        private void OnDisable()
        {
            UIManager.Current.OnUIStatusChanged -= UpdateStatus;
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
            UIManager.ToggleUI(menu);
        }

        private void UpdateStatus(CanvasGroup canvasGroup, bool closed)
        {
            if (!closed) return;
            if (canvasGroup != menu) return;
            InventoryTabController.Current.SetOpenedInventory(null);
        }
    }
}