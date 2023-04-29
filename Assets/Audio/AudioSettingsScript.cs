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

    private void Start()
    {
        //Set max value by standart at first start
        slider.value = float.MaxValue;
        mixer.SetFloat(volumeName, Mathf.Log(float.MaxValue) * 20f);

        LoadValues();
        UpdateValueOnChange(slider.value);

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
        LoadValues();
    }
    public void LoadValues()
    {
        float volumeValue = PlayerPrefs.GetFloat(volumeName);
        slider.value = volumeValue;
    }
}
