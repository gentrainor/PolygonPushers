using UnityEngine;
using UnityEngine.InputSystem;



namespace Character.CharacterControl
{
    public class PlayerInput : MonoBehaviour, PlayerControls.IPlayerActions
    {
        public PlayerControls PlayerControls { get; private set; }

        public Vector2 MovementInput { get; private set; }
        public Vector2 LookInput { get; private set; }



        private void OnEnable()
        {
            PlayerControls = new PlayerControls();
            PlayerControls.Enable();

            PlayerControls.Player.Enable();
            PlayerControls.Player.SetCallbacks(this);
        }

        private void OnDisable() {
            PlayerControls.Player.Disable();
            PlayerControls.Player.RemoveCallbacks(this);
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
            LookInput = context.ReadValue<Vector2>();

        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementInput = context.ReadValue<Vector2>();
            print(MovementInput);
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            // throw new System.NotImplementedException();
        }
    }



}
