using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ButtonSound
{
    public Button button;         // Nút cần gán
    public AudioClip audioClip;   // Âm thanh tương ứng
}

public class MultiButtonSoundController : MonoBehaviour
{
    public AudioSource audioSource;     // Audio Source dùng chung
    public ButtonSound[] buttonSounds;  // Mảng các nút và âm thanh

    void Start()
    {
        foreach (var item in buttonSounds)
        {
            if (item.button != null && item.audioClip != null)
            {
                item.button.onClick.AddListener(() => PlaySound(item.audioClip));
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Phát âm thanh mà không ngắt âm thanh đang chạy
        }
        else
        {
            Debug.LogWarning("AudioSource chưa được gán!");
        }
    }
}
