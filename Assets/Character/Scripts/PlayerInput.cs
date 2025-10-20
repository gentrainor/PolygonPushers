using UnityEngine;
using UnityEngine.InputSystem;

namespace Character.CharacterControl
{
    [DefaultExecutionOrder(-2)]
    public class PlayerInput : MonoBehaviour, PlayerControls.IPlayerActions
    {
        public PlayerControls PlayerControls { get; private set; }
        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }

        private bool _jumpPressed;
        public bool ConsumeJumpPressed()
        {
            bool j = _jumpPressed;
            _jumpPressed = false;   
            return j;
        }

        private void Awake()
        {
            PlayerControls = new PlayerControls();
        }

        private void OnEnable()
        {
            PlayerControls.Player.SetCallbacks(this);
            PlayerControls.Player.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Player.Disable();
            PlayerControls.Player.RemoveCallbacks(this);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>(); 
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed) _jumpPressed = true; 
        }

        public void OnAttack(InputAction.CallbackContext c) {}
        public void OnCrouch(InputAction.CallbackContext c) {}
        public void OnInteract(InputAction.CallbackContext c) {}
        public void OnNext(InputAction.CallbackContext c) {}
        public void OnPrevious(InputAction.CallbackContext c) {}
        public void OnSprint(InputAction.CallbackContext c) {}
    }
}

