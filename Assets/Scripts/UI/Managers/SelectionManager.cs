using DG.Tweening;
using Items;
using Systems.Inv;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using World;

namespace UI.Managers
{
    public class SelectionManager : MonoBehaviour
    {
        private InventoryManager _inventoryManager;
        private Tilemap _groundLayer;
        private Tilemap _obstacleLayer;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas selectionCanvas;
        [SerializeField] private CanvasGroup selection;

        private void Start()
        {
            _inventoryManager = InventoryManager.current;
            var tilemapManager = FindObjectOfType<TilemapManager>();
            _groundLayer = tilemapManager.GroundLayer;
            _obstacleLayer = tilemapManager.ObstacleLayer;
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