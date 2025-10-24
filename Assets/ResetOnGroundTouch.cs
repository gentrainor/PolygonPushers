using System.Collections;
using UnityEngine;

public class RespawnOnPlaneLanding : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint;          
    public float spawnYOffset = 0.15f;      

    [Header("Ground Detection")]
    public string groundTag = "Ground";     
    public float minImpactSpeed = 2.0f;     
    public float landingCooldown = 0.35f;   

    [Header("Player Movement Script")]
    public MonoBehaviour movementScript;    

    private CharacterController cc;
    private float nextAllowedTime = 0f;
    private bool wasGroundedLastFrame = false;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void Start()
    {
        if (respawnPoint == null)
        {
            Debug.LogWarning("No respawn point assigned! Using current player start position.");
            GameObject temp = new GameObject("AutoSpawnPoint");
            temp.transform.position = transform.position;
            temp.transform.rotation = transform.rotation;
            respawnPoint = temp.transform;
        }
    }

    void Update()
    {
        wasGroundedLastFrame = cc.isGrounded;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Time.time >= nextAllowedTime &&
            hit.collider.CompareTag(groundTag) &&
            !wasGroundedLastFrame &&
            cc.velocity.y < -minImpactSpeed &&
            hit.normal.y > 0.5f)
        {
            StartCoroutine(RespawnRoutine());
        }
    }

    private IEnumerator RespawnRoutine()
    {
        nextAllowedTime = Time.time + landingCooldown;
        if (movementScript != null)
            movementScript.enabled = false;

        if (cc != null)
            cc.enabled = false;

        Vector3 targetPos = respawnPoint.position;
        targetPos.y += spawnYOffset; // Lift slightly above platform
        transform.SetPositionAndRotation(targetPos, respawnPoint.rotation);

        yield return null;

        if (cc != null)
            cc.enabled = true;

        yield return null;

        if (movementScript != null)
            movementScript.enabled = true;

        Debug.Log("Respawned player at: " + respawnPoint.position);
    }
}





