using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
<<<<<<< Updated upstream
=======
    private void Start()
    {
        GameManager.Restarted.AddListener(PlayPressed);
    }
>>>>>>> Stashed changes
    public void PlayPressed()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }
}
