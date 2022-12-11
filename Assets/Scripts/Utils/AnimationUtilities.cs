using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UI.Managers;
using UnityEngine;

namespace Utils
{
    public static class AnimationUtilities
    {
        public static void Disable(this CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0;
            canvasGroup.gameObject.SetActive(false);
        }
        
        public static TweenerCore<float, float, FloatOptions> FadeToggle(this CanvasGroup canvasGroup, float fadeDuration, Ease ease = Ease.Linear)
        {
            return canvasGroup.gameObject.activeSelf ? canvasGroup.FadeOut(fadeDuration, ease) : canvasGroup.FadeIn(fadeDuration, ease);
        }
        
        public static TweenerCore<float, float, FloatOptions> FadeIn(this CanvasGroup canvasGroup, float fadeDuration, Ease ease = Ease.Linear)
        {
            canvasGroup.gameObject.SetActive(true);
            return canvasGroup.DOFade(1, fadeDuration)
                .SetEase(ease);
        }
        
        public static TweenerCore<float, float, FloatOptions> FadeOut(this CanvasGroup canvasGroup, float fadeDuration, Ease ease = Ease.Linear, Action onComplete = null)
        {
            return canvasGroup.DOFade(0, fadeDuration)
                .SetEase(ease)
                .OnComplete(() =>
                {
                    canvasGroup.gameObject.SetActive(false);
                    onComplete?.Invoke();
                });
        }

        public static void PushUI(this CanvasGroup canvasGroup, float fadeDuration)
        {
            UIManager.ToggleUI(canvasGroup, fadeDuration);   
        }
    }
}