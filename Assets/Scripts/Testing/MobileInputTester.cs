using UnityEngine;
using UnityEngine.UI;

public class MobileInputTester : MonoBehaviour
{
    [Header("Testing UI")]
    public Text debugText;
    public GameObject virtualJoystick;
    public GameObject virtualButtons;
    
    [Header("Input Display")]
    public Text touchCountText;
    public Text touchPositionText;
    public Text joystickInputText;
    public Text buttonStatesText;
    
    private MobileInputManager inputManager;
    private bool isTestingMode = false;
    
    void Start()
    {
        inputManager = FindObjectOfType<MobileInputManager>();
        SetupTestingUI();
    }
    
    void Update()
    {
        if (isTestingMode)
        {
            UpdateDebugDisplay();
            HandleTestingInput();
        }
    }
    
    void SetupTestingUI()
    {
        // Create debug UI if it doesn't exist
        if (debugText == null)
        {
            CreateDebugUI();
        }
        
        // Show/hide mobile controls based on platform
        #if UNITY_EDITOR
        ShowMobileControls(true);
        #else
        ShowMobileControls(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
        #endif
    }
    
    void CreateDebugUI()
    {
        // Create a debug canvas
        GameObject debugCanvas = new GameObject("DebugCanvas");
        debugCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        debugCanvas.AddComponent<CanvasScaler>();
        debugCanvas.AddComponent<GraphicRaycaster>();
        
        // Create debug text
        GameObject textObj = new GameObject("DebugText");
        textObj.transform.SetParent(debugCanvas.transform);
        debugText = textObj.AddComponent<Text>();
        debugText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        debugText.fontSize = 16;
        debugText.color = Color.white;
        
        RectTransform textRect = debugText.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0, 1);
        textRect.anchorMax = new Vector2(1, 1);
        textRect.offsetMin = new Vector2(10, -200);
        textRect.offsetMax = new Vector2(-10, -10);
    }
    
    void ShowMobileControls(bool show)
    {
        if (virtualJoystick != null)
            virtualJoystick.SetActive(show);
        if (virtualButtons != null)
            virtualButtons.SetActive(show);
    }
    
    void UpdateDebugDisplay()
    {
        if (debugText == null) return;
        
        string debugInfo = "=== MOBILE INPUT TESTER ===\n";
        debugInfo += $"Platform: {Application.platform}\n";
        debugInfo += $"Touch Count: {Input.touchCount}\n";
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            debugInfo += $"Touch Position: {touch.position}\n";
            debugInfo += $"Touch Phase: {touch.phase}\n";
            debugInfo += $"Touch Delta: {touch.deltaPosition}\n";
        }
        
        if (inputManager != null)
        {
            debugInfo += $"Movement Input: {inputManager.MovementInput}\n";
            debugInfo += $"Jump Pressed: {inputManager.JumpPressed}\n";
            debugInfo += $"Interact Pressed: {inputManager.InteractPressed}\n";
        }
        
        debugText.text = debugInfo;
    }
    
    void HandleTestingInput()
    {
        // Toggle testing mode with T key
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTestingMode();
        }
        
        // Simulate touch input with mouse
        if (Input.GetMouseButton(0))
        {
            SimulateTouchInput();
        }
    }
    
    void SimulateTouchInput()
    {
        // Convert mouse input to touch-like input for testing
        Vector2 mousePos = Input.mousePosition;
        Vector2 deltaPos = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        // This simulates touch input for testing in editor
        if (inputManager != null)
        {
            // Simulate movement input
            inputManager.MovementInput = deltaPos * 0.1f;
        }
    }
    
    public void ToggleTestingMode()
    {
        isTestingMode = !isTestingMode;
        if (debugText != null)
        {
            debugText.gameObject.SetActive(isTestingMode);
        }
        
        Debug.Log($"Mobile Input Testing Mode: {(isTestingMode ? "ON" : "OFF")}");
    }
    
    public void OnTestButtonPressed(string buttonName)
    {
        Debug.Log($"Test Button Pressed: {buttonName}");
        
        switch (buttonName)
        {
            case "Jump":
                if (inputManager != null)
                    inputManager.OnJumpButtonPressed();
                break;
            case "Interact":
                if (inputManager != null)
                    inputManager.OnInteractButtonPressed();
                break;
            case "Inventory":
                if (inputManager != null)
                    inputManager.OnInventoryButtonPressed();
                break;
        }
    }
}
