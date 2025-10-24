using UnityEngine;

public class StartZone : MonoBehaviour
{
    [SerializeField] private RaceTimer raceTimer;
    [SerializeField] private ScoreCounter scoreCounter;
    [SerializeField] private string playerTag = "Player";
    private bool started = false;

    private void OnTriggerExit(Collider other)
    {
        if (!started && other.CompareTag(playerTag))
        {
            started = true;
            if (raceTimer != null) raceTimer.StartTimer();
            if (scoreCounter != null) scoreCounter.StartCounter();
        }
    }
}
