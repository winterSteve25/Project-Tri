﻿using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Menu
{
    [Serializable]
    public class MenuTransition
    {
        public Vector2 movement;
        public float duration = 1f;

        public void Transition(Transform cameraTransform)
        {
            var eventSystem = EventSystem.current;
            eventSystem.enabled = false;
            var localPos = cameraTransform.localPosition;
            cameraTransform.DOLocalMove(new Vector3(localPos.x + movement.x, localPos.y + movement.y, localPos.z), duration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => eventSystem.enabled = true);
        }

        public void TransitionBack(Transform cameraTransform)
        {
            var eventSystem = EventSystem.current;
            eventSystem.enabled = false;
            var localPos = cameraTransform.localPosition;
            cameraTransform.DOLocalMove(new Vector3(localPos.x - movement.x, localPos.y - movement.y, localPos.z), duration)
                .SetEase(Ease.InOutCubic)
                .OnComplete(() => eventSystem.enabled = true);
        }
    }
}