using EditorAttributes;
using Items;
using Sirenix.OdinInspector;
using Systems.Craft;

namespace Editor.Registries
{
    public sealed class RecipeTableEntry : BaseTableEntry<CraftingRecipe>
    {
        public RecipeTableEntry(CraftingRecipe o) : base(o)
        {
        }

        [TableColumnWidth(100, false)]
        [ShowInInspector, ItemSlot]
        public ItemStack ResultItem
        {
            get => Object.result;
            set => Object.result = value;
        }

        [ShowInInspector]
        public ItemStack[] Inputs
        {
            get => Object.inputs;
            set => Object.inputs = value;
        }
    }
}