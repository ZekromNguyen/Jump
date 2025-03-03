using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider volumeSlider;

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume; // Adjust master volume

        // Save the volume setting
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    private void Start()
    {
        // Load saved volume (default to 1 if not set)
        float savedVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.volume = savedVolume;

        // Update slider UI if assigned
        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }
    }
}
