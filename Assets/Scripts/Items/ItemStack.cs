using System;
using System.Linq;
using MsgPack.Serialization;
using Registries;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils.Data;

namespace Items
{
    /// <summary>
    /// Struct that contains the item type and the count. Basically an instance of an item
    /// </summary>
    [Serializable]
    public struct ItemStack
    {
        public static readonly ItemStack Empty = new();
        
        public string itemId => item == null ? string.Empty : ItemsRegistry.Instance.Entries[item];
        [MinValue(1)]
        public int count;
        
        [MessagePackIgnore] 
        [AssetSelector(Paths = "Assets/Resources/Items")]
        public Item item;
        
        [MessagePackIgnore] 
        public bool IsEmpty => item == null || count <= 0;
        
        public ItemStack(Item item, int count)
        {
            this.item = item;
            this.count = count;
        }

        [MessagePackDeserializationConstructor]
        public ItemStack(string itemId, int count)
        {
            item = itemId == string.Empty ? null : ItemsRegistry.Instance.Entries.First(i => i.Value == itemId).Key;
            this.count = count;
        }

        public override string ToString()
        {
            return $"{item.name} x{count}";
        }

        public ItemStack Copy(Item item = null, int count = -1, SerializableData customData = null)
        {
            return Copy(this, item, count, customData);
        }

        public static ItemStack Copy(ItemStack itemStack, Item item = null, int count = -1, SerializableData customData = null)
        {
            var newIs = new ItemStack
            {
                item = item != null ? item : itemStack.item,
                count = count != -1 ? count : itemStack.count,
            };

            return newIs;
        }

        public static bool operator ==(ItemStack a, ItemStack b)
        {
            return a.item == b.item && a.count == b.count;
        }

        public static bool operator !=(ItemStack a, ItemStack b)
        {
            return !(a == b);
        }
        
        public bool Equals(ItemStack other)
        {
            return count == other.count && Equals(item, other.item);
        }

        public override bool Equals(object obj)
        {
            return obj is ItemStack other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(count, item);
        }
    }
}