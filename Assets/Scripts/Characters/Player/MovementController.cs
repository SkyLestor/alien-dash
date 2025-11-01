using Scripts.Characters.Player.MovementStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TrailRenderer))]
    public class MovementController : MonoBehaviour
    {
        private PlayerMovementState _currentState;

        // properties for states to use
        public Vector2 MoveDirection { get; private set; }
        public float Speed => 20f;
        public float InitialDashSpeed => 50f;
        public float DashSpeedDecreaseMultiplayer => 120f;
        public Rigidbody2D Rigidbody { get; private set; }
        public TrailRenderer TrailRenderer { get; private set; }
        public PlayerWalkingState WalkingState { get; private set; }
        public PlayerDashingState DashingState { get; private set; }
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            TrailRenderer = GetComponent<TrailRenderer>();
            WalkingState = new PlayerWalkingState(this);
            DashingState = new PlayerDashingState(this);
            _currentState = WalkingState;
        }

        private void Update()
        {
            _currentState?.Update();
        }

        private void FixedUpdate()
        {
            _currentState?.FixedUpdate();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            _currentState?.OnDash();
        }

        public void TransitionToState(PlayerMovementState state)
        {
            _currentState?.Exit();
            _currentState = state;
            state?.Enter();
        }
    }
}