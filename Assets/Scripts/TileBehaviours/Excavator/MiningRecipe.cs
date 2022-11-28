using System;
using Items;
using Tiles;

namespace TileBehaviours.Excavator
{
    /// <summary>
    /// An individual excavator recipe
    /// </summary>
    [Serializable]
    public struct MiningRecipe
    {
        public TriTile ore;
        public ItemStack output;
    }
}