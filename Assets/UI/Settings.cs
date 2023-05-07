using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class Settings : MonoBehaviour
{
    private void Start()
    {
        SetDefaultSettings();
    }


    [SerializeField]
    private TMP_Dropdown dropdown;
    List<int> widths = new List<int>() { 586, 960, 1280, 1920, 3840 };
    List<int> heights = new List<int>() { 320, 540, 800, 1080, 2160 };

    private void SetDefaultSettings()
    {
        dropdown.value = 3;
    }

    public void Resolution(int resolutionIndex)
    {
        bool fullscreen = Screen.fullScreen;
        int width = widths[resolutionIndex];
        int height = heights[resolutionIndex];
        Screen.SetResolution(width, height, fullscreen);
    }


    private bool isFullScreen = true;

    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        Debug.Log(isFullScreen);
    }
}
