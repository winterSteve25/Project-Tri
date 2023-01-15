using Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Utilities
{
    public class SoundOnHover : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private PlayableAudio hoverSound;
        [SerializeField] private float fadeDuration = 0f;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (gameObject.TryGetComponent<Selectable>(out var selectable))
            {
                if (!selectable.IsInteractable()) return;
            }
            hoverSound?.Play(fadeDuration);
        }
    }
}