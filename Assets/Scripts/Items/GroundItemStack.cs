using UnityEngine;

namespace Items
{
    public struct GroundItemStack
    {
        public ItemStack Item;
        public Vector2 Position;
        public float DespawnTimer;

        public GroundItemStack(ItemStack item, Vector2 position, float despawnTimer)
        {
            Item = item;
            Position = position;
            DespawnTimer = despawnTimer;
        }
    }
}