using DG.Tweening;
using TMPro;
using UI.Utilities;
using UnityEngine;
using UnityEngine.Localization.Components;
using Utils;

namespace UI.TextContents
{
    public class TooltipManager : Singleton<TooltipManager>
    {

        [SerializeField] private TextContentHelper textContentHelper;
        
        private void Start()
        {
            textContentHelper.DisableTooltip();
        }

        public static void Show(TextContent textContent)
        {
            Instance.textContentHelper.EnableTooltip(textContent);
        }

        public static void Hide()
        {
            Instance.textContentHelper.HideTooltip();
        }
    }
}