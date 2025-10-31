using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Input
{
    public interface IInputManager
    {
        Vector2 GetMovementVectorNormalized();
        public event Action DashPerformed;
    }

    public class InputManager : IInputManager
    {
        private readonly PlayerInputActions _playerInputActions = new();

        public InputManager()
        {
            _playerInputActions.Player.Dash.performed += DashOnPerformed;
            _playerInputActions.Player.Enable();
        }

        public Vector2 GetMovementVectorNormalized()
        {
            var inputVector = _playerInputActions.Player.Move.ReadValue<Vector2>();
            inputVector = inputVector.normalized;

            return inputVector;
        }

        public event Action DashPerformed;

        private void DashOnPerformed(InputAction.CallbackContext obj)
        {
            DashPerformed?.Invoke();
        }

        ~InputManager()
        {
            _playerInputActions.Player.Disable();
            _playerInputActions.Player.Dash.performed -= DashOnPerformed;
        }
    }
}