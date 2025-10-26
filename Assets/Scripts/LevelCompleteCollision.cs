using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteCollision : MonoBehaviour
{
     [Header("SoundFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSFX;


    [Header("UI References")]
    public GameObject levelCompleteUI; 
    
    [Header("Settings")]
    public string playerTag = "Player"; // Tag to identify the player
    public bool oneTimeOnly = true;   
    
    private bool hasTriggered = false;
    
    void Start()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
            audioSource = GetComponent<AudioSource>();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && (!oneTimeOnly || !hasTriggered))
        {
            TriggerLevelComplete();
        }
    }
    
    void TriggerLevelComplete()
    {
        hasTriggered = true;
        
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }

        //TODO play some sound eventually
        audioSource.PlayOneShot(winSFX);
        
        Debug.Log("Level Complete!");
    }
    
    public void HideLevelCompleteUI()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
    }
}