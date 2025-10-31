using UnityEngine;

namespace Scripts.Input
{
    public interface IInputManager
    {
        Vector2 GetMovementVectorNormalized();
    }

    public class InputManager : IInputManager
    {
        private readonly PlayerInputActions _playerInputActions = new();

        public InputManager()
        {
            _playerInputActions.Player.Enable();
        }


        public Vector2 GetMovementVectorNormalized()
        {
            var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
            inputVector = inputVector.normalized;

            return inputVector;
        }

        ~InputManager()
        {
            _playerInputActions.Player.Disable();
        }
    }
}