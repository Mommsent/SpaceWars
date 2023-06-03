using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = false;
        GameManager.Restarted.AddListener(PlayPressed);
    }
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
