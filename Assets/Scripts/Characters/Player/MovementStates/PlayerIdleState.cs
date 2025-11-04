using UnityEngine;

namespace Scripts.Characters.Player.MovementStates
{
    public class PlayerIdleState : PlayerMovementState
    {
        public PlayerIdleState(MovementController controller) : base(controller)
        {
        }


        public override void Enter()
        {
            Controller.Rigidbody.linearVelocity = Vector2.zero;
            Controller.Controller.AnimationsController.PlayIdleAnimation();
        }

        public override void Update()
        {
            if (Controller.MoveDirection != Vector2.zero)
            {
                Controller.TransitionToState(Controller.WalkingState);
            }
        }

        public override void OnDash()
        {
            Controller.TransitionToState(Controller.DashingState);
        }
    }
}