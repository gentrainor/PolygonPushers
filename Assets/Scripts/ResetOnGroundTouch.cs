using System.Collections;
using UnityEngine;

public class RespawnOnPlaneLanding : MonoBehaviour
{
    public Transform respawnPoint;          
    public float spawnYOffset = 0.15f;      
    public string groundTag = "Ground";     
    public float minImpactSpeed = 2.0f;     
    public float landingCooldown = 0.35f;   
    public MonoBehaviour movementScript;    
    public int respawnPenalty = 1;   // how much to lose per respawn

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


    public IEnumerator RespawnRoutine()
    {
        nextAllowedTime = Time.time + landingCooldown;

        if (movementScript != null)
            movementScript.enabled = false;

        if (cc != null)
            cc.enabled = false;

        Vector3 targetPos = respawnPoint.position;
        targetPos.y += spawnYOffset;
        transform.SetPositionAndRotation(targetPos, respawnPoint.rotation);

        yield return null;

        if (cc != null)
            cc.enabled = true;

        yield return null;

        if (movementScript != null)
            movementScript.enabled = true;

        if (ScoreCounter.Instance != null)
        {
            ScoreCounter.Instance.AddScore(-respawnPenalty);
            Debug.Log($"Respawned player. Score penalized by {respawnPenalty}. New score: {ScoreCounter.Instance.GetScore()}");
        }
        else
        {
            Debug.LogWarning("ScoreCounter.Instance is null - no score penalty applied.");
        }
    }
}






