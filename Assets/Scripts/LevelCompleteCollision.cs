using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteCollision : MonoBehaviour
{
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