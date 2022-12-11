using UI.Menu.EscapeMenu;
using UI.Menu.InventoryMenu;
using UnityEngine;

namespace Utils
{
    public static class GameInput
    {
        public static bool LeftClickButton()
        {
            return Input.GetMouseButton(0) && CanInput();
        }

        public static bool RightClickButton()
        {
            return Input.GetMouseButton(1) && CanInput();
        }
        
        public static bool LeftClickButtonDown()
        {
            return Input.GetMouseButtonDown(0) && CanInput();
        }

        public static bool RightClickButtonDown()
        {
            return Input.GetMouseButtonDown(1) && CanInput();
        }

        public static bool KeyboardKey(KeyCode keyCode)
        {
            return Input.GetKey(keyCode) && CanInput();
        }

        public static bool KeyboardKeyDown(KeyCode keyCode)
        {
            return Input.GetKeyDown(keyCode) && CanInput();
        }

        public static float GetAxis(string axis)
        {
            return CanInput() ? Input.GetAxis(axis) : 0;
        }

        private static bool CanInput()
        {
            return (InventoryMenuController.Current is null || !InventoryMenuController.Current.IsActive)
                   && (EscapeMenuController.Current is null || !EscapeMenuController.Current.IsActive);
        }
    }
}