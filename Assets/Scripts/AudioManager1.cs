using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider volumeSlider; // Assign in Inspector

    private void Start()
    {
        // Load saved volume or set default to 1
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        AudioListener.volume = volumeSlider.value;

        // Add listener to detect changes
        volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    private void ChangeVolume(float value)
    {
        AudioListener.volume = value; // Change game volume
        PlayerPrefs.SetFloat("Volume", value); // Save volume
    }
}
