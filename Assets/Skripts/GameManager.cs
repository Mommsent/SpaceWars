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
    private Text scoreText;
    [SerializeField]
    private TMP_Text gameOverScore;

    private int playerScore = 0;
    private int pointsToDecrease = 2;

    public static UnityEvent GamePaused = new UnityEvent();
    public static UnityEvent Continued = new UnityEvent();

    private bool IsGameOver;
    private bool gameIsPaused;


    private void Awake()
    {
        ShipControl.GameOver.AddListener(PlayerDied);
        Enemy.EnemyIsDied.AddListener(AddScore);
        DestroyOnTrigger.EnemyPassed.AddListener(DecreaseScore);
    }

    //Find player, that we can deactivate controls and play gameover clip
    private void Start()
    {
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

    private void PausePressed()
    {
        gameIsPaused = !gameIsPaused;
        PauseGame();
    }

    private void PauseGame()
    {
        if (gameIsPaused)
        {
            GamePaused.Invoke();

            Time.timeScale = 0f;
            Cursor.visible = true;
        }
        else
        {
            Continued.Invoke();

            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }
    
    private void AddScore(int value)
    {
        playerScore+= value;
        scoreText.text = "Score: " + playerScore.ToString(); // convert and print score on the screen
    }
    private void DecreaseScore()
    {
        playerScore -= pointsToDecrease;
        scoreText.text = "Score: " + playerScore.ToString();
    }


    //That what will heppend when player lose
    private void PlayerDied()
    {
        IsItTheBestScore(playerScore);
        IsGameOver = true;
        gameOverScore.text = "Your final score is: " + playerScore.ToString() + "\n Your the best score is " + SaveData.score;
        scoreText.enabled = false;
        Cursor.visible = true;
        _audioSource.PlayOneShot(_GameOverClip);
    }

    //Check if current score is the highest score you ever have
    private void IsItTheBestScore(int currentScore)
    {
        SaveGameManager.LoadGame();
        if (currentScore > SaveData.score)
        {
            SaveData.score = currentScore;
            SaveGameManager.SaveGame();
        }
    }
}

[System.Serializable]
public class EnemyIsDied : UnityEvent<int> { }