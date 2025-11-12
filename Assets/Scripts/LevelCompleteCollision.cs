using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;

public class LevelCompleteCollision : MonoBehaviour
{
    [Header("SoundFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSFX;

    [Header("UI References")]
    public GameObject levelCompleteUI; 
    
    [Header("Settings")]
    public string playerTag = "Player"; 
    public bool oneTimeOnly = true;   
    public string nextSceneName = "Level 2"; 
    public float delayBeforeNextScene = 2f;  
    
    private bool hasTriggered = false;
    
    void Start()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }

        if (audioSource == null)
        {
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

        // Show Level Complete UI
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(true);
        }

        // Play win sound
        if (audioSource != null && winSFX != null)
        {
            audioSource.PlayOneShot(winSFX);
        }

        Debug.Log("Level Complete!");

        Invoke(nameof(LoadNextScene), delayBeforeNextScene);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
    
    public void HideLevelCompleteUI()
    {
        if (levelCompleteUI != null)
        {
            levelCompleteUI.SetActive(false);
        }
    }
}
