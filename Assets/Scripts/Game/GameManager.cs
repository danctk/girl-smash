using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float gameTime = 300f; // 5 minutes
    public int maxLives = 3;
    
    [Header("UI References")]
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject inventoryUI;
    
    // Game state
    public static GameManager Instance { get; private set; }
    public bool IsGamePaused { get; private set; }
    public bool IsGameOver { get; private set; }
    public int CurrentLives { get; private set; }
    public float CurrentTime { get; private set; }
    
    // Events
    public System.Action OnGamePaused;
    public System.Action OnGameResumed;
    public System.Action OnGameOver;
    public System.Action OnLivesChanged;
    public System.Action OnTimeChanged;
    
    void Awake()
    {
        // Singleton pattern
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
        InitializeGame();
    }
    
    void Update()
    {
        HandleInput();
        UpdateGameTime();
    }
    
    void InitializeGame()
    {
        CurrentLives = maxLives;
        CurrentTime = gameTime;
        IsGamePaused = false;
        IsGameOver = false;
        
        Time.timeScale = 1f;
        
        // Initialize UI
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (inventoryUI != null) inventoryUI.SetActive(false);
    }
    
    void HandleInput()
    {
        // Pause/Resume game
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (IsGamePaused)
                ResumeGame();
            else
                PauseGame();
        }
        
        // Inventory toggle
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }
    
    void UpdateGameTime()
    {
        if (!IsGamePaused && !IsGameOver)
        {
            CurrentTime -= Time.deltaTime;
            OnTimeChanged?.Invoke();
            
            if (CurrentTime <= 0)
            {
                GameOver();
            }
        }
    }
    
    public void PauseGame()
    {
        IsGamePaused = true;
        Time.timeScale = 0f;
        
        if (pauseMenu != null)
            pauseMenu.SetActive(true);
        
        OnGamePaused?.Invoke();
    }
    
    public void ResumeGame()
    {
        IsGamePaused = false;
        Time.timeScale = 1f;
        
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
        
        OnGameResumed?.Invoke();
    }
    
    public void ToggleInventory()
    {
        if (inventoryUI != null)
        {
            bool isActive = inventoryUI.activeInHierarchy;
            inventoryUI.SetActive(!isActive);
        }
    }
    
    public void LoseLife()
    {
        CurrentLives--;
        OnLivesChanged?.Invoke();
        
        if (CurrentLives <= 0)
        {
            GameOver();
        }
    }
    
    public void AddLife()
    {
        CurrentLives++;
        OnLivesChanged?.Invoke();
    }
    
    public void GameOver()
    {
        IsGameOver = true;
        Time.timeScale = 0f;
        
        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);
        
        OnGameOver?.Invoke();
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
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
