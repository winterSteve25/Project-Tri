using UnityEngine;
using Utils;

namespace UI.TextContents
{
    public class EntityPanelManager : Singleton<EntityPanelManager>
    {
        [SerializeField] private TextContentHelper textContentHelper;
        
        private static TextContent _shownContent;

        private void Start()
        {
            textContentHelper.DisableTooltip();
        }

        public static void Show(TextContent textContent)
        {
            if (_shownContent == textContent)
            {
                Hide();
                return;
            }
            
            Instance.textContentHelper.EnableTooltip(textContent);
        }

        public static void Hide()
        {
            _shownContent = null;
            Instance.textContentHelper.HideTooltip();
        }
    }
}