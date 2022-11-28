using UI.Menu.EscapeMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Items.ItemTypes
{
    public interface IReleasedBehaviourItem
    {
        void Release(MouseButton mouseButton, ref ItemStack itemStack, TilemapManager tilemapManager,
            InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition);
    }
}