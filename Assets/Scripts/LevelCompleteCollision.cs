using TMPro;
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


        RaceTimer timer = FindFirstObjectByType<RaceTimer>();
        if (timer != null)
        {
            timer.StopTimer();
            float finalTime = timer.GetTime();
            int baseScore = ScoreCounter.Instance != null ? ScoreCounter.Instance.GetScore() : 0;
            print(baseScore);
            int timePenalty = Mathf.FloorToInt(finalTime);
            double finalScore = (baseScore * (1.0 / timePenalty)) * 100;
            print(finalScore); //TODO display this later


            if (levelCompleteUI != null)
            {
                Transform child = levelCompleteUI.transform.Find("LevelCompleteText");
                if (child != null)
                {
                    TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
                    if (textComponent != null)
                    {
                        textComponent.text = $"Level Complete!\nFinal Score: {finalScore:F0}";
                    }
                }
            }
        }

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
        if (ScoreCounter.Instance != null)
        {
            ScoreCounter.Instance.ResetCounter();
        }
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
