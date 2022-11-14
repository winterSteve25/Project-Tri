using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Menu
{
    [Serializable]
    public class MenuTransition
    {
        public Vector2 movement;
        public float duration = 1f;

        public void Transition(Transform cameraTransform)
        {
            var localPos = cameraTransform.localPosition;
            cameraTransform.DOLocalMove(new Vector3(localPos.x + movement.x, localPos.y + movement.y, localPos.z), duration)
                .SetEase(Ease.InOutCubic);
        }

        public void TransitionBack(Transform cameraTransform)
        {
            var localPos = cameraTransform.localPosition;
            cameraTransform.DOLocalMove(new Vector3(localPos.x - movement.x, localPos.y - movement.y, localPos.z), duration)
                .SetEase(Ease.InOutCubic);
        }
    }
}