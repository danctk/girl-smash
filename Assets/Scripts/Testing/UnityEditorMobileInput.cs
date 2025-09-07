using UnityEngine;
using UnityEditor;

[System.Serializable]
public class MobileInputSimulator
{
    [Header("Simulation Settings")]
    public bool simulateTouch = true;
    public float touchSensitivity = 1f;
    public bool showTouchGizmos = true;
    
    [Header("Virtual Joystick")]
    public Vector2 joystickCenter = new Vector2(200, 200);
    public float joystickRadius = 100f;
    public Vector2 joystickInput = Vector2.zero;
    
    [Header("Virtual Buttons")]
    public Rect jumpButtonRect = new Rect(50, 50, 100, 100);
    public Rect interactButtonRect = new Rect(150, 50, 100, 100);
    public Rect inventoryButtonRect = new Rect(250, 50, 100, 100);
    
    private bool isJumpPressed = false;
    private bool isInteractPressed = false;
    private bool isInventoryPressed = false;
    
    public void Update()
    {
        if (!simulateTouch) return;
        
        HandleMouseAsTouch();
        UpdateVirtualJoystick();
        UpdateVirtualButtons();
    }
    
    void HandleMouseAsTouch()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            
            // Simulate touch input
            SimulateTouchInput(mousePos, mouseDelta);
        }
    }
    
    void SimulateTouchInput(Vector2 position, Vector2 delta)
    {
        // Check if mouse is over virtual joystick
        if (Vector2.Distance(position, joystickCenter) <= joystickRadius)
        {
            joystickInput = (position - joystickCenter) / joystickRadius;
            joystickInput = Vector2.ClampMagnitude(joystickInput, 1f);
        }
        else
        {
            joystickInput = Vector2.zero;
        }
        
        // Check virtual buttons
        isJumpPressed = jumpButtonRect.Contains(position) && Input.GetMouseButtonDown(0);
        isInteractPressed = interactButtonRect.Contains(position) && Input.GetMouseButtonDown(0);
        isInventoryPressed = inventoryButtonRect.Contains(position) && Input.GetMouseButtonDown(0);
    }
    
    void UpdateVirtualJoystick()
    {
        // Handle keyboard input as joystick
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            joystickInput = new Vector2(horizontal, vertical);
        }
    }
    
    void UpdateVirtualButtons()
    {
        // Handle keyboard input as buttons
        if (Input.GetKeyDown(KeyCode.Space))
            isJumpPressed = true;
        if (Input.GetKeyDown(KeyCode.E))
            isInteractPressed = true;
        if (Input.GetKeyDown(KeyCode.I))
            isInventoryPressed = true;
    }
    
    public void OnGUI()
    {
        if (!simulateTouch) return;
        
        // Draw virtual joystick
        DrawVirtualJoystick();
        
        // Draw virtual buttons
        DrawVirtualButtons();
        
        // Draw touch simulation info
        DrawTouchInfo();
    }
    
    void DrawVirtualJoystick()
    {
        // Draw joystick background
        GUI.color = new Color(1, 1, 1, 0.5f);
        GUI.DrawTexture(new Rect(joystickCenter.x - joystickRadius, Screen.height - joystickCenter.y - joystickRadius, 
                                joystickRadius * 2, joystickRadius * 2), Texture2D.whiteTexture);
        
        // Draw joystick handle
        Vector2 handlePos = joystickCenter + joystickInput * joystickRadius * 0.7f;
        GUI.color = Color.white;
        GUI.DrawTexture(new Rect(handlePos.x - 20, Screen.height - handlePos.y - 20, 40, 40), Texture2D.whiteTexture);
        
        GUI.color = Color.white;
    }
    
    void DrawVirtualButtons()
    {
        // Jump button
        GUI.color = isJumpPressed ? Color.green : Color.white;
        if (GUI.Button(new Rect(jumpButtonRect.x, Screen.height - jumpButtonRect.y - jumpButtonRect.height, 
                               jumpButtonRect.width, jumpButtonRect.height), "JUMP"))
        {
            isJumpPressed = true;
        }
        
        // Interact button
        GUI.color = isInteractPressed ? Color.green : Color.white;
        if (GUI.Button(new Rect(interactButtonRect.x, Screen.height - interactButtonRect.y - interactButtonRect.height, 
                               interactButtonRect.width, interactButtonRect.height), "INTERACT"))
        {
            isInteractPressed = true;
        }
        
        // Inventory button
        GUI.color = isInventoryPressed ? Color.green : Color.white;
        if (GUI.Button(new Rect(inventoryButtonRect.x, Screen.height - inventoryButtonRect.y - inventoryButtonRect.height, 
                               inventoryButtonRect.width, inventoryButtonRect.height), "INVENTORY"))
        {
            isInventoryPressed = true;
        }
        
        GUI.color = Color.white;
    }
    
    void DrawTouchInfo()
    {
        GUI.color = Color.white;
        GUI.Label(new Rect(10, 10, 300, 100), 
                 $"Joystick Input: {joystickInput}\n" +
                 $"Jump: {isJumpPressed}\n" +
                 $"Interact: {isInteractPressed}\n" +
                 $"Inventory: {isInventoryPressed}");
    }
    
    // Getters for input values
    public Vector2 GetJoystickInput() => joystickInput;
    public bool GetJumpPressed() => isJumpPressed;
    public bool GetInteractPressed() => isInteractPressed;
    public bool GetInventoryPressed() => isInventoryPressed;
    
    // Reset button states
    public void ResetButtonStates()
    {
        isJumpPressed = false;
        isInteractPressed = false;
        isInventoryPressed = false;
    }
}
