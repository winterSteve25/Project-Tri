using System;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventorySystem
{
    /// <summary>
    /// Item slot in the UI
    /// </summary>
    public class ItemSlot : UIItem, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite hoverSprite;
        
        public Image slotIcon;
        public CanvasGroup canvasGroup;

        private InventoryManager _inventoryManager;
        private int _slotIndex;
        
        protected override void Start()
        {
            _inventoryManager = InventoryManager.Instance;
            _slotIndex = _inventoryManager.Slots.IndexOf(this);
            Refresh();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            slotIcon.sprite = hoverSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            slotIcon.sprite = normalSprite;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (_inventoryManager.draggedItem.Item.IsEmpty)
                {
                    var currInv = _inventoryManager.CurrentInventory;
                    if (currInv == null) return;
                    var itemStack = currInv[_slotIndex];
                    currInv[_slotIndex] = ItemStack.Empty;
                    _inventoryManager.DragItem(itemStack);
                }
                else
                {
                    // if currently holding the same item and has space
                    var i = item.item;
                    var draggedI = _inventoryManager.draggedItem.Item;
                    if (draggedI.item == i)
                    {
                        // if all can fit
                        if (item.count + draggedI.count <= i.maxStackSize)
                        {
                            // fit all in 1 slot
                            _inventoryManager.CurrentInventory[_slotIndex] = new ItemStack(i, item.count + draggedI.count);
                            // stop dragging
                            _inventoryManager.DragItem(ItemStack.Empty);
                            return;
                        }

                        // how much was added
                        var amountToAdd = i.maxStackSize - item.count;
                        _inventoryManager.CurrentInventory[_slotIndex] = new ItemStack(i, i.maxStackSize);
                        // drag the remaining
                        _inventoryManager.DragItem(new ItemStack(i, draggedI.count - amountToAdd));
                    }
                    else
                    {
                        var currentItem = item;
                        _inventoryManager.CurrentInventory[_slotIndex] = draggedI;
                        _inventoryManager.DragItem(currentItem);
                    }

                    // current slot is empty
                    if (item.IsEmpty)
                    {
                        // dump all in slot
                        _inventoryManager.CurrentInventory[_slotIndex] = draggedI;
                        _inventoryManager.DragItem(ItemStack.Empty);
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // if not dragging any item 
                if (_inventoryManager.draggedItem.Item.IsEmpty)
                {
                    // split the stack in half and drag one half
                    var currInv = _inventoryManager.CurrentInventory;
                    var currItem = currInv[_slotIndex];
                    var half = currItem.count / 2;
                    var itemStack = ItemStack.Empty;
                    if (half > 0)
                    {
                        itemStack = new ItemStack(currItem.item, half);
                    }
                
                    currInv[_slotIndex] = itemStack;

                    var remaining = currItem.count - half;
                    if (remaining > 0)
                    {
                        _inventoryManager.DragItem(new ItemStack(currItem.item, remaining));
                    }
                }
                else
                {
                    // if currently holding the same item and has space, we add one to current slot
                    var i = item.item;
                    var draggedI = _inventoryManager.draggedItem.Item;
                    if (draggedI.item == i && item.count < i.maxStackSize)
                    {
                        _inventoryManager.CurrentInventory[_slotIndex] = new ItemStack(i, item.count + 1);
                        _inventoryManager.draggedItem.Item = new ItemStack(i, draggedI.count - 1);
                        return;
                    }

                    // current slot is empty
                    if (item.IsEmpty)
                    {
                        _inventoryManager.CurrentInventory[_slotIndex] = new ItemStack(draggedI.item, 1);
                        _inventoryManager.draggedItem.Item = new ItemStack(draggedI.item, draggedI.count - 1);
                    }
                }
            }
        }
    }
}