using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Utilities
{
    [ExecuteInEditMode]
    public class SizeFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform[] fitTo;

        [HorizontalGroup("Vertical", width: 70), SerializeField, LabelWidth(50)] 
        private bool vertical;
        
        [DisableIf("@vertical"), HorizontalGroup("Vertical"), SerializeField, LabelWidth(80)] 
        private RectTransform[] fitToVertical;

        private RectTransform _transform;

        private void Start()
        {
            _transform = (RectTransform) transform;
        }

        private void Update()
        {
            var size = new Vector2();

            if (vertical && fitTo != null)
            {
                foreach (var t in fitTo)
                {
                    var rect = t.rect;
                    size.x += rect.width;
                    size.y += rect.height;
                }
            }

            if (!vertical)
            {
                if (fitTo != null)
                {
                    foreach (var t in fitTo)
                    {
                        var rect = t.rect;
                        size.x += rect.width;
                    }
                }

                if (fitToVertical != null)
                {
                    foreach (var t in fitToVertical)
                    {
                        var rect = t.rect;
                        size.y += rect.height;
                    }
                }
            }

            _transform.sizeDelta = size;
        }
    }
}