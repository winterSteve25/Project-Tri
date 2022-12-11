using UI;
using UI.Menu.InventoryMenu;
using UI.TextContents;
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
        
        private CraftingController _craftingController;
        private CraftingRecipe _recipe;
        private TextContent _textContent;
        private bool _isPointerOver;

        protected override void Start()
        {
            base.Start();
            _craftingController = CraftingController.Current;
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
            _craftingController.TryCraft(_recipe);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerOver = true;
            if (_recipe.IsInvalid) return;
            TooltipManager.Show(_textContent);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isPointerOver = false;
            TooltipManager.Hide();
        }

        private void RecipeChanged()
        {
            if (_recipe.IsInvalid) return;
            _textContent = TextContent.Titled(item.item.itemName)
                .AddText(craft)
                .AddItem(item)
                .AddText(requires)
                .AddItems(itemStacks: _recipe.inputs);
        }
    }
}