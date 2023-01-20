using Liquid;
using Systems.Inv;
using UnityEngine;
using UnityEngine.UI;

namespace TileBehaviours.Melter
{
    public class MelterUI : MonoBehaviour
    {
        [SerializeField] private ItemSlot slot;
        [SerializeField] private Slider slider;
        [SerializeField] private Image sliderFill;

        public void Setup(Inventory inventory, Tank tank)
        {
            slot.Init(inventory, 0);
            inventory.OnChanged += () =>
            {
                slot.Item = inventory[0];
            };

            slider.minValue = 0;
            slider.maxValue = tank.Capacity;
            
            tank.OnChanged += () =>
            {
                sliderFill.color = tank.Liquid.liquid.color;
                slider.value = tank.Liquid.volume;
            };
        }
    }
}