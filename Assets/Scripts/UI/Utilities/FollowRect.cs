using UnityEngine;

namespace UI.Utilities
{
    public class FollowRect : MonoBehaviour
    {
        [SerializeField] private Transform toFollow;
        [SerializeField] private float followSpeed;
        [SerializeField] private AnimationCurve followEase; 

        public void Follow(Transform followedObject, float speed = -1)
        {
            toFollow = followedObject;
            if (speed > 0)
            {
                followSpeed = speed;
            }
        }
        
        private void Update()
        {
            if (toFollow == null) return;
            transform.position = Vector2.Lerp(transform.position, toFollow.position, followEase.Evaluate(Time.deltaTime * followSpeed));
        }
    }
}