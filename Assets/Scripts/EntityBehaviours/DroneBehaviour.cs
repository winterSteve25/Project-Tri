using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EntityBehaviours
{
    public class DroneBehaviour : MonoBehaviour
    {
        private static readonly int StartFlight = Animator.StringToHash("Start Flying");
        private static readonly int EndFlight = Animator.StringToHash("Stop Flying");

        [SerializeField, Required] 
        private Animator animator;

        [SerializeField, MinValue(1)] 
        private float flySpeedModifier;

        [SerializeField] 
        private SpriteRenderer[] sprites;

        private Vector3 _destination;
        private bool _flying;
        
        private void FlyTo(Vector3 destination)
        {
            if (_flying) return;
            _destination = destination;
            _destination.z = 0;
            animator.ResetTrigger(StartFlight);
            animator.ResetTrigger(EndFlight);
            StartCoroutine(FlyBehaviour());
        }

        private IEnumerator FlyBehaviour()
        {
            _flying = true;
            
            animator.SetTrigger(StartFlight);
            yield return new WaitForSeconds(0.35f);
            foreach (var sprite in sprites)
            {
                sprite.sortingLayerID = SortingLayer.NameToID("Highup Entities");
            }

            var transform1 = transform;
            var pos = transform1.position;
            pos.z = 0;
            var dir = _destination - pos;

            var rotationSpeed = flySpeedModifier * 0.1f;
            transform1.DORotateQuaternion(Quaternion.AngleAxis(Vector3.Angle(pos, _destination), Vector3.forward), rotationSpeed);
            yield return new WaitForSeconds(rotationSpeed);

            var duration = dir.magnitude / flySpeedModifier;
            transform1.DOMove(_destination, duration)
                .SetEase(Ease.InOutCubic);
            yield return new WaitForSeconds(duration);

            transform1.DORotate(Vector3.zero, rotationSpeed);
            yield return new WaitForSeconds(rotationSpeed);

            animator.SetTrigger(EndFlight);
            yield return new WaitForSeconds(0.35f);
            foreach (var sprite in sprites)
            {
                sprite.sortingLayerID = SortingLayer.NameToID("Entities");
            }

            _flying = false;
        }
    }
}