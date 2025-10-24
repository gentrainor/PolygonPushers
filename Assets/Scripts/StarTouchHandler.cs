using UnityEngine;

public class StarTouchHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ScoreCounter scoreCounter;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Star touched by Player. Destroying star.");
            scoreCounter.AddScore(1);
            Destroy(gameObject); 
        }
    }
    
}
