using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Text gameOverText;
    public Button tryAgainButton; 

    private int fireballCollisions = 0;
    private int maxFireballCollisions = 3;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize UI components
        InitializeUI();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            // Check for game over condition
            if (fireballCollisions >= maxFireballCollisions)
            {
                GameOver();
            }
        }
    }

    private void InitializeUI()
    {
        // Add listeners for the button click
        tryAgainButton.onClick.AddListener(GameOverRetry);

        // Set initial text values
        gameOverText.text = "";
        tryAgainButton.gameObject.SetActive(false);
    }

    public void IncrementFireballCollisions()
    {
        if (!isGameOver)
        {
            fireballCollisions++;

            if (fireballCollisions >= maxFireballCollisions)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        gameOverText.text = "Game Over!";
        tryAgainButton.gameObject.SetActive(true);

        Time.timeScale = 0f;
    }

    public void GameOverRetry()
    {
        Time.timeScale = 1f;

        // Reset fireball collisions
        fireballCollisions = 0;

        // Reset game over flag
        isGameOver = false;

        // Hide UI elements
        gameOverText.text = "";
        tryAgainButton.gameObject.SetActive(false);

        // Restart the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
