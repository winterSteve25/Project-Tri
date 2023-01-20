using System.Threading.Tasks;
using Items;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using Systems.Inv;
using UI.Menu.InventoryMenu;
using UnityEngine;
using UnityEngine.Localization;
using Utils.Data;

namespace TileBehaviours.Container
{
    /// <summary>
    /// The behaviour of a container tile
    /// </summary>
    public class ContainerBehaviour : CustomTileBehaviour, IChainedWorldData
    {
        [SerializeField] private LocalizedString inventoryName;
        
        public Inventory Inventory { get; private set; }
        private InventoryTabController _inventoryTabController;
        private Transform _playerTransform;
        
        private void Awake()
        {
            Inventory = new Inventory(inventoryName, 15);
            _inventoryTabController = InventoryTabController.Current;
        }
        
        private void Update()
        {
            if (_playerTransform == null) return;
            if (Vector2.Distance(_playerTransform.position, transform.position) > Statistics.AccessDistance)
            {
                ExitMenu();
            }
        }

        public override void OnInteract(Transform playerTransform)
        {
            _playerTransform = playerTransform;
            if (Vector2.Distance(_playerTransform.position, transform.position) > Statistics.AccessDistance) return;
                _inventoryTabController.SetOpenedInventory(Inventory);
        }

        public override void OnBroken()
        {
            var pos = transform.position;
            
            foreach (var item in Inventory.ItemStacks)
            {
                ItemSpawner.Current.SpawnApproximatelyAt(pos, item);
            }
            
            ExitMenu();
        }

        private void ExitMenu()
        {
            // if currently displaying this inventory, close it when this is broken
            if (_inventoryTabController.OpenedInventory == Inventory)
            {
                _inventoryTabController.SetOpenedInventory(null);
            }
        }
        
        public async Task Save(SaveTask saveTask)
        {
            await Inventory.Serialize(saveTask);
        }

        public async Task Load(LoadTask loadTask)
        {
            await Inventory.Deserialize(loadTask);
        }
    }
}