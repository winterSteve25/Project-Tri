using System.Threading.Tasks;
using Items;
using SaveLoad.Interfaces;
using SaveLoad.Tasks;
using Systems.Inv;
using UnityEngine;
using UnityEngine.Localization;

namespace TileBehaviours.Melter
{
    public class MelterBehaviour : CustomTileBehaviour, IChainedWorldData
    {
        [SerializeField] private LocalizedString inventoryName;

        private Inventory _inventory;
     
        private void Update()
        {
            _inventory = new Inventory(inventoryName, 1);
        }

        public async Task Save(SaveTask saveTask)
        {
            await saveTask.Serialize(_inventory.ItemStacks);
        }

        public async Task Load(LoadTask loadTask)
        {
            _inventory.Load(await loadTask.Deserialize<ItemStack[]>());
        }
    }
}