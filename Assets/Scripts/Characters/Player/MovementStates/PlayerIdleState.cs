using UnityEngine;

namespace Scripts.Characters.Player.MovementStates
{
    public class PlayerIdleState : PlayerMovementState
    {
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        public PlayerIdleState(MovementController controller) : base(controller)
        {
        }


        public override void Enter()
        {
            Controller.Animator.SetBool(IsRunning, false);
            Controller.Rigidbody.linearVelocity = Vector2.zero;
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