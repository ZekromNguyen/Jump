using UnityEngine;
using TMPro; // TextMeshPro để hiển thị thời gian
using Dan.Main; // Import thư viện leaderboard
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI leaderboardText;

    void Start()
    {
        DisplayLeaderboard();
    }

    // Hiển thị dữ liệu leaderboard
    public void DisplayLeaderboard()
    {
        List<ScoreEntry> scores = LeaderboardCreator.Instance.GetLeaderboard();
        leaderboardText.text = "Leaderboard:\n";

        foreach (var score in scores)
        {
            leaderboardText.text += $"{score.PlayerName}: {score.Score:F2}s\n";
        }
    }
}
