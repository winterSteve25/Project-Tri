using PlaceableTiles;
using Sirenix.OdinInspector;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;
using World;

namespace Items.ItemTypes
{
    /// <summary>
    /// A type of item that can be placed as a tile when using
    /// </summary>
    [CreateAssetMenu(fileName = "New Placeable Item", menuName = "Items/New Placeable Item")]
    public class PlaceableItem : Item, IHoldBehaviourItem
    {
        [BoxGroup(GeneralInformationBox)] [VerticalGroup(VerticalMain)]
        public PlaceableTile placeableTile;

        public void Hold(MouseButton mouseButton, TileBase tileClicked, Vector3Int pos, TilemapManager tilemapManager,
            InventoryManager inventoryManager, Vector3 playerPosition, Vector3 playerDistanceToClickedPoint)
        {
            if (mouseButton != MouseButton.Right) return;
            if (tileClicked is not null) return;
            if (!(playerDistanceToClickedPoint.sqrMagnitude > 101)) return;
            if (!tilemapManager.TryPlaceTile(placeableTile, pos, TilemapLayer.Obstacles)) return;

            // consume an item
            var draggingItem = inventoryManager.draggedItem.Item;
            var itemCount = draggingItem.count - 1;
            if (itemCount <= 0)
            {
                inventoryManager.DragItem(ItemStack.Empty);
            }
            else
            {
                inventoryManager.draggedItem.Item = draggingItem.Copy(count: itemCount);
            }
        }

        public bool CanInteract(TileBase tileAtLocation, Vector3Int pos, TilemapManager tilemapManager,
            InventoryManager inventoryManager, Vector3 playerPosition, Vector3 playerDistanceToClickedPoint)
        {
            if (tileAtLocation is not null) return false;
            var canPlace = placeableTile.CanPlace(pos, tilemapManager.GroundLayer, tilemapManager.ObstacleLayer);
            if (!placeableTile.hasCollider) return canPlace;
            
            return 
                /* is the player inside the block they are about to place? */
                playerDistanceToClickedPoint.sqrMagnitude > 101 && 
                /* custom placement predicate */
                canPlace;
        }
    }
}