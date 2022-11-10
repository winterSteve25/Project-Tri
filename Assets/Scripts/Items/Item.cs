using System;
using UnityEngine;

namespace Items
{
    
    /// <summary>
    /// Object that holds the identity of an item
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
    public class Item : ScriptableObject
    {
        public Sprite sprite;
        public int maxStackSize;

        public virtual void OnPickUp()
        {
        }

        private void OnEnable()
        {
            ItemsRegistry.items.Add(this);
        }
    }
}