using Items;
using UI.Menu.EscapeMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Player.Interaction
{
    public interface IReleasedBehaviourItem
    {
        void Release(MouseButton mouseButton, ref ItemStack itemStack, TilemapManager tilemapManager,
            InventoryUIController inventoryUIController,
            EquipmentsController equipmentsController, Vector3 playerPosition);
    }
}