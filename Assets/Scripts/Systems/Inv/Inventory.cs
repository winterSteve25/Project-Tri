using System;
using System.Collections.Generic;
using System.Linq;
using Items;
using UnityEngine;
using UnityEngine.Localization;

namespace Systems.Inv
{
    /// <summary>
    /// An abstraction for keeping data and logic for a list of itemstacks
    /// </summary>
    [Serializable]
    public class Inventory
    {
        public readonly int RowCount;
        public readonly int SlotsCount;
        public readonly LocalizedString InventoryName;
        public event Action OnChanged;
        
        public ItemStack[] ItemStacks { get; private set; }

        public Inventory(LocalizedString inventoryName, int rows = 3)
        {
            // if there is an equal amount of rows;
            if (rows % 2 == 0)
            {
                var half = rows / 2;
                SlotsCount = half * 6 + half * 5;
            }
            else
            {
                var halfHigher = Mathf.CeilToInt(rows * 0.5f);
                SlotsCount = halfHigher * 6 + (halfHigher - 1) * 5;
            }

            RowCount = rows;
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
        /// <returns></returns>
        public bool Add(Vector2 position, ItemStack itemStack)
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

                // for each stack if we have space we add them if not we spawn them to the ground
                foreach (var itemstack in allStacks)
                {
                    if (Count < SlotsCount)
                    {
                        AddToFirstAvailableSlot(itemstack);
                    }
                    else
                    {
                        ItemSpawner.Current.Spawn(position, itemStack);
                    }
                }

                OnChanged?.Invoke();
                return true;
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

        public void Load(ItemStack[] stacks)
        {
            if (stacks.Length != ItemStacks.Length)
            {
                Debug.LogWarning("Can not load inventory as itemstacks given is not the same size as the inventory");
                return;
            }

            ItemStacks = stacks;
            OnChanged?.Invoke();
        }
    }
}