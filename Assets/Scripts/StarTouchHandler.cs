using UnityEngine;

public class StarTouchHandler : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSFX;
    [SerializeField] private ScoreCounter scoreCounter;

    void Start()
    { 
        audioSource = GetComponent<AudioSource>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Star touched by Player. Destroying star.");
            ScoreCounter.Instance?.AddScore(1);
            audioSource.PlayOneShot(collectSFX);

            Destroy(gameObject, collectSFX != null ? collectSFX.length : 0f);
        }
    }
    
}
