using Cinemachine;
using UnityEngine;
using Utils;

namespace Player
{
    /// <summary>
    /// Controls zooming of the camera
    /// </summary>
    public class PlayerCameraControl : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float size = 6f;

        [SerializeField] private Transform selection;

        private readonly Vector3 _constantOfProportionality = new (6, 6);
        private bool _isSelectionNull;

        private void Start()
        {
            _isSelectionNull = selection == null;
        }

        private void Update()
        {
            var multiplier = 2;
            
            if (GameInput.KeyboardKey(KeyCode.LeftShift))
            {
                multiplier *= 10;
            }

            if (GameInput.KeyboardKey(KeyCode.Minus))
            {
                size += Time.deltaTime * multiplier;
            }

            if (GameInput.KeyboardKey(KeyCode.Equals))
            {
                size -= Time.deltaTime * multiplier;
            }

            if (size == 0) return;
            size = Mathf.Clamp(size, 3, 18);
            virtualCamera.m_Lens.OrthographicSize = size;

            if (_isSelectionNull) return;
            selection.localScale = _constantOfProportionality / size;
        }
    }
}