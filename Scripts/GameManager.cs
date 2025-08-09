using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [Header("UI References")]
    public Text scoreText;
    public Text highScoreText;
    public GameObject gameOverPanel;
    public Button restartButton;
    public Button mainMenuButton;
    
    [Header("Game Settings")]
    public float gameSpeed = 10f;
    public float speedIncreaseRate = 0.1f;
    public float maxSpeed = 25f;
    
    [Header("Neon Environment")]
    public Light[] neonLights;
    public Material[] neonMaterials;
    public ParticleSystem[] backgroundParticles;
    
    private int currentScore = 0;
    private int highScore = 0;
    private bool isGameActive = true;
    private float gameTime = 0f;
    
    void Awake()
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
    
    void Start()
    {
        // Load high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
        
        // Setup UI buttons
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }
        
        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(LoadMainMenu);
        }
        
        // Setup neon environment
        SetupNeonEnvironment();
    }
    
    void Update()
    {
        if (isGameActive)
        {
            gameTime += Time.deltaTime;
            currentScore = Mathf.FloorToInt(gameTime * 10f);
            
            // Increase game speed over time
            gameSpeed = Mathf.Min(maxSpeed, gameSpeed + speedIncreaseRate * Time.deltaTime);
            
            UpdateUI();
            UpdateNeonEffects();
        }
    }
    
    void SetupNeonEnvironment()
    {
        // Setup neon lights
        foreach (Light light in neonLights)
        {
            if (light != null)
            {
                light.color = Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f);
                light.intensity = Random.Range(1f, 3f);
            }
        }
        
        // Setup neon materials
        foreach (Material material in neonMaterials)
        {
            if (material != null)
            {
                material.SetColor("_EmissionColor", Random.ColorHSV(0f, 1f, 0.8f, 1f, 0.8f, 1f));
                material.EnableKeyword("_EMISSION");
            }
        }
        
        // Start background particles
        foreach (ParticleSystem particles in backgroundParticles)
        {
            if (particles != null)
            {
                particles.Play();
            }
        }
    }
    
    void UpdateNeonEffects()
    {
        // Animate neon lights
        foreach (Light light in neonLights)
        {
            if (light != null)
            {
                light.intensity = 2f + Mathf.Sin(Time.time * 2f + light.transform.position.x) * 0.5f;
            }
        }
        
        // Animate neon materials
        foreach (Material material in neonMaterials)
        {
            if (material != null)
            {
                Color emissionColor = material.GetColor("_EmissionColor");
                float intensity = 1f + Mathf.Sin(Time.time * 3f) * 0.3f;
                material.SetColor("_EmissionColor", emissionColor * intensity);
            }
        }
    }
    
    public void AddScore(int points)
    {
        if (isGameActive)
        {
            currentScore += points;
            UpdateUI();
        }
    }
    
    public void GameOver()
    {
        isGameActive = false;
        
        // Update high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        
        // Show game over panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        // Stop background particles
        foreach (ParticleSystem particles in backgroundParticles)
        {
            if (particles != null)
            {
                particles.Stop();
            }
        }
    }
    
    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore.ToString();
        }
        
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore.ToString();
        }
    }
    
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public float GetGameSpeed()
    {
        return gameSpeed;
    }
    
    public bool IsGameActive()
    {
        return isGameActive;
    }
}
