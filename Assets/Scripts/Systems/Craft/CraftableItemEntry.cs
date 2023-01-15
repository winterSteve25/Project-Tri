using UnityEngine;
using UnityEngine.Localization.Components;

namespace Systems.Craft
{
    public class CraftableItemEntry : MonoBehaviour
    {
        public CraftingRecipe Recipe
        {
            get => slot.Recipe;
            set
            {
                slot.Recipe = value;
                resultText.StringReference = value.result.item.itemName;
                resultText.RefreshString();
            }
        }

        [SerializeField] private LocalizeStringEvent resultText;
        [SerializeField] private CraftableItemSlot slot;

        public CraftableItemSlot Slot => slot;
    }
}