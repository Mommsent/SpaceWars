using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSettingsScript : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private AudioMixer mixer;

    [SerializeField]
    private string volumeName;

    [SerializeField]
    private Text volumeLabel;

    private float currentMixerVolume = 1f;

    private void Start()
    {
        LoadValues();
    }

    public void UpdateValueOnChange(float value)
    {
        if(mixer != null)
            mixer.SetFloat(volumeName, Mathf.Log(value) * 20f);

        if (volumeLabel != null)
            volumeLabel.text = Mathf.Round(value * 100.0f).ToString() + "%";

        if (slider != null)
            slider.value = value;
    }   
    
    public void SaveVolumeButton()
    {
        float volumeValue = slider.value;
        PlayerPrefs.SetFloat(volumeName, volumeValue);
    }

    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName);
        if (volumeValue == 0)
            UpdateValueOnChange(currentMixerVolume);
        UpdateValueOnChange(volumeValue);
    }
}
