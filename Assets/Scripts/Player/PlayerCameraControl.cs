using Cinemachine;
using UnityEngine;

namespace Player
{
    public class PlayerCameraControl : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float size = 6f;

        private void Update()
        {
            var multiplier = 2;
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                multiplier *= 10;
            }

            if (Input.GetKey(KeyCode.Minus))
            {
                size += Time.deltaTime * multiplier;
            }

            if (Input.GetKey(KeyCode.Equals))
            {
                size -= Time.deltaTime * multiplier;
            }

            if (size == 0) return;
            
            size = Mathf.Clamp(size, 3, 18);
            selection.localScale = _constantOfProportionality / size;
            
            virtualCamera.m_Lens.OrthographicSize = size;
        }

        [SerializeField] private Transform selection;

        private readonly Vector3 _constantOfProportionality = new (6, 6);
    }
}