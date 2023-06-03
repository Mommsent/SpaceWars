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
        GameManager.GamePaused.AddListener(PauseOrGameOverMenu);
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
    //chooses what menu open depends on game condition
    public void PauseOrGameOverMenu()
    {
        if(isGameOver == false) mainMenu.SetActive(true);
        else gameOverMenu.SetActive(true);
    }

    public void Continue()
    {
        DeactivateMenuPannels();
        mainMenu.SetActive(false);
    }

    private bool isGameOver = false;
    private void GameIsOver()
    {
        isGameOver = true;
        PauseOrGameOverMenu();
    }
}
