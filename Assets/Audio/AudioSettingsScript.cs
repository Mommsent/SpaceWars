using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using SaveLoadSystem;

public class AudioSettingsScript : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private AudioMixer mixer;

    private AudioSource audioSource;

    [SerializeField]
    private string volumeName;

    [SerializeField]
    private Text volumeLabel;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        UpdateValueOnChange(1f);
        LoadValues();

        slider.onValueChanged.AddListener(delegate
        {
            UpdateValueOnChange(slider.value);
        });
    }

    public void UpdateValueOnChange(float value)
    {
        if(mixer != null)
        {
            mixer.SetFloat(volumeName, Mathf.Log(value) * 20f);
        }
             
        if (volumeLabel != null)
            volumeLabel.text = Mathf.Round(value * 100.0f).ToString() + "%";
    }   
    
    public void SaveVolumeButton()
    {
        float volumeValue = slider.value;
        PlayerPrefs.SetFloat(volumeName, volumeValue);
    }

    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName);
        UpdateValueOnChange(volumeValue);
        audioSource.volume = volumeValue;
        slider.value = volumeValue;
    }
}
