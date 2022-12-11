using Player.Interaction;
using Sirenix.OdinInspector;
using Tiles;
using UI.Menu.InventoryMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Items.ItemTypes
{
    /// <summary>
    /// A type of item that can be placed as a tile when using
    /// </summary>
    [CreateAssetMenu(fileName = "New Placeable Item", menuName = "Items/New Placeable Item")]
    public class PlaceableItem : TriItem, IHoldBehaviourItem
    {
        [BoxGroup(GeneralInformationBox)] [VerticalGroup(VerticalMain)]
        public PlaceableTile placeableTile;

        public void Hold(MouseButton mouseButton, ref ItemStack itemStack, TileInstance tileClicked, Vector3 clickedPos, 
            Vector3Int pos, TilemapManager tilemapManager, InventoryController inventoryController,
            EquipmentsController equipmentController, Vector3 playerPosition)
        {
            if (mouseButton != MouseButton.Right) return;
            if (tileClicked is not null) return;
            if (placeableTile.hasCollider && !(Vector3.Distance(playerPosition, clickedPos) > 1)) return;
            if (!tilemapManager.TryPlaceTile(placeableTile, pos, TilemapLayer.Obstacles)) return;

            // consume an item
            var item = equipmentController[EquipmentType.Outer];
            var itemCount = item.count - 1;
            equipmentController[EquipmentType.Outer] = itemCount <= 0 ? ItemStack.Empty : item.Copy(count: itemCount);
        }

        public bool CanInteract(ref ItemStack itemStack, TileInstance tileAtLocation, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryController inventoryController,
            EquipmentsController equipmentController, Vector3 playerPosition)
        {
            if (tileAtLocation is not null) return false;
            var canPlace = placeableTile.CanPlace(pos, tilemapManager);
            if (!placeableTile.hasCollider) return canPlace;

            return
                /* is the player inside the block they are about to place? */
                Vector3.Distance(playerPosition, clickedPos) > 1 &&
                /* custom placement predicate */
                canPlace;
        }
    }
}