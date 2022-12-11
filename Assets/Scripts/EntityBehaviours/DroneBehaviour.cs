using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UI.TextContents;
using UnityEngine;

namespace EntityBehaviours
{
    public class DroneBehaviour : SelectableEntity
    {
        private static readonly int StartFlight = Animator.StringToHash("Start Flying");
        private static readonly int EndFlight = Animator.StringToHash("Stop Flying");

        [SerializeField, Required] 
        private Animator animator;

        [SerializeField, MinValue(1)] 
        private float flySpeedModifier;

        private Vector3 _destination;
        private bool _flying;

        private int _higherEntitiesLayer;
        private int _entitiesLayer;

        private TextContent _entityPanel;
        private TextMeshProUGUI _destinationField;
        
        private void Start()
        {
            _higherEntitiesLayer = SortingLayer.NameToID("Highup Entities");
            _entitiesLayer = SortingLayer.NameToID("Entities");
            _entityPanel = TextContent.Titled("Drone")
                .AddText(Vector2.zero.ToString(), onContentConstructed: field => _destinationField = field);
        }

        private void FlyTo(Vector3 destination)
        {
            if (_flying) return;
            
            _destination = destination;
            _destination.z = 0;
            if (_destinationField != null)
            {
                _destinationField.text = _destination.ToString();
            }
            
            animator.ResetTrigger(StartFlight);
            animator.ResetTrigger(EndFlight);
            StartCoroutine(FlyBehaviour());
            
            IEnumerator FlyBehaviour()
            {
                _flying = true;
            
                animator.SetTrigger(StartFlight);
                yield return new WaitForSeconds(0.35f);
                foreach (var sprite in spriteRenderers)
                {
                    sprite.sortingLayerID = _higherEntitiesLayer;
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
                foreach (var sprite in spriteRenderers)
                {
                    sprite.sortingLayerID = _entitiesLayer;
                }

                _flying = false;
            }
        }

        protected override TextContent BuildEntityPanel()
        {
            return _entityPanel;
        }
    }
}