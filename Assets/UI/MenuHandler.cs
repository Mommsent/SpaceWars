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
    private GameObject[] pauseMenuse;

    private void Awake()
    {
        GameManager.GamePaused.AddListener(Pause);
        GameManager.Continued.AddListener(Continue);
        ShipControl.GameOver.AddListener(GameIsOver);
    }

    //Autocloser menu panels
    private void DeactivateMenuPannels()
    {
        foreach (GameObject menu in pauseMenuse)
        {
            menu.SetActive(false);
        }
    }
    //Game conditions
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
