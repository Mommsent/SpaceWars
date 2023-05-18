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
    private Text scoreText;
    [SerializeField]
    private TMP_Text gameOverScore;

    private int playerScore = 0;
    private int pointsToDecrease = 1;

    public static UnityEvent GamePaused = new UnityEvent();
    public static UnityEvent Continued = new UnityEvent();
    public static UnityEvent Restarted = new UnityEvent();

    private bool IsGameOver;
    private bool gameIsPaused;


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    //Find player, that we can deactivate controls and play gameover clip
    private void Start()
    {
        IsGameOver = false;

        ShipControl.GameOver.AddListener(PlayerDied);
        Enemy.EnemyIsDied.AddListener(AddScore);
        DestroyOnTrigger.EnemyPassed.AddListener(DecreaseScore);
    }

    //Check if Esc button pressed and do staff
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver)
        {
            PauseGame();
        }
        if(Input.GetKeyDown(KeyCode.Space) && IsGameOver)
        {
            Restarted.Invoke();
        }
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            GamePaused.Invoke();

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Continued.Invoke();

            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
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
        if(playerScore > 0)
        {
            playerScore -= pointsToDecrease;
            scoreText.text = "Score: " + playerScore.ToString(); // convert and print score on the screen
        }
    }


    //That what will heppend when player lose
    private void PlayerDied()
    {
        IsItTheBestScore(playerScore);
        IsGameOver = true;
        gameOverScore.text = "Your final score is: " + playerScore.ToString() + "\n Your the best score is " + PlayerPrefs.GetInt("HighScore", 0);
        scoreText.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        _audioSource.PlayOneShot(_GameOverClip);
    }

    //Check if current score is the highest score you ever have
    private void IsItTheBestScore(int currentScore)
    {
        
        if (currentScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }
}

[System.Serializable]
public class EnemyIsDied : UnityEvent<int> { }