using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private bool isPaused = false; 
    public GameObject optionPopup; 


    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape Pressed. Current isPaused: " + isPaused);

            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    public void PauseGame()
    {
        Debug.Log("Pausing Game...");
        pauseMenu.SetActive(true);
        optionPopup.SetActive(false);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Debug.Log("Resuming Game...");
        pauseMenu.SetActive(false);
        optionPopup.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void ShowOptionPopup()
    {
        optionPopup.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void HideOptionPopup()
    {
        optionPopup.SetActive(false); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
