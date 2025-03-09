using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI bestTimeText;
    void Start()
    {
        // Kiểm tra nếu đã lưu thời gian trước đó
        if (PlayerPrefs.HasKey("BestTime"))
        {
            float bestTime = PlayerPrefs.GetFloat("BestTime");
            bestTimeText.text = "Best Time: " + FormatTime(bestTime);
        }
        else
        {
            bestTimeText.text = "Best Time: --:--";
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f; // Đảm bảo thời gian chạy lại bình thường
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Hàm định dạng thời gian thành dạng mm:ss
    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
