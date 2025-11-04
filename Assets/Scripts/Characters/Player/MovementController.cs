using Scripts.Characters.Player.MovementStates;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Characters.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MovementController : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        public PlayerController Controller { get; private set; }

        public PlayerMovementState CurrentState { get; private set; }

        // properties for states to use
        public Vector2 MoveDirection { get; private set; }
        public float Speed => 20f;
        public float InitialDashSpeed => 50f;
        public float DashSpeedDecreaseMultiplayer => 120f;
        public Rigidbody2D Rigidbody { get; private set; }
        public TrailRenderer TrailRenderer => _trailRenderer;
        public PlayerWalkingState WalkingState { get; private set; }
        public PlayerDashingState DashingState { get; private set; }
        public PlayerIdleState IdleState { get; private set; }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Controller = GetComponent<PlayerController>();
            WalkingState = new PlayerWalkingState(this);
            DashingState = new PlayerDashingState(this);
            IdleState = new PlayerIdleState(this);
            CurrentState = WalkingState;
        }

        private void Update()
        {
            CurrentState?.Update();
        }

        private void FixedUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            CurrentState?.OnDash();
        }

        public void TransitionToState(PlayerMovementState state)
        {
            CurrentState?.Exit();
            CurrentState = state;
            state?.Enter();
        }
    }
}