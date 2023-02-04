using Items;
using Sirenix.OdinInspector;
using UI;
using UI.Managers;
using UI.Menu.InventoryMenu;
using UI.TextContents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Systems.Inv
{
    /// <summary>
    /// Item slot in the UI
    /// </summary>
    public class ItemSlot : UIItem, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private Inventory _inventory;
        private ItemDragManager _itemDragManager;
        private int _slotIndex;
        private bool _isPointerOver;
        private bool _isEquipmentSlot;
        
        private TextContent _textContent;

        public void Init(Inventory inventory, int index, bool equipmentSlot = false)
        {
            _inventory = inventory;
            _itemDragManager = ItemDragManager.Current;
            _slotIndex = index;
            _isEquipmentSlot = equipmentSlot;
            PostItemChanged();
        }
        
        protected override void PostItemChanged()
        {
            base.PostItemChanged();

            if (!item.IsEmpty)
            {
                _textContent = TextContent.Titled(item.item.itemName)
                    .AddText(item.item.itemDescription);
            }
            
            if (!_isPointerOver || !gameObject.activeSelf) return;
            ShowTooltip();
        }

        private void OnDisable()
        {
            if (_isPointerOver)
            {
                TooltipManager.Hide();
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            ShowTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            TooltipManager.Hide();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!Interactable) return;
            if (_inventory == null) return;
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                // not holding anything
                if (_itemDragManager.DraggedItem.Item.IsEmpty)
                {
                    if (item.IsEmpty) return;
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        var extraTab = ExtraTabController.Current;
                        var invToAdd = _inventory == extraTab.OpenedInventory ? InventoryTabController.Current.PlayerInventory : extraTab.OpenedInventory;
                        if (invToAdd == null) return;
                        if (invToAdd.Add(item, Vector2.zero, false))
                        {
                            _inventory[_slotIndex] = ItemStack.Empty;
                        }
                    }
                    else
                    {
                        // grabs the item from the slot
                        var itemStack = _inventory[_slotIndex];
                        _inventory[_slotIndex] = ItemStack.Empty;
                        _itemDragManager.DragItem(itemStack);
                    }
                }
                else
                {
                    // if currently holding the same item and has space
                    var i = item.item;
                    var draggedI = _itemDragManager.DraggedItem.Item;
                    
                    if (draggedI.item == i)
                    {
                        // if already full we swap
                        if (item.count >= i.maxStackSize)
                        {
                            _itemDragManager.DragItem(_inventory[_slotIndex]);
                            _inventory[_slotIndex] = new ItemStack(i, draggedI.count);
                            return;
                        }
                        
                        // if all can fit
                        if (item.count + draggedI.count <= i.maxStackSize)
                        {
                            // fit all in 1 slot
                            _inventory[_slotIndex] = new ItemStack(i, item.count + draggedI.count);
                            // stop dragging
                            _itemDragManager.DragItem(ItemStack.Empty);
                            return;
                        }

                        // how much was added
                        var amountToAdd = i.maxStackSize - item.count;
                        _inventory[_slotIndex] = new ItemStack(i, i.maxStackSize);
                        // drag the remaining
                        _itemDragManager.DragItem(new ItemStack(i, draggedI.count - amountToAdd));
                    }
                    else
                    {
                        var currentItem = item;
                        if (!CanItemBePlacedInSlot(draggedI)) return;
                        _inventory[_slotIndex] = draggedI;
                        _itemDragManager.DragItem(currentItem);
                    }
                    
                    if (!item.IsEmpty) return;
                    if (!CanItemBePlacedInSlot(draggedI)) return;
                    
                    // dump all in slot
                    _inventory[_slotIndex] = draggedI;
                    _itemDragManager.DragItem(ItemStack.Empty);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // if not dragging any item 
                if (_itemDragManager.DraggedItem.Item.IsEmpty)
                {
                    // split the stack in half and drag one half
                    var currItem = _inventory[_slotIndex];
                    var half = currItem.count / 2;
                    var itemStack = ItemStack.Empty;
                    if (half > 0)
                    {
                        itemStack = new ItemStack(currItem.item, half);
                    }
                
                    _inventory[_slotIndex] = itemStack;

                    var remaining = currItem.count - half;
                    if (remaining > 0)
                    {
                        _itemDragManager.DragItem(new ItemStack(currItem.item, remaining));
                    }
                }
                else
                {
                    // if currently holding the same item and has space, we add one to current slot
                    var i = item.item;
                    var draggedI = _itemDragManager.DraggedItem.Item;
                    if (draggedI.item == i && item.count < i.maxStackSize)
                    {
                        _inventory[_slotIndex] = new ItemStack(i, item.count + 1);
                        _itemDragManager.DraggedItem.Item = new ItemStack(i, draggedI.count - 1);
                        return;
                    }

                    // current slot is empty
                    if (item.IsEmpty && CanItemBePlacedInSlot(draggedI))
                    {
                        _inventory[_slotIndex] = new ItemStack(draggedI.item, 1);
                        _itemDragManager.DraggedItem.Item = new ItemStack(draggedI.item, draggedI.count - 1);
                    }
                }
            }
        }

        private void ShowTooltip()
        {
            if (item.IsEmpty) return;
            TooltipManager.Show(_textContent);
        }

        private bool CanItemBePlacedInSlot(ItemStack itemStack)
        {
            // if this is an equipment slot
            if (_isEquipmentSlot)
            {
                // check if this item is valid for this slot
                if (!itemStack.item.equipmentType.CanPlaceIn(_slotIndex))
                {
                    // if not we dont continue
                    return false;
                }
            }

            return true;
        }

        [Button]
        private void Test()
        {
            Debug.Log("Hello");
        }
    }
}