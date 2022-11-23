using System;
using Items;
using UnityEngine.Tilemaps;

namespace Tiles.Excavator
{
    /// <summary>
    /// An individual excavator recipe
    /// </summary>
    [Serializable]
    public struct MiningRecipe
    {
        public TileBase ore;
        public float duration;
        public ItemStack output;
    }
}