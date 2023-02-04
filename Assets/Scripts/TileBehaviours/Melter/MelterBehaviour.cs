using System;
using System.Threading.Tasks;
using Items;
using Liquid;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using Systems.Heat;
using Systems.Inv;
using UI.Menu.InventoryMenu;
using UnityEngine;
using UnityEngine.Localization;
using Utils.Data;

namespace TileBehaviours.Melter
{
    public class MelterBehaviour : CustomTileBehaviour, IChainedWorldData
    {
        [SerializeField] private LocalizedString inventoryName;
        [SerializeField] private GameObject uiContent;
        [SerializeField] private MelterRecipes recipes;
            
        private Inventory _inventory;
        private Tank _tank;
        private Transform _playerTransform;
        
        private MelterRecipe _recipe;
        private bool _crafting;
        private float _progress;
        public float Progress => _progress;

        private MelterUI _ui;
        
        private void Awake()
        {
            _inventory = new Inventory(inventoryName, 1);
            _tank = new Tank();
            _inventory.OnChanged += StartCraft;
        }

        private void OnDisable()
        {
            _inventory.OnChanged -= StartCraft;
        }

        private void StartCraft()
        {
            var itemStack = _inventory[0];
            if (HeatManager.Current.GetHeatAt(Pos2D) < 1) return; 
            if (!recipes.HasRecipe(itemStack)) return;
            var recipe = recipes.FindRecipe(itemStack);
            if (!_tank.CanAdd(recipe.output)) return;
            _recipe = recipe;
            _crafting = true;
            if (_ui == null) return;
            _ui.SetupRecipe(_recipe);
        }

        private void Update()
        {
            if (_crafting)
            {
                _progress += Time.deltaTime;
                if (_progress >= _recipe.duration)
                {
                    _progress = 0;
                    _tank.Add(_recipe.output);
                    _inventory.Remove(_recipe.input);
                    _crafting = false;
                    StartCraft();
                }
            }

            if (_playerTransform == null) return;
            if (Vector2.Distance(_playerTransform.transform.position, transform.position) > Statistics.AccessDistance)
            {
                ExitMenu();
            }
        }

        public override void OnBroken()
        {
            DropInventory(_inventory);
            ExitMenu();
        }

        public override void OnInteract(Transform playerTransform)
        {
            var content = ExtraTabController.Current.EnableExtraTab(inventoryName, uiContent, true, true, _inventory);
            if (content != null)
            {
                _ui = content.GetComponent<MelterUI>();
                _ui.Setup(this, _inventory, _tank);
                if (!_crafting) return;
                _ui.SetupRecipe(_recipe);
            }
        }

        private void ExitMenu()
        {
            // if currently displaying this inventory, close it when this is broken
            if (ExtraTabController.Current.EnabledTabContent == uiContent)
            {
                ExtraTabController.Current.DisableExtraTab();
            }
        }
        
        public async Task Save(SaveTask saveTask)
        {
            await _inventory.Serialize(saveTask);
            await _tank.Serialize(saveTask);
        }

        public async Task Load(LoadTask loadTask)
        {
            await _inventory.Deserialize(loadTask);
            await _tank.Deserialize(loadTask);
        }
    }
}