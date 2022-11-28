using UnityEngine;
using UnityEngine.UI;

namespace UI.Utilities
{
    public class ScrollRectReset : MonoBehaviour
    {
        private bool _completed;

        private void Update()
        {
            if (!_completed)
            {
                GetComponent<ScrollRect>().verticalNormalizedPosition = 1;
                _completed = true;
                Destroy(this);
            }
        }
    }
}