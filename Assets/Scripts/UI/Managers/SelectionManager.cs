﻿using DG.Tweening;
using Items;
using Player;
using Player.Interaction;
using UI.Menu.InventoryMenu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using Utils;
using World.Tiles;

namespace UI.Managers
{
    public class SelectionManager : MonoBehaviour
    {
        private InventoryController _inventoryController;
        private EquipmentsController _equipmentsController;
        private Tilemap _obstacleLayer;
        private TilemapManager _tilemapManager;
        private Transform _playerTransform;

        [SerializeField] private Camera mainCamera;
        [SerializeField] private Canvas selectionCanvas;
        [SerializeField] private CanvasGroup selection;

        private void Start()
        {
            _inventoryController = InventoryController.Current;
            _equipmentsController = EquipmentsController.Current;
            _tilemapManager = TilemapManager.Current;
            _obstacleLayer = _tilemapManager.ObstacleLayer;
            _playerTransform = FindObjectOfType<PlayerInteractionHandler>().transform;
        }

        private void OnEnable()
        {
            PlayerCameraControl.ScaledObjects.Add(selection.transform);
        }

        private void OnDisable()
        {
            PlayerCameraControl.ScaledObjects.Remove(selection.transform);
        }

        private void Update()
        {
            var point = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            point.z = 0;
            var pos = _obstacleLayer.WorldToCell(point);
            var tileAtPos = _tilemapManager.GetTile(pos, TilemapLayer.Obstacles);
            var isEmpty = tileAtPos == null;
            var overUI = EventSystem.current.IsPointerOverGameObject();

            var item = _equipmentsController[EquipmentType.Outer];
            var isHoldingInteractableItem = item.item is IInteractableItem;

            if ((isEmpty && !isHoldingInteractableItem) || overUI)
            {
                HideSelection();
            }
            else if (isHoldingInteractableItem)
            {
                var playerPosition = _playerTransform.position;
                var interactable = (IInteractableItem)item.item;

                if (interactable.CanInteract(ref item, tileAtPos, point, pos, _tilemapManager, _inventoryController,
                        _equipmentsController, playerPosition))
                {
                    MoveSelection(pos);
                }
                else
                {
                    HideSelection();
                }
            }
            // else
            // {
            //     MoveSelection(pos);
            // }
        }

        private void MoveSelection(Vector3Int cellPos)
        {
            var cam = selectionCanvas.worldCamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                selectionCanvas.transform as RectTransform,
                RectTransformUtility.WorldToScreenPoint(cam,
                    _obstacleLayer.CellToWorld(cellPos) + _obstacleLayer.cellSize * 0.5f),
                cam, out var p);
            var p2 = selectionCanvas.transform.TransformPoint(p);

            if (!selection.gameObject.activeSelf)
            {
                selection.transform.position = p2;
                selection.FadeIn(0.2f);
            }

            selection.transform.DOMove(p2, 0.35f)
                .SetEase(Ease.OutCubic);
        }

        private void HideSelection()
        {
            if (selection.gameObject.activeSelf)
            {
                selection.FadeOut(0.2f);
            }
        }
    }
}