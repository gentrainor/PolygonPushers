using UnityEngine;

namespace Character.CharacterControl
{
    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] private CharacterController charController;
        [SerializeField] private Camera cam;
        public float drag = 0.1f;
        public float accel = 0.1f;
        public float speed = 4f;

        [Header("Camera Settings")]
        public Vector3 camOffset = new Vector3(0f, 15f, -10f);
        public float camSmoothSpd = 10f;
        public float camLookHeight = 1.5f;

        [Header("Look Settings")]
        public bool enableMouseLook = false;
        public float lookSensH = 2f;
        public float lookSensV = 2f;
        public float lookLimitV = 89f;

        private PlayerInput playerInpt;
        private float camYaw = 0f;
        private float camPitch = 30f; // angle loking down

        private void Awake()
        {
            playerInpt = GetComponent<PlayerInput>();

            // initalize camera angles based on current pos
            if (cam != null)
            {
                Vector3 dir = cam.transform.position - transform.position;
                camYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                camPitch = Mathf.Asin(-dir.y / dir.magnitude) * Mathf.Rad2Deg;
            }
        }

        private void Update()
        {
            HandleMovement();
            
            // only handle mouse look if enabeld
            if (enableMouseLook)
            {
                HandleLookRotation();
            }
            else
            {
                // when mouse look is disabled, camera folows player rotation
                camYaw = transform.eulerAngles.y;
            }
        }

        private void LateUpdate()
        {
            HandleCameraFollow();
        }

        private void HandleMovement()
        {
            if (cam == null || charController == null || playerInpt == null)
                return;

            Vector3 camFwd = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z).normalized;
            Vector3 camRght = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z).normalized;
            Vector3 moveDir = camRght * playerInpt.MovementInput.x + camFwd * playerInpt.MovementInput.y;

            Vector3 moveChng = moveDir * accel * Time.deltaTime;
            Vector3 newVel = charController.velocity + moveChng;

            Vector3 dragFrc = newVel.normalized * drag * Time.deltaTime;
            if (newVel.magnitude > drag * Time.deltaTime)
                newVel -= dragFrc;
            else
                newVel = Vector3.zero;

            newVel = Vector3.ClampMagnitude(newVel, speed);
            charController.Move(newVel * Time.deltaTime);

            // rotate player to face movment direction
            if (moveDir.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }
        }

        private void HandleLookRotation()
        {
            if (playerInpt == null)
                return;

            // update camera rotation basd on mouse input
            camYaw += lookSensH * playerInpt.LookInput.x;
            camPitch -= lookSensV * playerInpt.LookInput.y;

            // clamp verticle rotation
            camPitch = Mathf.Clamp(camPitch, -lookLimitV, lookLimitV);
        }
        
        private void HandleCameraFollow()
        {
            if (cam == null)
                return;
            
            Quaternion rot = Quaternion.Euler(camPitch, camYaw, 0f);
            Vector3 desiredPos = transform.position + rot * camOffset;
            
            cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPos, Time.deltaTime * camSmoothSpd);
            
            // use adjustable hieght
            Vector3 lookPt = transform.position + Vector3.up * camLookHeight;
            cam.transform.LookAt(lookPt);
        }
    }
}