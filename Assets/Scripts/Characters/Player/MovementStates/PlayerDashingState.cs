using UnityEngine;

namespace Scripts.Characters.Player.MovementStates
{
    public class PlayerDashingState : PlayerMovementState
    {
        private Vector2 _dashDirection;
        private float _dashSpeed;

        public PlayerDashingState(MovementController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            _dashSpeed = Controller.InitialDashSpeed;
            _dashDirection = Controller.MoveDirection != Vector2.zero
                ? Controller.MoveDirection
                : new Vector2(Controller.transform.right.x, Controller.transform.right.y);
            Controller.TrailRenderer.emitting = true;
        }

        public override void Update()
        {
            _dashSpeed -= Time.deltaTime * Controller.DashSpeedDecreaseMultiplayer;
            if (_dashSpeed <= Controller.Speed)
            {
                Controller.TransitionToState(Controller.WalkingState);
            }
        }

        public override void FixedUpdate()
        {
            Controller.Rigidbody.linearVelocity = _dashDirection * _dashSpeed;
        }

        public override void Exit()
        {
            Controller.TrailRenderer.emitting = false;
        }
    }
}