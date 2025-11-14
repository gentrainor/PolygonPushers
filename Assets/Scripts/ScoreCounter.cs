using UnityEngine;
using TMPro; 

public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    private static int score = 0;
    private bool running = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this; 
        if (scoreText != null) 
            scoreText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!running) return;

        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    public void StartCounter()
    {
        if (running) return;
        running = true;
        Debug.Log("Score Counter Started");
        if (scoreText != null)
            scoreText.gameObject.SetActive(true);
    }

    public void StopCounter()
    {
        running = false;
    }

    public void ResetCounter()
    {
        running = false;
        if (scoreText != null)
            score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }
}
