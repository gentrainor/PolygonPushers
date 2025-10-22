using UnityEngine;


// notes:
// shift is to run
// right click to move camera around

namespace Character.CharacterControl
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Animator characterAnimator;

        public float dragForce = 0.1f;
        public float acceleration = 0.1f;
        public float walkSpeed = 4f;
        public float sprintMultiplier = 1.5f;

        [Header("Camera Settings")]
        public Vector3 cameraOffset = new Vector3(0f, 15f, -10f);
        public float cameraSmoothSpeed = 10f;
        public float cameraLookHeight = 1.5f;

        [Header("Look Settings")]
        public bool enableMouseLook = false;
        public bool requireRightClick = true;
        public float horizontalSensitivity = 2f;
        public float verticalSensitivity = 2f;
        public float verticalLookLimit = 89f;

        private PlayerInput playerInput;
        private float cameraYaw = 0f;
        private float cameraPitch = 30f;
        public float gravityForce = -9.81f;
        public float jumpForce = 15f;
        private float verticalVelocity = 0f;

        public float jumpGracePeriod = 0.10f;
        public float jumpBufferTime = 0.05f;
        private float groundedTimer = 0f;
        private float jumpBufferTimer = 0f;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            if (characterAnimator == null)
                characterAnimator = GetComponent<Animator>();

            if (playerCamera != null)
            {
                Vector3 direction = playerCamera.transform.position - transform.position;
                cameraYaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                cameraPitch = Mathf.Asin(-direction.y / direction.magnitude) * Mathf.Rad2Deg;
            }
        }

        private void Update()
        {
            HandleMovement();
            HandleAnimations();

            // Check if we should handle mouse look
            if (enableMouseLook)
            {
                // right click to move camera
                if (!requireRightClick || Input.GetMouseButton(1))
                {
                    HandleLookRotation();
                }
            }
            else
            {
                cameraYaw = transform.eulerAngles.y;
            }
        }

        private void LateUpdate()
        {
            HandleCameraFollow();
        }

        private void HandleMovement()
        {
            if (playerCamera == null || characterController == null || playerInput == null)
                return;

            bool isSprinting = Input.GetKey(KeyCode.LeftShift);
            float currentSpeed = isSprinting ? walkSpeed * sprintMultiplier : walkSpeed;
            float currentAcceleration = isSprinting ? acceleration * sprintMultiplier : acceleration;

            Vector3 cameraForward = new Vector3(playerCamera.transform.forward.x, 0f, playerCamera.transform.forward.z).normalized;
            Vector3 cameraRight = new Vector3(playerCamera.transform.right.x, 0f, playerCamera.transform.right.z).normalized;
            Vector3 moveDirection = cameraRight * playerInput.MovementInput.x + cameraForward * playerInput.MovementInput.y;

            Vector3 currentVelocity = characterController.velocity;
            currentVelocity.y = 0f;

            bool hasMovementInput = playerInput.MovementInput.magnitude > 0.01f;

            Vector3 horizontalVelocity;

            if (hasMovementInput)
            {
                Vector3 velocityChange = moveDirection * currentAcceleration * Time.deltaTime;
                Vector3 newVelocity = currentVelocity + velocityChange;

                Vector3 dragVector = newVelocity.normalized * dragForce * Time.deltaTime;
                if (newVelocity.magnitude > dragForce * Time.deltaTime)
                    newVelocity -= dragVector;

                newVelocity = Vector3.ClampMagnitude(newVelocity, currentSpeed);
                horizontalVelocity = newVelocity;
            }
            else
            {
                float stoppingSpeed = dragForce * 3f * Time.deltaTime;

                if (currentVelocity.magnitude > stoppingSpeed)
                {
                    horizontalVelocity = currentVelocity - currentVelocity.normalized * stoppingSpeed;
                }
                else
                {
                    horizontalVelocity = Vector3.zero;
                }
            }

            if (characterController.isGrounded)
                groundedTimer = jumpGracePeriod;
            else
                groundedTimer -= Time.deltaTime;

            if (playerInput.ConsumeJumpPressed())
                jumpBufferTimer = jumpBufferTime;
            else
                jumpBufferTimer -= Time.deltaTime;

            if (groundedTimer > 0f && jumpBufferTimer > 0f)
            {
                verticalVelocity = jumpForce;
                groundedTimer = 0f;
                jumpBufferTimer = 0f;
            }

            if (characterController.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }
            else
            {
                float gravityMultiplier = verticalVelocity < 0f ? 2f : 1f; 
                verticalVelocity += gravityForce * gravityMultiplier * Time.deltaTime;
            }

            Vector3 finalMotion = (horizontalVelocity + new Vector3(0f, verticalVelocity, 0f)) * Time.deltaTime;
            characterController.Move(finalMotion);

            if (moveDirection.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        private void HandleAnimations()
        {
            if (characterAnimator == null || playerInput == null)
                return;

            float movementMagnitude = playerInput.MovementInput.magnitude;
            bool isSprinting = Input.GetKey(KeyCode.LeftShift); // SHIFT TO RUN

            float targetAnimationSpeed = 0f;

            if (movementMagnitude > 0.1f)
            {
                targetAnimationSpeed = isSprinting ? 1f : 0.5f;
            }

            float currentAnimationSpeed = characterAnimator.GetFloat("Speed");
            float smoothedSpeed = Mathf.Lerp(currentAnimationSpeed, targetAnimationSpeed, Time.deltaTime * 10f);

            characterAnimator.SetFloat("Speed", smoothedSpeed);
            characterAnimator.SetBool("IsGrounded", characterController.isGrounded);
        }

        // cam settings
        private void HandleLookRotation()
        {
            if (playerInput == null)
                return;

            cameraYaw += horizontalSensitivity * playerInput.LookInput.x;
            cameraPitch -= verticalSensitivity * playerInput.LookInput.y;
            cameraPitch = Mathf.Clamp(cameraPitch, -verticalLookLimit, verticalLookLimit);
        }

        private void HandleCameraFollow()
        {
            if (playerCamera == null)
                return;

            Quaternion cameraRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
            Vector3 desiredPosition = transform.position + cameraRotation * cameraOffset;

            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, desiredPosition, Time.deltaTime * cameraSmoothSpeed);

            Vector3 lookAtPoint = transform.position + Vector3.up * cameraLookHeight;
            playerCamera.transform.LookAt(lookAtPoint);
        }
    }
}