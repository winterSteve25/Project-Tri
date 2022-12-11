using DG.Tweening;
using UnityEngine;
using Utils;

namespace UI.Managers
{
    public class UIManager : CurrentInstanced<UIManager>
    {
        private CanvasGroup _activeUI;

        public static void ToggleUI(CanvasGroup canvasGroup, float fadeDuration = 0.2f, Ease fadeEase = Ease.Linear)
        {
            if (Current._activeUI == canvasGroup)
            {
                Current._activeUI.FadeOut(fadeDuration, onComplete: () =>
                {
                    Current._activeUI = null;
                });
                return;
            }
            
            if (Current._activeUI != null)
            {
                Current._activeUI.FadeOut(fadeDuration, onComplete: () =>
                {
                    Current._activeUI = canvasGroup;
                    Current._activeUI.FadeIn(fadeDuration, fadeEase);
                });
            }
            else
            {
                Current._activeUI = canvasGroup;
                Current._activeUI.FadeIn(fadeDuration, fadeEase);
            }
        }

        public static void CloseUI(float fadeDuration = 0.2f, Ease fadeEase = Ease.Linear)
        {
            if (Current._activeUI == null) return;
            Current._activeUI.FadeOut(fadeDuration, fadeEase);
            Current._activeUI = null;
        }
    }
}