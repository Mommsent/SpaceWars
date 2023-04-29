using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    //Menu pannels which we need to monipulate
    //[SerializeField]
    //private GameObject pauseMenu;
    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private static GameObject settings;
    [SerializeField]
    private static GameObject audioSettings;
    [SerializeField]
    private static GameObject graphicSettings;
    [SerializeField]
    private List<GameObject> pauseMenuse = new List<GameObject>() { settings, audioSettings, graphicSettings };

    private void OnEnable()
    {
        GameManager.Paused += Pause;
        GameManager.Continued += Continue;
        GameManager.GameIsOver += GameIsOver;
    }

    private void OnDisable()
    {
        GameManager.Paused -= Pause;
        GameManager.Continued -= Continue;
        GameManager.GameIsOver -= GameIsOver;
    }
    //Autocloser menu panels
    private void DeactivateMenuPannels()
    {
        foreach (GameObject menu in pauseMenuse)
        {
            menu.SetActive(false);
        }
    }

    private void Pause()
    {
        //pauseMenu.SetActive(true);
        mainMenu.SetActive(true);
    }

    private void Continue()
    {
        DeactivateMenuPannels();
        //pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
    }

    private void GameIsOver()
    {
        gameOverMenu.SetActive(true);
    }

    
    
    
}
