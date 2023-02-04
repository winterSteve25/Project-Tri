using System;
using DG.Tweening;
using Liquid;
using Systems.Inv;
using UnityEngine;
using UnityEngine.UI;

namespace TileBehaviours.Melter
{
    public class MelterUI : MonoBehaviour
    {
        [SerializeField] private ItemSlot slot;
        [SerializeField] private Slider progressMeter;
        [SerializeField] private TankUI tankUI;
        
        private MelterBehaviour _melter;
        private Inventory _inventory;

        public void Setup(MelterBehaviour melter, Inventory inventory, Tank tank)
        {
            _melter = melter;
            _inventory = inventory;
            
            slot.Init(inventory, 0);
            inventory.OnChanged += InvChange;
            InvChange();
            
            tankUI.Setup(tank);
        }

        public void SetupRecipe(MelterRecipe recipe)
        {
            progressMeter.maxValue = recipe.duration;
            progressMeter.minValue = 0;
        }

        private void OnDestroy()
        {
            _inventory.OnChanged -= InvChange;
        }

        private void Update()
        {
            progressMeter.value = _melter.Progress;
        }

        private void InvChange()
        {
            slot.Item = _inventory[0];
        }
    }
}