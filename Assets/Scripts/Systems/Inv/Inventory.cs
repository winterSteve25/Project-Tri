using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using SaveLoad.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using Task = System.Threading.Tasks.Task;

namespace Systems.Inv
{
    /// <summary>
    /// An abstraction for keeping data and logic for a list of itemstacks
    /// </summary>
    [Serializable]
    public class Inventory
    {
        public readonly int SlotsCount;
        public readonly LocalizedString InventoryName;
        public event Action OnChanged;
        public event Action<ItemStack, bool> OnItemChanged; 

        public ItemStack[] ItemStacks { get; private set; }

        public Inventory(LocalizedString inventoryName, int slotsCount = 20)
        {
            SlotsCount = slotsCount;
            InventoryName = inventoryName;
            ItemStacks = new ItemStack[SlotsCount];
        }
        
        public int Count => ItemStacks.Count(i => !i.IsEmpty);
        public ItemStack this[int index]
        {
            get => ItemStacks[index];
            set
            {
                ItemStacks[index] = value;
                OnChanged?.Invoke();
            }
        }

        /// <summary>
        /// Attempts to add itemstack to inventory 
        /// </summary>
        /// <param name="position">Position that it will spawn the access items to the ground if inventory can not fit</param>
        /// <param name="itemStack">ItemStack to add</param>
        /// <param name="spawnOnGround"></param>
        /// <returns>If the item was added</returns>
        public bool Add(ItemStack itemStack, Vector2 position, bool spawnOnGround = true)
        {
            var result = AddInternal(position, itemStack, spawnOnGround);

            if (result)
            {
                OnItemChanged?.Invoke(itemStack, true);
            }
            
            return result;
        }
        
        private bool AddInternal(Vector2 position, ItemStack itemStack, bool spawnOnGround)
        {
            if (itemStack.IsEmpty)
            {
                Debug.LogWarning($"Tried to add air to inventory with name {InventoryName}");
                return true;
            }
            
            var i = Array.FindIndex(ItemStacks, i => i.item == itemStack.item && i.count < i.item.maxStackSize);
            var maxStackSize = itemStack.item.maxStackSize;

            if (maxStackSize <= 0)
            {
                Debug.LogError($"{itemStack.item.name}'s max stack size can not be 0 or below!");
                return false;
            }
            
            var itemCount = itemStack.count;
            var item = itemStack.item;

            #region Existing Slots

            // if there available slots
            while (i != -1)
            {
                var existing = ItemStacks[i];

                // if it can fit all of them
                if (existing.count + itemCount < maxStackSize)
                {
                    ItemStacks[i] = new ItemStack(item, existing.count + itemCount);
                    OnChanged?.Invoke();
                    return true;
                }
                
                ItemStacks[i] = new ItemStack(item, maxStackSize);
                itemCount -= maxStackSize - existing.count;

                if (itemCount <= 0)
                {
                    OnChanged?.Invoke();
                    return true;
                }
                
                i = Array.FindIndex(ItemStacks, stack => stack.item == itemStack.item && stack.count < stack.item.maxStackSize);
            }

            #endregion
            
            #region No Existing Slots

            // no existing slot available
            // if there are more than 1 stack
            if (itemCount > maxStackSize)
            {
                var allStacks = new List<ItemStack>();
                var remainingItems = itemCount;
                    
                // create max stacks until no more max stack can be created
                while (remainingItems > maxStackSize)
                {
                    allStacks.Add(new ItemStack(item, maxStackSize));
                    remainingItems -= maxStackSize;
                }

                // make a itemstack for the remaining items
                if (remainingItems > 0)
                {
                    allStacks.Add(new ItemStack(item, remainingItems));
                }

                var hasSpace = true;

                // for each stack if we have space we add them if not we spawn them to the ground
                foreach (var itemstack in allStacks)
                {
                    if (Count < SlotsCount)
                    {
                        AddToFirstAvailableSlot(itemstack);
                    }
                    else
                    {
                        if (spawnOnGround)
                        {
                            ItemSpawner.Current.Spawn(position, itemStack);
                        }

                        hasSpace = false;
                    }
                }

                OnChanged?.Invoke();
                return hasSpace;
            }

            if (Count >= SlotsCount) return false;
            AddToFirstAvailableSlot(new ItemStack(itemStack.item, itemCount));
            OnChanged?.Invoke();
            return true;

            #endregion
        }
        
        public bool Contains(ItemStack itemStack)
        {
            var itemStacks = ItemStacks.Where(item => item.item == itemStack.item).ToArray();
            
            if (itemStacks.Length > 0)
            {
                return itemStacks.Aggregate(0, (value, item) => value + item.count) >= itemStack.count;
            }

            return false;
        }

        public bool Remove(ItemStack itemStack)
        {
            var result = RemoveInternal(itemStack);

            if (result)
            {
                OnItemChanged?.Invoke(itemStack, false);
            }
            
            return result;
        }
        
        private bool RemoveInternal(ItemStack itemStack)
        {
            if (!Contains(itemStack)) return false;

            var amountToRemove = itemStack.count;
            
            foreach (var iStack in ItemStacks.Where(i => !i.IsEmpty && i.item == itemStack.item).Reverse())
            {
                if (iStack.count >= amountToRemove)
                {
                    var remaining = iStack.count - amountToRemove;
                    if (remaining > 0)
                    {
                        ItemStacks[Array.IndexOf(ItemStacks, iStack)] = new ItemStack(iStack.item, remaining);
                    }
                    else
                    {
                        ItemStacks[Array.IndexOf(ItemStacks, iStack)] = ItemStack.Empty;
                    }
                    
                    OnChanged?.Invoke();
                    return true;
                }

                amountToRemove -= iStack.count;
                ItemStacks[Array.IndexOf(ItemStacks, iStack)] = ItemStack.Empty;
            }

            return false;
        }
        
        private void AddToFirstAvailableSlot(ItemStack itemstack)
        {
            ItemStacks[Array.FindIndex(ItemStacks, s => s.IsEmpty)] = itemstack;
        }
        
        public void RefreshItemSlotsWithContent(ItemSlot[] slots)
        {
            RefreshItemSlotsWithContent(slots, this);
        }

        public async Task Serialize(SaveTask task)
        {
            await task.Serialize(ItemStacks);
        }

        public async Task Deserialize(LoadTask task)
        {
            Load(await task.Deserialize<ItemStack[]>());
        }
        
        private void Load(ItemStack[] stacks)
        {
            if (stacks.Length != ItemStacks.Length)
            {
                Debug.LogWarning("Can not load inventory as itemstacks given is not the same size as the inventory");
                return;
            }

            ItemStacks = stacks;
            OnChanged?.Invoke();
        }
        
        public static void RefreshItemSlotsWithContent(ItemSlot[] slots, Inventory inventory)
        {
            for (var i = 0; i < slots.Length; i++)
            {
                slots[i].Item = inventory == null ? ItemStack.Empty : inventory[i];
            }
        }
    }
}