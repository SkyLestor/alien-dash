namespace Scripts.Characters.Player.MovementStates
{
    public abstract class PlayerMovementState
    {
        protected readonly MovementController Controller;

        protected PlayerMovementState(MovementController controller)
        {
            Controller = controller;
        }

        public virtual void Enter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void Exit()
        {
        }

        public virtual void OnDash()
        {
        }
    }
}