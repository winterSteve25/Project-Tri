using System.Collections.Generic;
using System.Linq;
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
        public static readonly List<Transform> ScaledObjects = new();

        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private float size = 6f;

        private readonly Vector3 _constantOfProportionality = new (6, 6);

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
            
            foreach (var obj in ScaledObjects.Where(obj => obj.gameObject.activeSelf))
            {
                obj.localScale = _constantOfProportionality / size;
            }
        }
    }
}