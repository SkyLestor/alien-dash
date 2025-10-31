using Scripts.Input;
using UnityEngine;
using Zenject;

namespace Scripts.Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private float _speed = 20f;

        private IInputManager _inputManager;
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

        private void FixedUpdate()
        {
            _rigidbody2D.linearVelocity = _inputManager.GetMovementVectorNormalized() * _speed;
        }
    }
}