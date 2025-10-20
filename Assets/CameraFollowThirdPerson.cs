using UnityEngine;

public class CameraFollowThirdPerson : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    [Tooltip("Where the camera aims on the target (player head).")]
    public float lookAtHeight = 1.6f;

    [Header("Framing (relative to target forward)")]
    public float distance = 6f;   // how far behind
    public float height   = 3f;   // how high above
    public float shoulder = 0.4f; // + right, - left
    public float fixedPitch = 15f; // degrees downward tilt

    [Header("Smoothing")]
    public float posSmooth = 12f;  // higher = snappier
    public float rotSmooth = 12f;

    [Header("Collision (prevents camera from going through walls/player)")]
    public float castRadius = 0.25f;   // small sphere cast
    public float minDistance = 1.5f;   // never get closer than this
    public LayerMask collideWith = ~0; // everything by default
    public float collisionBuffer = 0.2f;

    private Vector3 _vel; // for SmoothDamp if you prefer it later

    void LateUpdate()
    {
        if (!target) return;

        // Build desired camera anchor behind the player (no snap—always from target basis)
        Vector3 anchor = target.position + Vector3.up * height;
        Vector3 desiredPos =
              anchor
            - target.forward * distance
            + target.right   * shoulder;

        // --- Collision handling: push camera forward if obstructed ---
        Vector3 lookPoint = target.position + Vector3.up * lookAtHeight;
        Vector3 toCam = desiredPos - lookPoint;
        float   desiredLen = Mathf.Max(minDistance, toCam.magnitude);

        if (Physics.SphereCast(lookPoint, castRadius, toCam.normalized, out var hit, desiredLen, collideWith, QueryTriggerInteraction.Ignore))
        {
            float newLen = Mathf.Max(minDistance, hit.distance - collisionBuffer);
            desiredPos = lookPoint + toCam.normalized * newLen;
        }

        // --- Smooth position & rotation together (no angle snap) ---
        transform.position = Vector3.Lerp(transform.position, desiredPos, 1f - Mathf.Exp(-posSmooth * Time.deltaTime));

        // Build a target rotation that keeps a gentle downward tilt
        Vector3 fwd = (lookPoint - transform.position).normalized;
        // Enforce a soft pitch by blending toward a pitched forward
        Quaternion targetRot = Quaternion.LookRotation(fwd, Vector3.up);
        // Optional: bias pitch toward fixedPitch without snapping:
        Vector3 e = targetRot.eulerAngles;
        float pitchedX = Mathf.LerpAngle(e.x, fixedPitch, 0.35f);
        targetRot = Quaternion.Euler(pitchedX, e.y, 0f);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 1f - Mathf.Exp(-rotSmooth * Time.deltaTime));
    }
}
