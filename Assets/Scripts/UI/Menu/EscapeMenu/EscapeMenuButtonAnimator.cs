using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UI.Menu.EscapeMenu
{
    public class EscapeMenuButtonAnimator : MonoBehaviour
    {
        [SerializeField, Required] private RectTransform[] buttons;
        
        [SerializeField, BoxGroup("Configurations")] private float offset;
        [SerializeField, BoxGroup("Configurations")] private float moveTime;
        [SerializeField, BoxGroup("Configurations")] private float delayBetweenEach;

        private float[] _originalPositions;

        private void Start()
        {
            _originalPositions = new float[buttons.Length];
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                _originalPositions[i] = button.position.x;
            }
        }

        public void AnimateIn()
        {
            for (var i = 0; i < buttons.Length; i++)
            {
                var button = buttons[i];
                button.DOMoveX(_originalPositions[i] + offset, 0);
            }

            StartCoroutine(MoveDelayed());

            IEnumerator MoveDelayed()
            {
                for (var i = 0; i < buttons.Length; i++)
                {
                    var button = buttons[i];
                    button.DOMoveX(_originalPositions[i], moveTime)
                        .SetEase(Ease.OutCubic);
                    yield return new WaitForSeconds(delayBetweenEach);
                }
            }
        }

        public void AnimateOut()
        {
            StartCoroutine(MoveDelayed());
            
            IEnumerator MoveDelayed()
            {
                foreach (var button in buttons)
                {
                    button.DOMoveX(button.position.x + offset, moveTime)
                        .SetEase(Ease.InCubic);
                    yield return new WaitForSeconds(delayBetweenEach);
                }
            }
        }
    }
}