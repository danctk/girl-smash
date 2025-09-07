using UnityEngine;
using UnityEngine.UI;

public class MobileInputManager : MonoBehaviour
{
    [Header("UI References")]
    public Joystick movementJoystick;
    public Button jumpButton;
    public Button interactButton;
    public Button inventoryButton;
    
    [Header("Settings")]
    public float joystickSensitivity = 1f;
    
    // Input values
    public Vector2 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool InventoryPressed { get; private set; }
    
    private PlayerController playerController;
    
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        SetupUI();
    }
    
    void SetupUI()
    {
        // Create UI elements if they don't exist
        if (movementJoystick == null)
        {
            CreateMovementJoystick();
        }
        
        if (jumpButton == null)
        {
            CreateJumpButton();
        }
        
        if (interactButton == null)
        {
            CreateInteractButton();
        }
        
        if (inventoryButton == null)
        {
            CreateInventoryButton();
        }
    }
    
    void CreateMovementJoystick()
    {
        // This would typically be set up in the Unity Editor
        // For now, we'll handle input through touch
    }
    
    void CreateJumpButton()
    {
        // This would typically be set up in the Unity Editor
    }
    
    void CreateInteractButton()
    {
        // This would typically be set up in the Unity Editor
    }
    
    void CreateInventoryButton()
    {
        // This would typically be set up in the Unity Editor
    }
    
    void Update()
    {
        HandleTouchInput();
        HandleButtonInput();
    }
    
    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            // Movement input
            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                MovementInput = touch.deltaPosition * joystickSensitivity;
            }
            else
            {
                MovementInput = Vector2.zero;
            }
            
            // Jump input (tap to jump)
            if (touch.phase == TouchPhase.Began)
            {
                JumpPressed = true;
            }
        }
        else
        {
            MovementInput = Vector2.zero;
            JumpPressed = false;
        }
    }
    
    void HandleButtonInput()
    {
        // Button inputs are handled by UI events
        // These would be connected in the Unity Editor
    }
    
    // Button event methods
    public void OnJumpButtonPressed()
    {
        JumpPressed = true;
    }
    
    public void OnJumpButtonReleased()
    {
        JumpPressed = false;
    }
    
    public void OnInteractButtonPressed()
    {
        InteractPressed = true;
    }
    
    public void OnInteractButtonReleased()
    {
        InteractPressed = false;
    }
    
    public void OnInventoryButtonPressed()
    {
        InventoryPressed = true;
    }
    
    public void OnInventoryButtonReleased()
    {
        InventoryPressed = false;
    }
}
