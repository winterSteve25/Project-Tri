using System.Linq;
using Items;
using Player;
using Systems.Inv;
using UnityEngine;

namespace Systems.Craft
{
    public class CraftingManager : MonoBehaviour
    {
        public static CraftingManager current { get; private set; }

        [SerializeField] private RecipesRegistry recipes;
        [SerializeField] private Transform craftableItems;
        [SerializeField] private CraftableItemSlot craftingSlotPrefab;
        
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

            foreach (var recipe in recipes.Recipes.Where(recipe => recipe.CanCraft(_playerInv)))
            {
                Instantiate(craftingSlotPrefab, craftableItems)
                    .Recipe = recipe;
            }
        }

        public void TryCraft(CraftingRecipe recipe)
        {
            if (!recipe.CanCraft(_playerInv)) return;
            
            var inventoryManager = InventoryManager.current;
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
                inventoryManager.DragItem(new ItemStack(draggedItemItem.item, draggedItemItem.count + 1));
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