using UnityEngine;

namespace UI.Utilities
{
    public class FollowRectWithOffset : MonoBehaviour
    {
        [SerializeField] private Transform toFollow;
        [SerializeField] private float followSpeed;
        [SerializeField] private AnimationCurve followEase;

        private Vector3 _offset;
        private Vector3 _startingOffset;
        private bool _offsetted;

        public void Follow(Transform followedObject, Vector3 startingOffset, Vector3 offset, float speed = -1)
        {
            toFollow = followedObject;
            if (speed > 0)
            {
                followSpeed = speed;
            }

            _offset = offset;
            _startingOffset = startingOffset;
            _offsetted = false;
        }

        private void Update()
        {
            if (toFollow == null) return;

            var followPosition = toFollow.position;

            if (!_offsetted)
            {
                transform.position = followPosition + _startingOffset;
                _offsetted = true;
            }

            transform.position = Vector2.Lerp(transform.position, followPosition + _offset, followEase.Evaluate(Time.deltaTime * followSpeed));
        }
    }
}