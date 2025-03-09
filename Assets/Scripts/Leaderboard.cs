using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Leaderboard : MonoBehaviour
{
    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public float completionTime;
    }

    [System.Serializable]
    public class LeaderboardData
    {
        public List<PlayerData> players = new List<PlayerData>();
    }

    public TMP_InputField playerNameInput; // Kéo ô nhập tên vào đây
    public Transform leaderboardContent;
    public GameObject entryPrefab;

    private const string leaderboardFile = "/leaderboard.json";
    private LeaderboardData leaderboard = new LeaderboardData();

    // Thêm người chơi mới vào BXH
    public void AddPlayer()
    {
        string playerName = playerNameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player_" + Random.Range(1000, 9999);
        }

        leaderboard.players.Add(new PlayerData
        {
            playerName = playerName,
            completionTime = GameManager.completionTime
        });

        // Sắp xếp theo thời gian nhanh nhất
        leaderboard.players = leaderboard.players.OrderBy(p => p.completionTime).ToList();

        SaveLeaderboard();
        DisplayLeaderboard();
    }

    // Hiển thị BXH trên UI
    public void DisplayLeaderboard()
    {
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var player in leaderboard.players)
        {
            GameObject entry = Instantiate(entryPrefab, leaderboardContent);
            entry.GetComponent<TextMeshProUGUI>().text = $"{player.playerName} - {player.completionTime:F2} s";
        }
    }

    // Lưu dữ liệu BXH vào JSON
    public void SaveLeaderboard()
    {
        string json = JsonUtility.ToJson(leaderboard);
        File.WriteAllText(Application.persistentDataPath + leaderboardFile, json);
    }

    // Tải dữ liệu BXH khi mở game
    public void LoadLeaderboard()
    {
        string path = Application.persistentDataPath + leaderboardFile;

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            leaderboard = JsonUtility.FromJson<LeaderboardData>(json);
        }

        leaderboard.players = leaderboard.players.OrderBy(p => p.completionTime).ToList();
        DisplayLeaderboard();
    }

    private void Start()
    {
        LoadLeaderboard();
    }
}
