using Player.Interaction;
using Sirenix.OdinInspector;
using UI.Menu.InventoryMenu;
using UnityEngine;
using Utils;
using World.Tiles;

namespace Items.ItemTypes
{
    public class PrefabSpawnerItem : TriItem, IClickedBehaviourItem
    {
        [BoxGroup(GeneralInformationBox)]
        [VerticalGroup(VerticalMain)]
        [SerializeField, AssetsOnly]
        private GameObject prefab;

        public void Click(MouseButton mouseButton, ref ItemStack itemStack, TileInstance tileClicked, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryTabController inventoryTabController,
            EquipmentsController equipmentsController, Vector3 playerPosition)
        {
            // spawn the prefab
            var spawned = Instantiate(prefab);
            spawned.transform.position = clickedPos;

            // consume
            var item = equipmentsController[EquipmentType.Outer];
            var itemCount = item.count - 1;
            equipmentsController[EquipmentType.Outer] = itemCount <= 0 ? ItemStack.Empty : item.Copy(count: itemCount);
        }

        public bool CanInteract(ref ItemStack itemStack, TileInstance tileAtLocation, Vector3 clickedPos,
            Vector3Int pos, TilemapManager tilemapManager, InventoryTabController inventoryTabController,
            EquipmentsController equipmentsController, Vector3 playerPosition)
        {
            return true;
        }
    }
}