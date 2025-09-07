using UnityEngine;

public class MobileInputBridge : MonoBehaviour
{
    [Header("Input Sources")]
    public MobileInputManager mobileInputManager;
    public UnityEditorMobileInput.MobileInputSimulator editorSimulator;
    
    [Header("Settings")]
    public bool useEditorSimulation = true;
    
    // Input values
    public Vector2 MovementInput { get; private set; }
    public bool JumpPressed { get; private set; }
    public bool InteractPressed { get; private set; }
    public bool InventoryPressed { get; private set; }
    
    void Start()
    {
        // Get references
        if (mobileInputManager == null)
            mobileInputManager = FindObjectOfType<MobileInputManager>();
        
        if (editorSimulator == null)
            editorSimulator = new UnityEditorMobileInput.MobileInputSimulator();
        
        // Determine input source based on platform
        #if UNITY_EDITOR
        useEditorSimulation = true;
        #else
        useEditorSimulation = false;
        #endif
    }
    
    void Update()
    {
        UpdateInput();
    }
    
    void UpdateInput()
    {
        if (useEditorSimulation)
        {
            UpdateEditorInput();
        }
        else
        {
            UpdateMobileInput();
        }
    }
    
    void UpdateEditorInput()
    {
        if (editorSimulator != null)
        {
            editorSimulator.Update();
            
            MovementInput = editorSimulator.GetJoystickInput();
            JumpPressed = editorSimulator.GetJumpPressed();
            InteractPressed = editorSimulator.GetInteractPressed();
            InventoryPressed = editorSimulator.GetInventoryPressed();
            
            // Reset button states after reading
            editorSimulator.ResetButtonStates();
        }
    }
    
    void UpdateMobileInput()
    {
        if (mobileInputManager != null)
        {
            MovementInput = mobileInputManager.MovementInput;
            JumpPressed = mobileInputManager.JumpPressed;
            InteractPressed = mobileInputManager.InteractPressed;
            InventoryPressed = mobileInputManager.InventoryPressed;
        }
    }
    
    void OnGUI()
    {
        if (useEditorSimulation && editorSimulator != null)
        {
            editorSimulator.OnGUI();
        }
    }
    
    // Public methods for other scripts to get input
    public Vector2 GetMovementInput()
    {
        return MovementInput;
    }
    
    public bool IsJumpPressed()
    {
        return JumpPressed;
    }
    
    public bool IsInteractPressed()
    {
        return InteractPressed;
    }
    
    public bool IsInventoryPressed()
    {
        return InventoryPressed;
    }
    
    // Method to switch input mode (useful for testing)
    public void SetInputMode(bool useEditor)
    {
        useEditorSimulation = useEditor;
        Debug.Log($"Input mode switched to: {(useEditor ? "Editor Simulation" : "Mobile Input")}");
    }
}
