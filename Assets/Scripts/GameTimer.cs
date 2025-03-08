using UnityEngine;
using TMPro; // Nếu dùng TextMeshPro

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // Kéo TMP Text từ Inspector vào đây
    private float timer = 0f;
    private bool isRunning = true;

    void Start()
    {
        timer = 0f;
        isRunning = true;
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        int milliseconds = Mathf.FloorToInt((timer * 1000) % 1000);
        timerText.text = $"{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
