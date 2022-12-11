using System;
using System.Linq;
using MsgPack.Serialization;
using Registries;
using Sirenix.OdinInspector;
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

        private SerializableDataStorage _savedData;
        public SerializableDataStorage SavedData
        {
            get => _savedData ??= new SerializableDataStorage();
            set => _savedData = value;
        }
        
        [MessagePackIgnore] private DataStorage _customData;
        [MessagePackIgnore] public DataStorage CustomData
        {
            get => _customData ??= new DataStorage();
            set => _customData = value;
        }

        [MessagePackIgnore] 
        [AssetSelector]
        public TriItem item;

        [MessagePackIgnore]
        public bool IsEmpty => item == null || count <= 0;

        public ItemStack(TriItem item, int count)
        {
            this.item = item;
            this.count = count;
            _savedData = new SerializableDataStorage();
            _customData = new DataStorage();
        }

        [MessagePackDeserializationConstructor]
        public ItemStack(string itemId, int count, SerializableDataStorage savedData)
        {
            item = itemId == string.Empty ? null : ItemsRegistry.Instance.Entries.First(i => i.Value == itemId).Key;
            this.count = count;
            _savedData = savedData;
            _customData = new DataStorage();
        }

        public override string ToString()
        {
            return $"{item.name} x{count}";
        }

        public ItemStack Copy(TriItem item = null, int count = -1, SerializableDataStorage customData = null)
        {
            return Copy(this, item, count, customData);
        }

        public static ItemStack Copy(ItemStack itemStack, TriItem item = null, int count = -1, SerializableDataStorage customData = null)
        {
            var newIs = new ItemStack
            {
                item = item != null ? item : itemStack.item,
                count = count != -1 ? count : itemStack.count,
                _savedData = customData ?? itemStack._savedData
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