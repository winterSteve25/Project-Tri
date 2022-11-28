using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.TabsSystem
{
    public class TabButton : MonoBehaviour
    {
        [SerializeField, Required] 
        private TabManager tabManager;

        [SerializeField] 
        private CanvasGroup tabObject;

        public CanvasGroup TabObject => tabObject;
        
        private int _index;

        private void Start()
        {
            _index = tabManager.tabs.IndexOf(this);
        }

        public void OnClick()
        {
            tabManager.ClickedTab(_index);
        }
    }
}