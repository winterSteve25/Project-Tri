using UnityEngine;
using Utils;

namespace Player
{
    /// <summary>
    /// Controls the player movement
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _dir;
        
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _dir = new Vector2();
        }

        private void Update()
        {
            _dir.x = GameInput.GetAxis("Horizontal");
            _dir.y = GameInput.GetAxis("Vertical");
        }

        private void FixedUpdate()
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + _dir * (speed * Time.deltaTime));
        }
    }
}