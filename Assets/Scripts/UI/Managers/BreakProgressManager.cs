using System;
using Items;
using Items.ItemTypes;
using UI.Menu.EscapeMenu;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Managers
{
    public class BreakProgressManager : MonoBehaviour
    {
        [SerializeField] private Sprite[] breakStages;
        [SerializeField] private Image tileMask;
        [SerializeField] private Image breakProgress;

        private EquipmentsController _equipments;

        private void Start()
        {
            _equipments = EquipmentsController.Current;
        }

        private void Update()
        {
            var item = _equipments[EquipmentType.Outer];
            var miningTile = item.CustomData.GetOrDefault(MiningItem.CurrentlyMiningTile, null);
            
            if (miningTile == null)
            {
                tileMask.gameObject.SetActive(false);
                return;    
            }

            if (!tileMask.gameObject.activeSelf)
            {
                tileMask.gameObject.SetActive(true);
                breakProgress.sprite = breakStages[0];
            }

            var percentageDone = miningTile.BreakProgress / miningTile.Tile.hardness;
            var spriteUsed = Mathf.FloorToInt(breakStages.Length * percentageDone);
            tileMask.sprite = miningTile.Tile.Sprite;

            if (spriteUsed > 0 && spriteUsed < breakStages.Length)
            {
                breakProgress.sprite = breakStages[spriteUsed];
            }
        }
    }
}