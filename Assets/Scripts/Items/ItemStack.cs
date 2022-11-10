using System;

namespace Items
{
    /// <summary>
    /// Struct that contains the item type and the count. Basically an instance of an item
    /// </summary>
    [Serializable]
    public struct ItemStack
    {
        public static readonly ItemStack Empty = new();
        
        public Item item;
        public int count;

        public bool IsEmpty => item == null || count <= 0;
        
        public ItemStack(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }

        public override string ToString()
        {
            return $"{item.name} x{count}";
        }
    }
}