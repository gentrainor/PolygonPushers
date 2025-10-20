using UnityEngine;
using TMPro;   // required for TextMeshPro

public class RaceTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    private bool running = false;
    private float t = 0f;

    void Awake()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false); // hide timer at start
    }

    void Update()
    {
        if (!running) return;

        t += Time.deltaTime;

        int minutes = (int)(t / 60f);
        float seconds = t % 60f;

        if (timerText != null)
            timerText.text = $"{minutes:0}:{seconds:00.00}";
    }

    public void StartTimer()
    {
        if (running) return;
        t = 0f;
        running = true;
        if (timerText != null)
            timerText.gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        running = false;
    }

    public void ResetTimer()
    {
        running = false;
        t = 0f;
        if (timerText != null)
            timerText.text = "0:00.00";
    }
}

