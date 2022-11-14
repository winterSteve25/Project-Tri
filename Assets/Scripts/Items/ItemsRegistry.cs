using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    [CreateAssetMenu(fileName = "Items Registry", menuName = "Items/New Items Registry")]
    public class ItemsRegistry : ScriptableObject
    {
        private static ItemsRegistry _instance;
        public static ItemsRegistry Instance => _instance ??= Resources.Load<ItemsRegistry>("Items/Items Registry");
        
        [SerializeField] private List<Item> items;
        public List<Item> Items => items;
    }
}