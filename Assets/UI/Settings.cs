using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private List<ResItem> resolutions = new List<ResItem>();

    private int selectedResolution;

    private void Start()
    {
        foreach (var item in resolutions)
        {
            UpdateDropDown(item);
        }
        
        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedResolution = i;
                Resolution(selectedResolution);
                DropDownItemSelected(selectedResolution);
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);

            UpdateDropDown(newRes);

            selectedResolution = resolutions.Count - 1;
            Resolution(selectedResolution);
            DropDownItemSelected(selectedResolution);
        }
    }

    public void Resolution(int resolutionIndex)
    {
        bool fullscreen = Screen.fullScreen;
        int horizontal = resolutions[resolutionIndex].horizontal;
        int vertical = resolutions[resolutionIndex].vertical;
        Screen.SetResolution(horizontal, vertical, fullscreen);
    }

    private bool isFullScreen = true;
    [SerializeField]
    private Toggle fullScreen;

    public void FullScreenToggle()
    {
        isFullScreen = !isFullScreen;
        Screen.fullScreen = isFullScreen;
        Debug.Log("Fullscreen" + isFullScreen);
    }

    [SerializeField]
    private TMP_Dropdown dropdown;
    [SerializeField]
    private TMP_Text textBox;

    private void UpdateDropDown(ResItem newRes)
    {
        string newText = newRes.horizontal + " x " + newRes.vertical;
        dropdown.options.Add(new TMP_Dropdown.OptionData() { text = newText });
    }

    private void DropDownItemSelected(int selectedIndex)
    {
        dropdown.value = selectedIndex;
        int SetValue = dropdown.value;
        textBox.text = dropdown.options[SetValue].text;
    }
}
[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}
