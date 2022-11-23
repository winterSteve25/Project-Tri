using System.Linq;
using Items;
using Player;
using Registries;
using Sirenix.OdinInspector;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace Systems.Craft
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager current { get; private set; }

        [SerializeField] private RecipesRegistry recipes;
        [SerializeField] private Transform craftableItems;
        [SerializeField, AssetsOnly] private CraftableItemSlot craftingSlotPrefab;
        
        private PlayerInventory _playerInventory;
        private Inventory _playerInv;
        
        private void Awake()
        {
            current = this;
        }

        private void Start()
        {
            _playerInventory = FindObjectOfType<PlayerInventory>();
            _playerInv = _playerInventory.Inv;
            _playerInv.OnChanged += PlayerInventoryChanged;
            PlayerInventoryChanged();
        }
        
        private void OnDisable()
        {
            _playerInv.OnChanged -= PlayerInventoryChanged;
        }

        private void PlayerInventoryChanged()
        {
            foreach (Transform children in craftableItems)
            {
                Destroy(children.gameObject);
            }

            foreach (var recipe in recipes.Entries.Where(recipe => !recipe.Key.IsInvalid && recipe.Key.CanCraft(_playerInv)))
            {
                Instantiate(craftingSlotPrefab, craftableItems)
                    .Recipe = recipe.Key;
            }
        }

        public void TryCraft(CraftingRecipe recipe)
        {
            if (!recipe.CanCraft(_playerInv)) return;
            
            var inventoryManager = InventoryManager.Current;
            var draggedItemItem = inventoryManager.draggedItem.Item;
            
            // if dragging the same item
            if (draggedItemItem.item == recipe.result.item)
            {
                // if can not fit
                if (draggedItemItem.count + 1 > draggedItemItem.item.maxStackSize)
                {
                    return;
                }
                
                //create result
                inventoryManager.DragItem(draggedItemItem.Copy(count: draggedItemItem.count + 1));
            }
            else
            {
                // if currently dragging an item
                if (!draggedItemItem.IsEmpty)
                {
                    // put it in the player inventory inventory
                    _playerInv.Add(_playerInventory.transform.position, draggedItemItem);
                } 

                // create the result
                inventoryManager.DragItem(recipe.result);
            }
            
            // remove all the ingredients
            foreach (var ingredient in recipe.inputs)
            {
                _playerInv.Remove(ingredient);
            }
        }
    }
}