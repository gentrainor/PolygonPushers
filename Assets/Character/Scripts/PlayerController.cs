using UnityEngine;

namespace Character.CharacterControl
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera camera;
        public float drag = 0.1f;
        public float accel = 0.1f;
        public float speed = 4f;

        [Header("Camera Settings")]
        public Transform cameraDesiredPose; // The target transform for the camera

        [Header("Look Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;

        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            HandleMovement();
        }

        private void LateUpdate()
        {
            HandleCameraFollow();
        }

        private void HandleMovement()
        {
            if (camera == null || characterController == null || playerInput == null)
                return;

            Vector3 cameraForward = new Vector3(camera.transform.forward.x, 0f, camera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(camera.transform.right.x, 0f, camera.transform.right.z).normalized;
            Vector3 moveDir = cameraRight * playerInput.MovementInput.x + cameraForward * playerInput.MovementInput.y;

            Vector3 moveChange = moveDir * accel * Time.deltaTime;
            Vector3 newVelocity = characterController.velocity + moveChange;
            Vector3 dragForce = newVelocity.normalized * drag * Time.deltaTime;

            if (newVelocity.magnitude > drag * Time.deltaTime)
                newVelocity -= dragForce;
            else
                newVelocity = Vector3.zero;

            newVelocity = Vector3.ClampMagnitude(newVelocity, speed);
            characterController.Move(newVelocity * Time.deltaTime);
        }

        private void HandleCameraFollow()
        {
            if (cameraDesiredPose == null || camera == null)
                return;

            camera.transform.position = cameraDesiredPose.position;
            camera.transform.rotation = cameraDesiredPose.rotation;
        }
    }
}
