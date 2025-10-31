using Scripts.Input;
using UnityEngine;
using Zenject;

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

        private IInputManager _inputManager;

        private bool _isDashing;
        private Rigidbody2D _rigidbody2D;


        [Inject]
        public void Construct(IInputManager inputManager)
        {
            _inputManager = inputManager;
        }

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
                _rigidbody2D.linearVelocity = _inputManager.GetMovementVectorNormalized() * _speed;
            }
            else
            {
                _rigidbody2D.linearVelocity = _dashDirection * _dashSpeed;
            }
        }

        private void OnEnable()
        {
            _inputManager.DashPerformed += InputManagerOnDashPerformed;
        }

        private void OnDisable()
        {
            _inputManager.DashPerformed += InputManagerOnDashPerformed;
        }

        private void InputManagerOnDashPerformed()
        {
            _dashDirection = _inputManager.GetMovementVectorNormalized() != Vector2.zero
                ? _inputManager.GetMovementVectorNormalized()
                : new Vector2(transform.right.x, transform.right.y).normalized;
            _dashSpeed = InitialDashSpeed;
            _isDashing = true;
        }
    }
}