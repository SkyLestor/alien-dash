using UnityEngine;

namespace Scripts.Characters.Player.MovementStates
{
    public class PlayerWalkingState : PlayerMovementState
    {
        private readonly Vector3 _flippedRotation = new(0, 180, 0);

        public PlayerWalkingState(MovementController controller) : base(controller)
        {
        }

        public override void Enter()
        {
            Controller.Controller.AnimationsController.PlayRunAnimation();
        }

        public override void Update()
        {
            if (Controller.MoveDirection == Vector2.zero)
            {
                Controller.TransitionToState(Controller.IdleState);
            }
        }

        public override void FixedUpdate()
        {
            if (Controller.MoveDirection.x < 0)
            {
                Controller.transform.eulerAngles = _flippedRotation;
            }
            else if (Controller.MoveDirection.x > 0)
            {
                Controller.transform.eulerAngles = Vector3.zero;
            }

            Controller.Rigidbody.linearVelocity = Controller.MoveDirection * Controller.Speed;
        }

        public override void OnDash()
        {
            Controller.TransitionToState(Controller.DashingState);
        }
    }
}