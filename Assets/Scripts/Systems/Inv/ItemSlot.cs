using Items;
using UI;
using UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Systems.Inv
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
        private bool _isPointerOver;

        private Tooltip _tooltip;

        protected override void Start()
        {
            _inventoryManager = InventoryManager.Current;
            _slotIndex = _inventoryManager.Slots.IndexOf(this);
            PostItemChanged();
        }

        protected override void PostItemChanged()
        {
            base.PostItemChanged();

            if (!item.IsEmpty)
            {
                _tooltip = Tooltip.Empty()
                    .AddText(item.item.itemName, headerStyle: true)
                    .AddText(item.item.itemDescription);
            }
            
            if (!_isPointerOver) return;
            
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
            slotIcon.sprite = hoverSprite;
            ShowTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            slotIcon.sprite = normalSprite;
            TooltipManager.Hide();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            var currentInventory = _inventoryManager.CurrentInventory;
            if (currentInventory == null) return;

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (_inventoryManager.draggedItem.Item.IsEmpty)
                {
                    var itemStack = currentInventory[_slotIndex];
                    currentInventory[_slotIndex] = ItemStack.Empty;
                    _inventoryManager.DragItem(itemStack);
                }
                else
                {
                    // if currently holding the same item and has space
                    var i = item.item;
                    var draggedI = _inventoryManager.draggedItem.Item;
                    
                    
                    if (draggedI.item == i)
                    {
                        // if already full we swap
                        if (item.count >= i.maxStackSize)
                        {
                            _inventoryManager.DragItem(currentInventory[_slotIndex]);
                            currentInventory[_slotIndex] = new ItemStack(i, draggedI.count);
                            return;
                        }
                        
                        // if all can fit
                        if (item.count + draggedI.count <= i.maxStackSize)
                        {
                            // fit all in 1 slot
                            currentInventory[_slotIndex] = new ItemStack(i, item.count + draggedI.count);
                            // stop dragging
                            _inventoryManager.DragItem(ItemStack.Empty);
                            return;
                        }

                        // how much was added
                        var amountToAdd = i.maxStackSize - item.count;
                        currentInventory[_slotIndex] = new ItemStack(i, i.maxStackSize);
                        // drag the remaining
                        _inventoryManager.DragItem(new ItemStack(i, draggedI.count - amountToAdd));
                    }
                    else
                    {
                        var currentItem = item;
                        currentInventory[_slotIndex] = draggedI;
                        _inventoryManager.DragItem(currentItem);
                    }

                    // current slot is empty
                    if (!item.IsEmpty) return;
                    
                    // dump all in slot
                    currentInventory[_slotIndex] = draggedI;
                    _inventoryManager.DragItem(ItemStack.Empty);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                // if not dragging any item 
                if (_inventoryManager.draggedItem.Item.IsEmpty)
                {
                    // split the stack in half and drag one half
                    var currInv = currentInventory;
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
                        currentInventory[_slotIndex] = new ItemStack(i, item.count + 1);
                        _inventoryManager.draggedItem.Item = new ItemStack(i, draggedI.count - 1);
                        return;
                    }

                    // current slot is empty
                    if (item.IsEmpty)
                    {
                        currentInventory[_slotIndex] = new ItemStack(draggedI.item, 1);
                        _inventoryManager.draggedItem.Item = new ItemStack(draggedI.item, draggedI.count - 1);
                    }
                }
            }
        }

        private void ShowTooltip()
        {
            if (item.IsEmpty) return;
            TooltipManager.Show(_tooltip);
        }
    }
}