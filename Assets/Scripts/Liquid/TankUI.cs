using DG.Tweening;
using UI.TextContents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Liquid
{
    public class TankUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Slider slider;
        [SerializeField] private Image sliderFill;

        private Tank _tank;
        private bool _pointerOver;
        
        private void OnDestroy()
        {
            _tank.OnChanged -= TankChange;
        }

        public void Setup(Tank tank)
        {
            _tank = tank;
            slider.minValue = 0;
            slider.maxValue = tank.Capacity;
            tank.OnChanged += TankChange;
            TankChange();
        }
        
        private void TankChange()
        {
            if (_tank.Liquid.IsEmpty)
            {
                slider.DOValue(0, 0.5f).SetEase(Ease.InCubic);
                return;
            }
            
            sliderFill.color = _tank.Liquid.liquid.color;
            slider.DOValue(_tank.Liquid.volume, 0.5f)
                .SetEase(Ease.OutCubic);

            if (!_pointerOver) return;
            if (_tank.Liquid.IsEmpty)
            {
                TooltipManager.Show(TextContent.Titled("Empty"));
                return;
            }
            
            TooltipManager.Show(
                TextContent.Titled(_tank.Liquid.liquid.liquidName)
                    .AddText($"{_tank.Liquid.volume}/{_tank.Capacity}")
            );
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _pointerOver = true;
            
            if (_tank.Liquid.IsEmpty)
            {
                TooltipManager.Show(TextContent.Titled("Empty"));
                return;
            }
            
            TooltipManager.Show(
                TextContent.Titled(_tank.Liquid.liquid.liquidName)
                    .AddText($"{_tank.Liquid.volume}/{_tank.Capacity}")
            );
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _pointerOver = false;
            TooltipManager.Hide();
        }
    }
}