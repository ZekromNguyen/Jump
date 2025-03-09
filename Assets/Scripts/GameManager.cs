using Dan.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameWinUi;
    private bool isGameWin = false;

    private float startTime;
    private float finishTime;

    void Start()
    {
        GameWinUi.SetActive(false); // Tắt UI Win Game lúc khởi động
        startTime = Time.time;      // Bắt đầu tính thời gian
    }

    // Khi người chơi thắng
    public void GameWin()
    {
        isGameWin = true;
        finishTime = Time.time - startTime; // Lấy thời gian hoàn thành

        // Gửi dữ liệu lên leaderboard
        LeaderboardCreator.Instance.SubmitScore("Player1", finishTime);



        // Hiển thị giao diện Win Game
        Time.timeScale = 0;
        GameWinUi.SetActive(true);
    }

    // Khi người chơi nhấn nút "Menu"
    public void GotoMenu()
    {
     
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1; 
    }

    // Kiểm tra trạng thái thắng game
    public bool IsGameWin()
    {
        return isGameWin;
    }
}
