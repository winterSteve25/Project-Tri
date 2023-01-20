using System.Threading.Tasks;
using Liquid;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
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
            
        private Inventory _inventory;
        private Tank _tank;
        private Transform _playerTransform;

        private void Awake()
        {
            _inventory = new Inventory(inventoryName, 1);
            _tank = new Tank();
        }

        private void Update()
        {
            if (_playerTransform == null) return;
            if (Vector2.Distance(_playerTransform.transform.position, transform.position) > Statistics.AccessDistance)
            {
                ExitMenu();
            }
        }

        public override void OnInteract(Transform playerTransform)
        {
            var content = ExtraTabController.Current.EnableExtraTab(inventoryName, uiContent, true, true);
            if (content != null) content.GetComponent<MelterUI>().Setup(_inventory, _tank);
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