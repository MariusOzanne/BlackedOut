/*

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public List<AudioSource> audioSources;  // Liste de toutes les sources audio pour chaque mode
    public Slider volumeSlider;
    public Button muteButton;
    private bool isMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Assure la persistance entre les scènes
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();
    }

    private void Start()
    {
        volumeSlider.onValueChanged.AddListener(SetVolume);
        muteButton.onClick.AddListener(ToggleMute);
    }

    public void SetVolume(float volume)
    {
        if (!isMuted)
        {
            foreach (AudioSource source in audioSources)
            {
                source.volume = volume;
            }
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        foreach (AudioSource source in audioSources)
        {
            source.mute = isMuted;
        }
        PlayerPrefs.SetInt("MuteState", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        isMuted = PlayerPrefs.GetInt("MuteState", 0) == 1;

        foreach (AudioSource source in audioSources)
        {
            source.volume = volume;
            source.mute = isMuted;
        }

        volumeSlider.value = volume;
    }
}

*/