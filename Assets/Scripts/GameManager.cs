using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject GameWinUi;
    private bool isGameWin = false;

    public static float completionTime; // Biến tạm để lưu thời gian hoàn thành

    private float startTime;

    void Start()
    {
        GameWinUi.SetActive(false); // Tắt UI Win Game lúc khởi động
        startTime = Time.time;      // Bắt đầu tính thời gian
    }

    // Khi người chơi thắng
    public void GameWin()
    {
        isGameWin = true;

        // Lưu thời gian hoàn thành
        completionTime = Time.time - startTime;

        // Hiển thị giao diện Win Game
        Time.timeScale = 0;
        GameWinUi.SetActive(true);
    }

    // Khi người chơi nhấn nút "Menu"
    public void GotoMenu()
    {
        PlayerPrefs.SetFloat("LastCompletionTime", completionTime); // Lưu thời gian tạm thời
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1; // Khôi phục thời gian bình thường
    }

    // Kiểm tra trạng thái thắng game
    public bool IsGameWin()
    {
        return isGameWin;
    }
}
