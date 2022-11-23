using System.Threading.Tasks;
using Items;
using Player;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Localization;

namespace Tiles.Container
{
    /// <summary>
    /// The behaviour of a container tile
    /// </summary>
    public class ContainerBehaviour : MachineTile, IChainedWorldData
    {
        [SerializeField] private float distanceBeforeAccessDenied;
        [SerializeField] private LocalizedString inventoryName;
        
        public Inventory Inventory { get; private set; }
        
        private PlayerInventory _playerInventory;
        private InventoryManager _inventoryManager;

        private void Awake()
        {
            Inventory = new Inventory(inventoryName);
            _playerInventory = FindObjectOfType<PlayerInventory>();
            _inventoryManager = InventoryManager.Current;
        }

        private void Update()
        {
            if (Vector2.Distance(_playerInventory.transform.position, transform.position) > distanceBeforeAccessDenied)
            {
                ExitMenu();
            }
        }

        public override void OnInteract()
        {
            _inventoryManager.Show(Inventory);
        }

        public override void OnBroken()
        {
            ExitMenu();
        }

        private void ExitMenu()
        {
            // if currently displaying this inventory, close it when this is broken
            if (_inventoryManager.CurrentInventory == Inventory)
            {
                _inventoryManager.Show(null);
            }
        }

        public async Task Save(SaveTask saveTask)
        {
            await saveTask.Serialize(Inventory.ItemStacks);
        }

        public async Task Load(LoadTask loadTask)
        {
            Inventory.Load(await loadTask.Deserialize<ItemStack[]>());
        }
    }
}