using DG.Tweening;
using InventorySystem;
using Items;
using Terrain;
using Tiles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace Player
{
    /// <summary>
    /// Allows player to edit terrain (place, mine)
    /// </summary>
    public class PlayerTerrainInteraction : MonoBehaviour
    {
        private TilemapManager _tilemapManager;
        private Tilemap _groundLayer;
        private Tilemap _obstacleLayer;
        private Camera _camera;

        [SerializeField] private Canvas selectionCanvas;
        [SerializeField] private CanvasGroup selection;

        private void Start()
        {
            _camera = Camera.main; 
            _tilemapManager = FindObjectOfType<TilemapManager>();
            _groundLayer = _tilemapManager.GroundLayer;
            _obstacleLayer = _tilemapManager.ObstacleLayer;
        }

        private void Update()
        {
            var point = _camera.ScreenToWorldPoint(Input.mousePosition);
            var pos = _obstacleLayer.WorldToCell(point);
            var tileAtPos = _obstacleLayer.GetTile(pos);
            var isEmpty = tileAtPos == null;
            var overUI = EventSystem.current.IsPointerOverGameObject();
            
            var inventoryManager = InventoryManager.Instance;
            var draggingItem = inventoryManager.draggedItem.Item;
            var isHoldingPlaceableItem = draggingItem.item is PlaceableItem;
            
            // selection
            if ((isEmpty && !isHoldingPlaceableItem) || overUI)
            {
                HideSelection();
            }
            else if (isHoldingPlaceableItem)
            {
                var placeableItem = (PlaceableItem) draggingItem.item;
                if (placeableItem.placeableTile.CanPlace(pos, _groundLayer, _obstacleLayer))
                {
                    MoveSelection(pos);
                }
                else
                {
                    HideSelection();
                }
            }
            else
            {
                MoveSelection(pos);
            }

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
                                    inventoryManager.DragItem(ItemStack.Empty);
                                }
                                else
                                {
                                    inventoryManager.draggedItem.Item = new ItemStack(draggingItem.item, itemCount);
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

        private void MoveSelection(Vector3Int cellPos)
        {
            var cam = selectionCanvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                selectionCanvas.transform as RectTransform,
                RectTransformUtility.WorldToScreenPoint(cam, _obstacleLayer.CellToWorld(cellPos) + _obstacleLayer.cellSize * 0.5f),
                cam, out var p);
            var p2 = selectionCanvas.transform.TransformPoint(p);
            
            if (!selection.gameObject.activeSelf)
            {
                selection.transform.position = p2;
                selection.gameObject.SetActive(true);
                selection.DOFade(1, 0.2f)
                    .SetEase(Ease.Linear);
            }
            
            selection.transform.DOMove(p2, 0.35f)
                .SetEase(Ease.OutCubic);
        }

        private void HideSelection()
        {
            if (selection.gameObject.activeSelf)
            {
                selection.DOFade(0, 0.2f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => selection.gameObject.SetActive(false));
            }
        }
    }
}