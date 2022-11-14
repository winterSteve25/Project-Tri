using UI;
using UI.Tooltips;
using UnityEngine.EventSystems;

namespace Systems.Craft
{
    public class CraftableItemSlot : UIItem, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public CraftingRecipe Recipe
        {
            get => _recipe;
            set
            {
                _recipe = value;
                Item = _recipe.result;
                RecipeChanged();
            }
        }

        private CraftingManager _craftingManager;
        private CraftingRecipe _recipe;
        private Tooltip _tooltip;

        protected override void Start()
        {
            base.Start();
            _craftingManager = CraftingManager.current;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_recipe.IsInvalid) return;
            _craftingManager.TryCraft(_recipe);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_recipe.IsInvalid) return;
            TooltipManager.Show(_tooltip);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipManager.Hide();
        }

        private void RecipeChanged()
        {
            if (_recipe.IsInvalid) return;
            _tooltip = Tooltip.Empty()
                .AddText("Craft: ")
                .AddItem(item)
                .AddText("Requires: ")
                .AddItems(_recipe.inputs);
        }
    }
}