using UnityEngine;
using UnityEngine.Localization;

namespace Items
{
    
    /// <summary>
    /// Object that holds the identity of an item
    /// </summary>
    [CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
    public class Item : ScriptableObject
    {
        public LocalizedString itemName;
        public LocalizedString itemDescription;
        public Sprite sprite;
        public int maxStackSize;

        public virtual void OnPickUp()
        {
        }
    }
}