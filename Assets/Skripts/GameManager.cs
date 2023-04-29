using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _GameOverClip;

    [SerializeField]
    public Text scoreText;
    [SerializeField]
    private TMP_Text gameOverScore;

    private ShipControl movement;

    private int playerScore = 0;

    public delegate void GamePaused();
    public static event GamePaused Paused;
    public delegate void GameContinued();
    public static event GameContinued Continued;
    public delegate void GameOver();
    public static event GameOver GameIsOver;

    private bool IsGameOver;
    private static bool gameIsPaused;

    //Find player, that we can deactivate controls and play gameover clip
    private void Start()
    {
        IsGameOver = false;
        movement = GameObject.FindObjectOfType<ShipControl>();
        _audioSource = GetComponent<AudioSource>();
    }

    //Check if Esc button pressed and do staff
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver)
        {
            PausePressed();
        }
    }

    private void PausePressed()
    {
        gameIsPaused = !gameIsPaused;
        PauseGame();
    }

    private void PauseGame()
    {
        if (gameIsPaused)
        {
            if (Paused != null)
                Paused();

            Time.timeScale = 0f;
            Cursor.visible = true;
        }
        else
        {
            if (Continued != null)
                Continued();

            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }
    

    public void AddScore()
    {
        playerScore++;
        scoreText.text = "Score: " + playerScore.ToString(); // convert and print score on the screen
    }

    //That what will heppend when player lose
    public void PlayerDied()
    {
        IsGameOver = true;
        gameOverScore.text = "Your final score is: " + playerScore.ToString();
        scoreText.enabled = false;
        movement.enabled = false;

        if (GameIsOver != null)
            GameIsOver();

        Cursor.visible = true;
        _audioSource.PlayOneShot(_GameOverClip);
    }    
}
