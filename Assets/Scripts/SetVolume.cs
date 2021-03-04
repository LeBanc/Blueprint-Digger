using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public float musicDeltaInit = 10f;
    public float soundDeltaInit = 0f;
    public Slider musicSlider;
    public Slider soundSlider;
    public TextMeshProUGUI musicVolumeText;
    public TextMeshProUGUI soundVolumeText;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);
    }

    public void SetMusicLevel(float sliderValue)
    {
        mixer.SetFloat("musicVolume", Mathf.Log10(sliderValue) * 20 + musicDeltaInit);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
        musicVolumeText.text = ((int)(sliderValue * 100)).ToString();
    }

    public void SetSoundLevel(float sliderValue)
    {
        mixer.SetFloat("soundVolume", Mathf.Log10(sliderValue) * 20 + soundDeltaInit);
        PlayerPrefs.SetFloat("SoundVolume", sliderValue);
        soundVolumeText.text = ((int)(sliderValue * 100)).ToString();
    }
}