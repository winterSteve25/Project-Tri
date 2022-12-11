using System;
using DG.Tweening;
using TMPro;
using UI.Utilities;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using Utils;
using Object = UnityEngine.Object;

namespace UI.TextContents
{
    [Serializable]
    public class TextContentHelper
    {
        private const float FadeDuration = 0.2f;

        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private LocalizeStringEvent localizedTitle;
        [FormerlySerializedAs("contentWhole")] 
        [SerializeField] private CanvasGroup contentContainer;
        [SerializeField] private Transform contentArea;
        [SerializeField] private bool firstChildIsTitle = true;
        
        public TextWrapper ContentTextWrapper => contentArea.GetComponent<TextWrapper>();
        private TextWrapper _textWrapper;
        
        public void DisableTooltip()
        {
            contentContainer.Disable();
        }

        public void EnableTooltip(TextContent textContent)
        {
            contentContainer.DOKill();
            contentContainer.gameObject.SetActive(true);

            for (var i = 0; i < contentArea.childCount; i++)
            {
                if (i == 0 && firstChildIsTitle) continue;
                Object.Destroy(contentArea.GetChild(i).gameObject);
            }

            _textWrapper ??= ContentTextWrapper;
            _textWrapper.TextBoxes.Clear();
            textContent.Build(localizedTitle, title, contentArea);
            _textWrapper.Refresh();
            contentContainer.DOFade(1, FadeDuration);
        }

        public void HideTooltip()
        {
            if (!contentContainer.gameObject.activeSelf) return;
            contentContainer.DOKill();
            contentContainer.FadeOut(FadeDuration);
        }
    }
}