namespace Scripts.Characters.Player.MovementStates
{
    public class PlayerWalkingState : PlayerMovementState
    {
        public PlayerWalkingState(MovementController controller) : base(controller)
        {
        }

        public override void FixedUpdate()
        {
            Controller.Rigidbody.linearVelocity = Controller.MoveDirection * Controller.Speed;
        }

        public override void OnDash()
        {
            Controller.TransitionToState(Controller.DashingState);
        }
    }
}