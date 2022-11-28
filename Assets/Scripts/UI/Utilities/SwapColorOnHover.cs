using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Utilities
{
    public class SwapColorOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, Required] 
        private Color normalColor;

        [SerializeField, Required] 
        private Color hoverColor;

        [SerializeField, Required]
        private Image image;

        [SerializeField]
        private float lerpDuration = 0.5f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            image.DOColor(hoverColor, lerpDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.DOColor(normalColor, lerpDuration);
        }
    }
}