using UI;
using UI.Tooltips;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

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
        
        [SerializeField] private LocalizedString craft;
        [SerializeField] private LocalizedString requires;
        
        private CraftingManager _craftingManager;
        private CraftingRecipe _recipe;
        private Tooltip _tooltip;
        private bool _isPointerOver;

        protected override void Start()
        {
            base.Start();
            _craftingManager = CraftingManager.current;
        }

        private void OnDestroy()
        {
            if (_isPointerOver)
            {
                TooltipManager.Hide();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_recipe.IsInvalid) return;
            _craftingManager.TryCraft(_recipe);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            if (_recipe.IsInvalid) return;
            TooltipManager.Show(_tooltip);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            TooltipManager.Hide();
        }

        private void RecipeChanged()
        {
            if (_recipe.IsInvalid) return;
            _tooltip = Tooltip.Empty()
                .AddText(craft)
                .AddItem(item)
                .AddText(requires)
                .AddItems(_recipe.inputs);
        }
    }
}