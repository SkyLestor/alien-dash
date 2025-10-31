using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
        private const float InitialDashSpeed = 50f;
        private const float DashSpeedDecreaseMultiplayer = 120f;


        [SerializeField] private float _speed = 20f;
        private Vector2 _dashDirection;
        private float _dashSpeed;

        private bool _isDashing;
        private Vector2 _moveDirection;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_isDashing)
            {
                _dashSpeed -= Time.deltaTime * DashSpeedDecreaseMultiplayer;
                if (_dashSpeed <= 0)
                {
                    _isDashing = false;
                }
            }
        }


        private void FixedUpdate()
        {
            if (!_isDashing)
            {
                _rigidbody2D.linearVelocity = _moveDirection * _speed;
            }
            else
            {
                _rigidbody2D.linearVelocity = _dashDirection * _dashSpeed;
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveDirection = context.ReadValue<Vector2>();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            _dashDirection = _moveDirection != Vector2.zero
                ? _moveDirection
                : new Vector2(transform.right.x, transform.right.y).normalized;
            _dashSpeed = InitialDashSpeed;
            _isDashing = true;
        }
    }
}