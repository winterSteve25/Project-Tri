using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Utilities
{
    public class SwapSpriteOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Sprite normalSprite;
        [SerializeField] private Sprite hoverSprite;
        [SerializeField] private Image image;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            image.sprite = hoverSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            image.sprite = normalSprite;
        }
    }
}