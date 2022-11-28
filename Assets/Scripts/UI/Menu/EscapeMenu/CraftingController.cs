﻿using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using Registries;
using Sirenix.OdinInspector;
using Systems.Craft;
using Systems.Inv;
using TMPro;
using UI.Managers;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI.Menu.EscapeMenu
{
    public class CraftingController : CurrentInstanced<CraftingController>
    {
        [SerializeField, Required]
        private RecipesRegistry recipes;
        
        [SerializeField, Required]
        private Transform craftableItemsParent;

        [SerializeField, Required, AssetsOnly] 
        private CraftableItemEntry craftableItemEntryPrefab;

        [SerializeField, Required] 
        private TMP_InputField searchBar;
        
        private List<CraftableItemEntry> _craftableItems;
        private Inventory _inventory;

        private void Start()
        {
            _craftableItems = new List<CraftableItemEntry>();
            searchBar.onValueChanged.AddListener(SearchItem);
            
            _inventory = FindObjectOfType<PlayerInventory>().Inv;
            _inventory.OnChanged += PlayerInventoryChanged;
            PlayerInventoryChanged();
        }
        
        private void OnDisable()
        {
            _inventory.OnChanged -= PlayerInventoryChanged;
        }

        private void PlayerInventoryChanged()
        {
            foreach (Transform children in craftableItemsParent)
            {
                Destroy(children.gameObject);
            }
            
            _craftableItems.Clear();

            foreach (var recipe in recipes.Entries.Where(recipe => !recipe.Key.IsInvalid && recipe.Key.CanCraft(_inventory)))
            {
                var entry = Instantiate(craftableItemEntryPrefab, craftableItemsParent);
                entry.Recipe = recipe.Key;
                _craftableItems.Add(entry);
            }
        }

        public void TryCraft(CraftingRecipe recipe)
        {
            if (!recipe.CanCraft(_inventory)) return;
            
            var dragController = ItemDragManager.Current;
            var draggedItemItem = dragController.DraggedItem.Item;
            
            // if dragging the same item
            if (draggedItemItem.item == recipe.result.item)
            {
                // if can not fit
                if (draggedItemItem.count + 1 > draggedItemItem.item.maxStackSize)
                {
                    return;
                }
                
                //create result
                dragController.DragItem(draggedItemItem.Copy(count: draggedItemItem.count + 1));
            }
            else
            {
                // if currently dragging an item
                if (!draggedItemItem.IsEmpty)
                {
                    // we do not craft
                    return;
                }

                // create the result
                dragController.DragItem(recipe.result);
            }
            
            // remove all the ingredients
            foreach (var ingredient in recipe.inputs)
            {
                _inventory.Remove(ingredient);
            }
        }

        private void SearchItem(string itemName)
        {
            foreach (var entry in _craftableItems)
            {
                if (string.IsNullOrEmpty(itemName) || entry.Recipe.result.item.itemName.GetLocalizedString().Contains(itemName, StringComparison.CurrentCultureIgnoreCase))
                {
                    entry.gameObject.SetActive(true);
                    continue;
                }
                
                entry.gameObject.SetActive(false);
            }

            craftableItemsParent.parent.GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
        }
    }
}