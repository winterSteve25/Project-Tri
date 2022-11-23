using DG.Tweening;
using UnityEngine;

namespace UI.Menu.Title
{
    public class TriangleAnimator : MonoBehaviour
    {
        private Vector3? _startingPos;
        private Quaternion? _startingRotation;

        private void OnEnable()
        {
            var transform1 = transform;
            var position = transform1.position;

            _startingPos ??= position;
            _startingRotation ??= transform1.rotation;
            
            position = _startingPos.Value;
            transform1.SetPositionAndRotation(position, _startingRotation.Value);
            transform1.localScale = Vector3.one;

            transform1.DOMoveY(position.y - 0.4f, 1.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutCubic);
            
            InvokeRepeating(nameof(Spin), 1.5f, 6f);
        }

        private void OnDisable()
        {
            transform.DOKill();
            CancelInvoke();
        }

        private void Spin()
        {
            transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.InOutCubic);

            transform.DOScale(1.5f, 1f)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => transform.DOScale(1f, 0.5f)
                    .SetEase(Ease.InOutCubic));
        }
    }
}