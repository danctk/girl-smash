using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("HUD Elements")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI livesText;
    public Slider healthBar;
    public Slider timeBar;
    
    [Header("Mobile UI")]
    public GameObject mobileControls;
    public Joystick movementJoystick;
    public Button jumpButton;
    public Button interactButton;
    public Button inventoryButton;
    public Button pauseButton;
    
    [Header("Menus")]
    public GameObject pauseMenu;
    public GameObject inventoryMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    
    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public Image speakerPortrait;
    public Button continueButton;
    
    private GameManager gameManager;
    private PlayerController playerController;
    private InventorySystem inventorySystem;
    private DialogueSystem dialogueSystem;
    
    void Start()
    {
        // Get references
        gameManager = GameManager.Instance;
        playerController = FindObjectOfType<PlayerController>();
        inventorySystem = FindObjectOfType<InventorySystem>();
        dialogueSystem = FindObjectOfType<DialogueSystem>();
        
        // Set up mobile controls
        SetupMobileControls();
        
        // Subscribe to events
        SubscribeToEvents();
        
        // Initialize UI
        InitializeUI();
    }
    
    void SetupMobileControls()
    {
        // Enable mobile controls on mobile platforms
        #if UNITY_ANDROID || UNITY_IOS
        if (mobileControls != null)
            mobileControls.SetActive(true);
        #else
        if (mobileControls != null)
            mobileControls.SetActive(false);
        #endif
    }
    
    void SubscribeToEvents()
    {
        if (gameManager != null)
        {
            gameManager.OnGamePaused += OnGamePaused;
            gameManager.OnGameResumed += OnGameResumed;
            gameManager.OnGameOver += OnGameOver;
            gameManager.OnLivesChanged += UpdateLivesDisplay;
            gameManager.OnTimeChanged += UpdateTimeDisplay;
        }
    }
    
    void InitializeUI()
    {
        // Initialize all UI elements
        UpdateHealthDisplay();
        UpdateLivesDisplay();
        UpdateTimeDisplay();
        
        // Hide menus
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (inventoryMenu != null) inventoryMenu.SetActive(false);
        if (settingsMenu != null) settingsMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        if (dialoguePanel != null) dialoguePanel.SetActive(false);
    }
    
    void Update()
    {
        UpdateHealthDisplay();
        UpdateTimeDisplay();
    }
    
    void UpdateHealthDisplay()
    {
        if (playerController != null)
        {
            // This would be connected to player health
            if (healthText != null)
                healthText.text = "Health: 100"; // Placeholder
            
            if (healthBar != null)
                healthBar.value = 1f; // Placeholder
        }
    }
    
    void UpdateLivesDisplay()
    {
        if (gameManager != null && livesText != null)
        {
            livesText.text = $"Lives: {gameManager.CurrentLives}";
        }
    }
    
    void UpdateTimeDisplay()
    {
        if (gameManager != null)
        {
            if (timeText != null)
            {
                int minutes = Mathf.FloorToInt(gameManager.CurrentTime / 60);
                int seconds = Mathf.FloorToInt(gameManager.CurrentTime % 60);
                timeText.text = $"{minutes:00}:{seconds:00}";
            }
            
            if (timeBar != null)
            {
                timeBar.value = gameManager.CurrentTime / gameManager.gameTime;
            }
        }
    }
    
    // Event handlers
    void OnGamePaused()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(true);
    }
    
    void OnGameResumed()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }
    
    void OnGameOver()
    {
        if (gameOverMenu != null)
            gameOverMenu.SetActive(true);
    }
    
    // Button methods
    public void OnPauseButtonPressed()
    {
        if (gameManager != null)
        {
            if (gameManager.IsGamePaused)
                gameManager.ResumeGame();
            else
                gameManager.PauseGame();
        }
    }
    
    public void OnInventoryButtonPressed()
    {
        if (inventorySystem != null)
        {
            inventorySystem.ToggleInventory();
        }
    }
    
    public void OnSettingsButtonPressed()
    {
        if (settingsMenu != null)
        {
            settingsMenu.SetActive(!settingsMenu.activeInHierarchy);
        }
    }
    
    public void OnResumeButtonPressed()
    {
        if (gameManager != null)
            gameManager.ResumeGame();
    }
    
    public void OnRestartButtonPressed()
    {
        if (gameManager != null)
            gameManager.RestartGame();
    }
    
    public void OnMainMenuButtonPressed()
    {
        if (gameManager != null)
            gameManager.LoadMainMenu();
    }
    
    public void OnQuitButtonPressed()
    {
        if (gameManager != null)
            gameManager.QuitGame();
    }
    
    void OnDestroy()
    {
        // Unsubscribe from events
        if (gameManager != null)
        {
            gameManager.OnGamePaused -= OnGamePaused;
            gameManager.OnGameResumed -= OnGameResumed;
            gameManager.OnGameOver -= OnGameOver;
            gameManager.OnLivesChanged -= UpdateLivesDisplay;
            gameManager.OnTimeChanged -= UpdateTimeDisplay;
        }
    }
}
