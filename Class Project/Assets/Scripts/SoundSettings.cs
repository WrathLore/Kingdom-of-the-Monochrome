using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundSettings : MonoBehaviour
{
    int i = 1;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider backgroundVolumeSlider;
    [SerializeField] GameObject optionsPanel;

    string masterVolume = "MasterVolume";
    string sfxVolume = "SFXVolume";
    string musicVolume = "MusicVolume";
    string backgroundVolume = "BackgroundVolume";

    void Awake()
    {
        i = PlayerPrefs.GetInt("FirstOpen",1);
        if(i == 1 && optionsPanel != null)
        {
            optionsPanel.GetComponent<Canvas>().enabled = true;
        }
    }
    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat(masterVolume,0f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(sfxVolume, 0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolume, 0f);
        backgroundVolumeSlider.value = PlayerPrefs.GetFloat(backgroundVolume,0f);
        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
    }


    public void SetMasterVolume()
    {
        SetVolume(masterVolume, masterVolumeSlider.value);
        PlayerPrefs.SetFloat(masterVolume, masterVolumeSlider.value);
    }
    public void SetSFXVolume()
    {
        SetVolume(sfxVolume, sfxVolumeSlider.value);
        PlayerPrefs.SetFloat(sfxVolume, sfxVolumeSlider.value);
    }
    public void SetMusicVolume()
    {
        SetVolume(musicVolume, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(musicVolume, musicVolumeSlider.value);
    }
    public void SetBackgroundVolume()
    {
        SetVolume(backgroundVolume, backgroundVolumeSlider.value);
        PlayerPrefs.SetFloat(backgroundVolume, backgroundVolumeSlider.value);
    }

    void SetVolume(string groupName, float value)
    {
        float adjustedVolume = Mathf.Log10(value) * 20;
        if(value == 0)
        {
            adjustedVolume = -80;
        }
        audioMixer.SetFloat(groupName, adjustedVolume);
    }
}
