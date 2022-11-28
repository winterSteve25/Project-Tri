using DG.Tweening;
using UI.Utilities;
using UnityEngine;
using Utils;

namespace UI.TextContents
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        private const float FadeDuration = 0.2f;

        [SerializeField] private CanvasGroup tooltip;

        private TextWrapper _textWrapper;

        private void Start()
        {
            tooltip.Disable();
            _textWrapper = tooltip.GetComponent<TextWrapper>();
        }

        public static void Show(TextContent textContent)
        {
            Instance.tooltip.DOKill();
            Instance.tooltip.gameObject.SetActive(true);

            foreach (Transform child in Instance.tooltip.transform)
            {
                Destroy(child.gameObject);
            }

            Instance._textWrapper.TextBoxes.Clear();
            textContent.Build(Instance.tooltip.transform);
            Instance._textWrapper.Refresh();
            Instance.tooltip.DOFade(1, FadeDuration);
        }

        public static void Hide()
        {
            if (!Instance.tooltip.gameObject.activeSelf) return;
            Instance.tooltip.DOKill();
            Instance.tooltip.FadeOut(FadeDuration);
        }
    }
}