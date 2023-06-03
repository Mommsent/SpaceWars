using UnityEngine;
using UnityEngine.EventSystems;

public class GamepadController : MonoBehaviour
{
    public GameObject menuFirstButton, settingsFirstButton,
        audioOptionsFirstButton, graphicsOptionsFirstButton, gameOverFirstButton;
    private bool isGameOver = false;
    private void Awake()
    {
        GameManager.GamePaused.AddListener(SetInMenu);
        ShipControl.GameOver.AddListener(SetInGameOverMenu);
        GameManager.Continued.AddListener(SetInMenu);
    }
    public void SetInMenu()
    {
        if(isGameOver == false)
            SetFirstSelectedButton(menuFirstButton);
        else
            SetFirstSelectedButton(gameOverFirstButton);
    }
    public void SetInSettings()
    {
        SetFirstSelectedButton(settingsFirstButton);
    }
    public void SetInAudioSettings()
    {
        SetFirstSelectedButton(audioOptionsFirstButton);
    }
    public void SetInGraphicsSettings()
    {
        SetFirstSelectedButton(graphicsOptionsFirstButton);
    }
    public void SetInGameOverMenu()
    {
        isGameOver = true;
        SetFirstSelectedButton(gameOverFirstButton);
    }

    private void SetFirstSelectedButton(GameObject menu)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(menu);
    }
}
