using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    //Menu pannels which we need to monipulate
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
        ShipControl.GameIsOver += GameIsOver;
    }

    private void OnDisable()
    {
        GameManager.Paused -= Pause;
        GameManager.Continued -= Continue;
        ShipControl.GameIsOver -= GameIsOver;
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
        mainMenu.SetActive(true);
    }

    private void Continue()
    {
        DeactivateMenuPannels();
        mainMenu.SetActive(false);
    }

    private void GameIsOver()
    {
        gameOverMenu.SetActive(true);
    }
}
