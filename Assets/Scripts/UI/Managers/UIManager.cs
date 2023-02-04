using System;
using DG.Tweening;
using UnityEngine;
using Utils;

namespace UI.Managers
{
    public class UIManager : CurrentInstanced<UIManager>
    {
        public delegate void UIStatusChange(CanvasGroup menu, bool closed);
        public event UIStatusChange OnUIStatusChanged;
        
        private CanvasGroup _activeUI;
        
        /// <summary>
        /// If given canvas group is is the same as the current ui the current UI will be faded out, if it is different, if they are different the current will close and the new one will open
        /// </summary>
        /// <param name="canvasGroup"></param>
        /// <param name="fadeDuration"></param>
        /// <param name="fadeEase"></param>
        /// <param name="onlyOpenIfCurrentIsNull"></param>
        /// <returns>True if closed current UI, false if didn't</returns>
        public static bool ToggleUI(CanvasGroup canvasGroup, float fadeDuration = 0.2f, Ease fadeEase = Ease.Linear, bool onlyOpenIfCurrentIsNull = false)
        {
            if (Current._activeUI == canvasGroup)
            {
                Current.OnUIStatusChanged?.Invoke(canvasGroup, true);
                Current._activeUI.FadeOut(fadeDuration, onComplete: () =>
                {
                    Current._activeUI = null;
                });
                return true;
            }
            
            if (Current._activeUI != null)
            {
                Current.OnUIStatusChanged?.Invoke(Current._activeUI, true);
                Current._activeUI.FadeOut(fadeDuration, onComplete: () =>
                {
                    if (onlyOpenIfCurrentIsNull)
                    {
                        Current._activeUI = null;
                        return;
                    }

                    Current._activeUI = canvasGroup;
                    Current.OnUIStatusChanged?.Invoke(Current._activeUI, false);
                    Current._activeUI.FadeIn(fadeDuration, fadeEase);
                });
            }
            else
            {
                Current._activeUI = canvasGroup;
                Current.OnUIStatusChanged?.Invoke(Current._activeUI, false);
                Current._activeUI.FadeIn(fadeDuration, fadeEase);
            }

            return false;
        }
        
        public static void CloseUI(float fadeDuration = 0.2f, Ease fadeEase = Ease.Linear)
        {
            if (Current._activeUI == null) return;
            Current._activeUI.FadeOut(fadeDuration, fadeEase);
            Current._activeUI = null;
        }
    }
}