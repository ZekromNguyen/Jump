using UnityEngine;
using UnityEngine.SceneManagement;

public class Trophy : MonoBehaviour
{
    private GameManager gameManager;

    public void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Đảm bảo nhân vật chính (Player) mới kích hoạt chiến thắng
        if (collision.CompareTag("Trophy"))
        {
            gameManager.GameWin(); // Gọi hàm GameWin trong GameManager

            // Chuyển sang Menu và khôi phục thời gian sau khi chuyển Scene
            SceneManager.LoadScene("Menu");
            Time.timeScale = 1f;
        }
    }
}
