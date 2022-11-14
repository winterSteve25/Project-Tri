using Items;
using Systems.Inv;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using World;

namespace Player
{
    /// <summary>
    /// Allows player to edit terrain (place, mine)
    /// </summary>
    public class PlayerTerrainInteraction : MonoBehaviour
    {
        private InventoryManager _inventoryManager;
        private TilemapManager _tilemapManager;
        private Tilemap _groundLayer;
        private Tilemap _obstacleLayer;
        
        [SerializeField] private Camera mainCamera;

        private void Start()
        {
            _inventoryManager = InventoryManager.current;
            _tilemapManager = FindObjectOfType<TilemapManager>();
            _groundLayer = _tilemapManager.GroundLayer;
            _obstacleLayer = _tilemapManager.ObstacleLayer;
        }

        private void Update()
        {
            var point = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            var pos = _obstacleLayer.WorldToCell(point);
            var tileAtPos = _obstacleLayer.GetTile(pos);
            var isEmpty = tileAtPos == null;
            var overUI = EventSystem.current.IsPointerOverGameObject();
            
            var draggingItem = _inventoryManager.draggedItem.Item;
            var isHoldingPlaceableItem = draggingItem.item is PlaceableItem;

            if (Input.GetMouseButton(0) && !overUI)
            {
                if (!isEmpty)
                {
                    _tilemapManager.RemoveTile(pos, TilemapLayer.Obstacles);
                }
            }

            if (Input.GetMouseButton(1) && !overUI)
            {
                switch (isEmpty)
                {
                    case true:
                        // if currently using a placeable item
                        if (isHoldingPlaceableItem)
                        {
                            var placeableItem = (PlaceableItem) draggingItem.item;
                            var tile = placeableItem.placeableTile;
                            if (tile.CanPlace(pos, _groundLayer, _obstacleLayer))
                            {
                                // place the tile
                                _tilemapManager.PlaceTile(pos, tile.tileBase, TilemapLayer.Obstacles);

                                // consume an item
                                var itemCount = draggingItem.count - 1;
                                if (itemCount <= 0)
                                {
                                    _inventoryManager.DragItem(ItemStack.Empty);
                                }
                                else
                                {
                                    _inventoryManager.draggedItem.Item = new ItemStack(draggingItem.item, itemCount);
                                }
                            }
                        }

                        break;
                    case false:
                    {
                        var go = _tilemapManager.GetGameObject(tileAtPos, pos, _obstacleLayer);
                        if (go == null) return;
                        if (go.TryGetComponent<MachineTile>(out var tileMachine))
                        {
                            tileMachine.OnInteract();
                        }

                        break;
                    }
                }
            }
        }
    }
}