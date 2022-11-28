﻿using Items;
using JetBrains.Annotations;
using Player;
using Sirenix.OdinInspector;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Utils;

namespace UI.Menu.EscapeMenu
{
    public class InventoryUIController : CurrentInstanced<InventoryUIController>
    {
        #region Inventories

        public Inventory PlayerInventory { get; private set; }
        
        [CanBeNull]
        public Inventory OpenedInventory { get; private set; }

        #endregion

        #region Prefabs

        [FoldoutGroup("Prefabs"), AssetsOnly, SerializeField, Required]
        private ItemSlot slot;

        [FoldoutGroup("Prefabs"), AssetsOnly, SerializeField, Required]
        private Transform largeLongRow;

        [FoldoutGroup("Prefabs"), AssetsOnly, SerializeField, Required]
        private Transform largeShortRow;
        
        [FoldoutGroup("Prefabs"), AssetsOnly, SerializeField, Required]
        private Transform smallLongRow;
        
        [FoldoutGroup("Prefabs"), AssetsOnly, SerializeField, Required]
        private Transform smallShortRow;

        #endregion

        #region Parent Transforms

        [FoldoutGroup("Parent Transforms"), SerializeField, Required]
        private Transform playerInventorySlotParent;
        
        [FoldoutGroup("Parent Transforms"), SerializeField, Required]
        private Transform openedInventorySlotParent;

        #endregion

        #region Item Slots

        private ItemSlot[] _playerSlots;
        private ItemSlot[] _openedInventorySlots;

        #endregion

        #region Opened Inventory

        [SerializeField, Required] 
        [FoldoutGroup("Opened Inventory")]
        private GameObject openedInventoryTabButton;

        [SerializeField, Required] 
        [FoldoutGroup("Opened Inventory")]
        private LocalizeStringEvent openedInventoryTabButtonTitle;

        [SerializeField, Required] 
        [FoldoutGroup("Opened Inventory")]
        private LocalizeStringEvent openedInventoryTabTitle;
        
        #endregion

        [SerializeField, Required]
        private EscapeMenuController escapeMenuController;
        
        private void Start()
        {
            RefreshPlayerInventory();
            OpenPlayerInventory(PlayerInventory);
            openedInventoryTabButton.SetActive(false);
        }

        public void SetOpenedInventory(Inventory inventory)
        {
            if (inventory != null)
            {
                escapeMenuController.ToggleMenu();
            }
            
            // if we are replacing an inventory remove the listener on the old one
            if (inventory == null && OpenedInventory != null)
            {
                OpenedInventory.OnChanged -= RefreshOpenedInvContent;
            }
            
            // sets up the slots and rows in the UI
            OpenedInventory = inventory;

            // add listener to inventory change
            if (OpenedInventory != null)
            {
                OpenedInventory.OnChanged += RefreshOpenedInvContent;
                openedInventoryTabButton.SetActive(true);
                openedInventoryTabButtonTitle.StringReference = OpenedInventory.InventoryName;
                openedInventoryTabTitle.StringReference = OpenedInventory.InventoryName;
                openedInventoryTabButtonTitle.RefreshString();
                openedInventoryTabTitle.RefreshString();
                RefreshOpenedInvContent();
                OpenOtherInventory(OpenedInventory);
            }
            else
            {
                openedInventoryTabButton.SetActive(false);
            }
        }

        [Button]
        private void AddItem(ItemStack itemStack)
        {
            PlayerInventory.Add(Vector2.zero, itemStack);
        }
        
        #region Refresh Inventory

        private void RefreshPlayerInventory(Inventory overrideInv = null)
        {
            if (PlayerInventory != null)
            {
                PlayerInventory.OnChanged -= RefreshPlayerInvContent;
            }
            
            PlayerInventory = overrideInv ?? FindObjectOfType<PlayerInventory>().Inv;
            PlayerInventory.OnChanged += RefreshPlayerInvContent;
        }

        private void RefreshPlayerInvContent()
        {
            PlayerInventory.RefreshItemSlotsWithContent(_playerSlots);
        }

        private void RefreshOpenedInvContent()
        {
            OpenedInventory?.RefreshItemSlotsWithContent(_openedInventorySlots);
        }
        
        #endregion

        #region Slot Setup Helpers

        private void OpenPlayerInventory(Inventory inventory)
        {
            OpenInventory(inventory, playerInventorySlotParent, largeLongRow, largeShortRow, 8, 7, slot, ref _playerSlots);
        }

        private void OpenOtherInventory(Inventory inventory)
        {
            OpenInventory(inventory, openedInventorySlotParent, smallLongRow, smallShortRow, 5, 4, slot, ref _openedInventorySlots);
        }

        private static void OpenInventory(Inventory inventory, Transform parent, Transform longPrefab, Transform shortPrefab, int longCapacity, int shortCapacity, ItemSlot slotPrefab, ref ItemSlot[] slotsArray)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }

            if (inventory == null) return;
            
            slotsArray = new ItemSlot[inventory.SlotsCount];

            var slots = inventory.SlotsCount;
            var i = 0;
            var n = 0;
            
            while (slots > 0)
            {
                var capacity = longCapacity;
                Transform go;
                
                if (i % 2 == 0)
                {
                    go = Instantiate(longPrefab, parent);
                }
                else
                {
                    capacity = shortCapacity;
                    go = Instantiate(shortPrefab, parent);
                }
                
                var remaining = slots - capacity;
                var amountToSpawn = capacity;
                
                if (remaining < 0)
                {
                    amountToSpawn = slots;
                }

                for (var j = 0; j < amountToSpawn; j++)
                {
                    var slot = Instantiate(slotPrefab, go);
                    slot.Init(inventory, n);
                    slotsArray[n] = slot;
                    n++;
                }

                slots -= amountToSpawn;
                i++;
            }
        }

        #endregion
    }
}