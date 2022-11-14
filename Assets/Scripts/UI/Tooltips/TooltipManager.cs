using DG.Tweening;
using UI.Utilities;
using UnityEngine;
using Utils;

namespace UI.Tooltips
{
    public class TooltipManager : Singleton<TooltipManager>
    {
        private const float FadeDuration = 0.2f;

        [SerializeField] private CanvasGroup tooltip;

        private Tooltip _currentTooltip;
        private TextWrapper _textWrapper;

        private void Start()
        {
            tooltip.alpha = 0;
            tooltip.gameObject.SetActive(false);
            _textWrapper = tooltip.GetComponent<TextWrapper>();
        }

        public static void Show(Tooltip tooltip)
        {
            Instance.tooltip.DOKill();
            Instance.tooltip.gameObject.SetActive(true);

            foreach (Transform child in Instance.tooltip.transform)
            {
                Destroy(child.gameObject);
            }

            Instance._textWrapper.TextBoxes.Clear();
            Instance._currentTooltip = tooltip;
            Instance._currentTooltip.Build(Instance.tooltip.transform);
            Instance.tooltip.DOFade(1, FadeDuration);
        }

        public static void Hide()
        {
            if (!Instance.tooltip.gameObject.activeSelf) return;
            Instance.tooltip.DOKill();
            Instance.tooltip.DOFade(0, FadeDuration)
                .OnComplete(() => Instance.tooltip.gameObject.SetActive(false));
        }
    }
}