using UnityEngine;
using UnityEngine.UI; // Thư viện UI để tương tác với Button

public class ButtonSoundController : MonoBehaviour
{
    public AudioSource audioSource; // Kéo Audio Source vào đây
    public Button myButton;        // Kéo Button của bạn vào đây

    void Start()
    {
        // Đăng ký sự kiện click của Button
        if (myButton != null)
        {
            myButton.onClick.AddListener(PlaySound);
        }
    }

    void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource hoặc AudioClip chưa được gán!");
        }
    }
}