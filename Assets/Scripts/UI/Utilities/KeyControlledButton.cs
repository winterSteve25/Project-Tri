using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Utilities
{
    public class KeyControlledButton : MonoBehaviour
    {
    
        [SerializeField] private KeyCode keyCode;
        [SerializeField] private Button button;
        
        private bool _keyWasDown;
        private EventSystem _eventSystem;

        private void Start()
        {
            _eventSystem = EventSystem.current;
        }

        private void Update()
        {
            var keyIsDown = Input.GetKey(keyCode);

            if (keyIsDown && !_keyWasDown)
            {
                button.OnPointerEnter(new PointerEventData(_eventSystem));
            }

            if (_keyWasDown && !keyIsDown)
            {
                var data = new PointerEventData(_eventSystem);
                button.OnPointerClick(data);
                button.OnPointerExit(data);
            }

            _keyWasDown = keyIsDown;
        }
    }
}