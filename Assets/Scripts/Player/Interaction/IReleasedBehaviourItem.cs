using Items;
using UI.Menu.InventoryMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Player.Interaction
{
    public interface IReleasedBehaviourItem
    {
        void Release(MouseButton mouseButton, ref ItemStack itemStack, TilemapManager tilemapManager,
            InventoryTabController inventoryTabController,
            EquipmentsController equipmentsController, Vector3 playerPosition);
    }
}