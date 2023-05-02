using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using SaveLoadSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _GameOverClip;

    [SerializeField]
    public Text scoreText;
    [SerializeField]
    private TMP_Text gameOverScore;

    private int playerScore = 0;

    public delegate void GamePaused();
    public static event GamePaused Paused;
    public delegate void GameContinued();
    public static event GameContinued Continued;

    private bool IsGameOver;
    private static bool gameIsPaused;

    //Find player, that we can deactivate controls and play gameover clip
    private void Start()
    {
        SaveGameManager.LoadGame();
        IsGameOver = false;
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

    private void OnEnable()
    {
        ShipControl.GameIsOver += PlayerDied;
        EnemyMover.EnemyIsDied += AddScore;
        RoamingEnemy.EnemyIsDied += AddScore;
    }

    private void OnDisable()
    {
        ShipControl.GameIsOver -= PlayerDied;
        EnemyMover.EnemyIsDied -= AddScore;
        RoamingEnemy.EnemyIsDied -= AddScore;
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
    
    private void AddScore(int value)
    {
        playerScore+= value;
        scoreText.text = "Score: " + playerScore.ToString(); // convert and print score on the screen
    }

    //That what will heppend when player lose
    private void PlayerDied()
    {
        IsItTheBestScore(playerScore);
        SaveGameManager.SaveGame();
        IsGameOver = true;
        gameOverScore.text = "Your final score is: " + playerScore.ToString() + "\n Your the best score is " + SaveData.score;
        scoreText.enabled = false;
        Cursor.visible = true;
        _audioSource.PlayOneShot(_GameOverClip);
    }

    //Check if current score is the highest score you ever have
    private void IsItTheBestScore(int currentScore)
    {
        if(currentScore > SaveData.score)
        {
            SaveData.score = currentScore;
        }
    }
}
