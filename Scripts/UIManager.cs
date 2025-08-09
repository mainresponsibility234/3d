using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject gamePanel;
    public GameObject gameOverPanel;
    public GameObject pausePanel;
    
    [Header("Game UI")]
    public Text scoreText;
    public Text highScoreText;
    public Text speedText;
    public Slider healthSlider;
    public Image[] neonUIElements;
    
    [Header("Game Over UI")]
    public Text finalScoreText;
    public Text newHighScoreText;
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Neon Effects")]
    public Color neonUIColor = Color.cyan;
    public float neonPulseSpeed = 2f;
    public float neonPulseAmount = 0.3f;
    
    private bool isPaused = false;
    private float neonPulseTimer = 0f;
    
    void Start()
    {
        // Setup button listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(LoadMainMenu);
        }
        
        // Show main menu by default
        ShowMainMenu();
        
        // Setup neon UI effects
        SetupNeonUI();
    }
    
    void Update()
    {
        // Handle pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
        
        // Update neon effects
        UpdateNeonEffects();
    }
    
    void SetupNeonUI()
    {
        foreach (Image neonElement in neonUIElements)
        {
            if (neonElement != null)
            {
                // Add neon glow effect
                neonElement.color = neonUIColor;
            }
        }
    }
    
    void UpdateNeonEffects()
    {
        neonPulseTimer += Time.deltaTime * neonPulseSpeed;
        float pulse = 1f + Mathf.Sin(neonPulseTimer) * neonPulseAmount;
        
        foreach (Image neonElement in neonUIElements)
        {
            if (neonElement != null)
            {
                Color currentColor = neonUIColor;
                currentColor.a = pulse;
                neonElement.color = currentColor;
            }
        }
    }
    
    public void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (gamePanel != null) gamePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        
        Time.timeScale = 0f;
    }
    
    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (gamePanel != null) gamePanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        
        Time.timeScale = 1f;
    }
    
    public void ShowGameOver(int finalScore, int highScore)
    {
        if (gamePanel != null) gamePanel.SetActive(false);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        
        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore.ToString();
        }
        
        if (newHighScoreText != null)
        {
            if (finalScore >= highScore)
            {
                newHighScoreText.text = "NEW HIGH SCORE!";
                newHighScoreText.gameObject.SetActive(true);
            }
            else
            {
                newHighScoreText.gameObject.SetActive(false);
            }
        }
    }
    
    public void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }
    
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
    
    public void UpdateHighScore(int highScore)
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }
    
    public void UpdateSpeed(float speed)
    {
        if (speedText != null)
        {
            speedText.text = "Speed: " + speed.ToString("F1");
        }
    }
    
    public void UpdateHealth(float healthPercentage)
    {
        if (healthSlider != null)
        {
            healthSlider.value = healthPercentage;
        }
    }
    
    public void SetNeonColor(Color color)
    {
        neonUIColor = color;
        SetupNeonUI();
    }
}
