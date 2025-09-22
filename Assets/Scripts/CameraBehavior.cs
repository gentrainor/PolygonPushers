using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform target; //target is the main player
    public Vector3 offset = new Vector3(0f, 100f, -10f); // camera height
    public float followSmoothing = 8f; 
    public Vector3 fixedEuler = new Vector3(50f, 0f, 0f); // keep this rotation so it doesn't rotate with capsule

    void LateUpdate()
    {
        if (!target) return;

        // Follow position only 
        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, 1f - Mathf.Exp(-followSmoothing * Time.deltaTime));

        // Keep a fixed rotation (no player rotation influence)
        transform.rotation = Quaternion.Euler(fixedEuler);
    }
}

