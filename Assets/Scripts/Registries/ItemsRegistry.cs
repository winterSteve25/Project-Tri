using System;
using Items;
using UnityEngine;

namespace Registries
{
    [CreateAssetMenu(fileName = "Items Registry", menuName = "Registries/New Items Registry")]
    public class ItemsRegistry : Registry<ItemsRegistry.ItemsRegistryDictionary, TriItem>
    {
        private static ItemsRegistry _instance;
        public static ItemsRegistry Instance => _instance ??= Resources.Load<ItemsRegistry>("Registries/REG_Items Registry");

        [Serializable]
        public class ItemsRegistryDictionary : SerializableDictionary<TriItem, string>
        {
        }
    }
}