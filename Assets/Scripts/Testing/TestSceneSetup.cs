using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestSceneSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    public bool setupOnStart = true;
    public bool createTestEnvironment = true;
    public bool createTestUI = true;
    public bool addTestingComponents = true;
    
    [Header("Test Configuration")]
    public string testSceneName = "TestScene";
    public bool runTestsAfterSetup = true;
    
    void Start()
    {
        if (setupOnStart)
        {
            SetupTestScene();
        }
    }
    
    [ContextMenu("Setup Test Scene")]
    public void SetupTestScene()
    {
        Debug.Log("=== SETTING UP TEST SCENE ===");
        
        if (createTestEnvironment)
        {
            CreateTestEnvironment();
        }
        
        if (createTestUI)
        {
            CreateTestUI();
        }
        
        if (addTestingComponents)
        {
            AddTestingComponents();
        }
        
        Debug.Log("=== TEST SCENE SETUP COMPLETE ===");
        
        if (runTestsAfterSetup)
        {
            StartCoroutine(RunInitialTests());
        }
    }
    
    void CreateTestEnvironment()
    {
        Debug.Log("Creating test environment...");
        
        // Create ground
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "TestGround";
        ground.transform.position = Vector3.zero;
        ground.transform.localScale = new Vector3(20, 1, 20);
        
        // Add ground material
        Renderer groundRenderer = ground.GetComponent<Renderer>();
        Material groundMaterial = new Material(Shader.Find("Standard"));
        groundMaterial.color = new Color(0.5f, 0.5f, 0.5f);
        groundRenderer.material = groundMaterial;
        
        // Create test objects
        CreateTestObjects();
        
        // Create lighting
        CreateLighting();
        
        // Create player
        CreatePlayer();
        
        // Create camera
        CreateCamera();
        
        Debug.Log("✓ Test environment created");
    }
    
    void CreateTestObjects()
    {
        // Create collectibles
        for (int i = 0; i < 10; i++)
        {
            GameObject collectible = GameObject.CreatePrimitive(PrimitiveType.Cube);
            collectible.name = $"Collectible_{i}";
            collectible.transform.position = new Vector3(
                Random.Range(-8f, 8f), 
                0.5f, 
                Random.Range(-8f, 8f)
            );
            collectible.transform.localScale = Vector3.one * 0.5f;
            
            // Add collectible component
            Collectible collectibleScript = collectible.AddComponent<Collectible>();
            collectibleScript.itemId = $"item_{i}";
            collectibleScript.itemName = $"Test Item {i}";
            collectibleScript.autoCollect = true;
            
            // Make it look like a collectible
            Renderer renderer = collectible.GetComponent<Renderer>();
            Material material = new Material(Shader.Find("Standard"));
            material.color = Color.yellow;
            renderer.material = material;
        }
        
        // Create doors
        for (int i = 0; i < 3; i++)
        {
            GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
            door.name = $"Door_{i}";
            door.transform.position = new Vector3(i * 6f - 6f, 1.5f, 8f);
            door.transform.localScale = new Vector3(1f, 3f, 0.2f);
            
            // Add door component
            Door doorScript = door.AddComponent<Door>();
            doorScript.isLocked = i == 0; // First door is locked
            doorScript.keyId = "door_key";
            doorScript.interactionText = doorScript.isLocked ? "Door is locked" : "Open Door";
            
            // Make it look like a door
            Renderer renderer = door.GetComponent<Renderer>();
            Material material = new Material(Shader.Find("Standard"));
            material.color = doorScript.isLocked ? Color.red : Color.green;
            renderer.material = material;
        }
        
        // Create NPCs
        for (int i = 0; i < 3; i++)
        {
            GameObject npc = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            npc.name = $"NPC_{i}";
            npc.transform.position = new Vector3(i * 6f - 6f, 1f, -6f);
            npc.transform.localScale = new Vector3(1f, 1.5f, 1f);
            
            // Add NPC component
            NPC npcScript = npc.AddComponent<NPC>();
            npcScript.npcName = $"Test NPC {i}";
            npcScript.dialogueIds = new string[] { $"dialogue_{i}" };
            npcScript.interactionText = $"Talk to {npcScript.npcName}";
            
            // Make it look like an NPC
            Renderer renderer = npc.GetComponent<Renderer>();
            Material material = new Material(Shader.Find("Standard"));
            material.color = Color.blue;
            renderer.material = material;
        }
        
        Debug.Log("✓ Test objects created");
    }
    
    void CreateLighting()
    {
        // Create directional light
        GameObject lightObj = new GameObject("TestLight");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        light.color = Color.white;
        lightObj.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        
        // Set ambient lighting
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.5f, 0.5f, 0.6f);
        RenderSettings.ambientEquatorColor = new Color(0.4f, 0.4f, 0.4f);
        RenderSettings.ambientGroundColor = new Color(0.3f, 0.3f, 0.3f);
        
        Debug.Log("✓ Lighting created");
    }
    
    void CreatePlayer()
    {
        // Create player object
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.transform.position = new Vector3(0, 1, 0);
        player.transform.localScale = new Vector3(1f, 1.5f, 1f);
        
        // Add player controller
        PlayerController playerController = player.AddComponent<PlayerController>();
        
        // Add character controller
        CharacterController characterController = player.AddComponent<CharacterController>();
        characterController.height = 2f;
        characterController.radius = 0.5f;
        characterController.center = new Vector3(0, 1, 0);
        
        // Add ground check
        GameObject groundCheck = new GameObject("GroundCheck");
        groundCheck.transform.SetParent(player.transform);
        groundCheck.transform.localPosition = new Vector3(0, -1f, 0);
        playerController.groundCheck = groundCheck.transform;
        
        // Make player look different
        Renderer renderer = player.GetComponent<Renderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.green;
        renderer.material = material;
        
        // Add player tag
        player.tag = "Player";
        
        Debug.Log("✓ Player created");
    }
    
    void CreateCamera()
    {
        // Create camera object
        GameObject cameraObj = new GameObject("Main Camera");
        Camera camera = cameraObj.AddComponent<Camera>();
        cameraObj.transform.position = new Vector3(0, 5, -10);
        cameraObj.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
        
        // Add camera controller
        CameraController cameraController = cameraObj.AddComponent<CameraController>();
        
        // Set camera target
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            cameraController.target = player.transform;
        }
        
        Debug.Log("✓ Camera created");
    }
    
    void CreateTestUI()
    {
        Debug.Log("Creating test UI...");
        
        // Create canvas
        GameObject canvasObj = new GameObject("TestCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        canvasObj.AddComponent<GraphicRaycaster>();
        
        // Create test status panel
        CreateTestStatusPanel(canvasObj);
        
        // Create mobile controls
        CreateMobileControls(canvasObj);
        
        // Create debug panel
        CreateDebugPanel(canvasObj);
        
        Debug.Log("✓ Test UI created");
    }
    
    void CreateTestStatusPanel(GameObject canvas)
    {
        // Create status panel
        GameObject statusPanel = new GameObject("StatusPanel");
        statusPanel.transform.SetParent(canvas.transform);
        
        RectTransform statusRect = statusPanel.AddComponent<RectTransform>();
        statusRect.anchorMin = new Vector2(0, 1);
        statusRect.anchorMax = new Vector2(1, 1);
        statusRect.offsetMin = new Vector2(0, -120);
        statusRect.offsetMax = new Vector2(0, 0);
        
        Image statusImage = statusPanel.AddComponent<Image>();
        statusImage.color = new Color(0, 0, 0, 0.7f);
        
        // Create status text
        GameObject statusText = new GameObject("StatusText");
        statusText.transform.SetParent(statusPanel.transform);
        
        TextMeshProUGUI text = statusText.AddComponent<TextMeshProUGUI>();
        text.text = "Test Scene Ready - Press T to toggle testing mode";
        text.fontSize = 18;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = statusText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
    }
    
    void CreateMobileControls(GameObject canvas)
    {
        // Create mobile controls panel
        GameObject controlsPanel = new GameObject("MobileControls");
        controlsPanel.transform.SetParent(canvas.transform);
        
        RectTransform controlsRect = controlsPanel.AddComponent<RectTransform>();
        controlsRect.anchorMin = new Vector2(0, 0);
        controlsRect.anchorMax = new Vector2(1, 1);
        controlsRect.offsetMin = Vector2.zero;
        controlsRect.offsetMax = Vector2.zero;
        
        // Create virtual joystick
        CreateVirtualJoystick(controlsPanel);
        
        // Create action buttons
        CreateActionButtons(controlsPanel);
    }
    
    void CreateVirtualJoystick(GameObject parent)
    {
        // Joystick background
        GameObject joystickBG = new GameObject("JoystickBackground");
        joystickBG.transform.SetParent(parent.transform);
        
        RectTransform bgRect = joystickBG.AddComponent<RectTransform>();
        bgRect.anchorMin = new Vector2(0, 0);
        bgRect.anchorMax = new Vector2(0, 0);
        bgRect.anchoredPosition = new Vector2(150, 150);
        bgRect.sizeDelta = new Vector2(200, 200);
        
        Image bgImage = joystickBG.AddComponent<Image>();
        bgImage.color = new Color(1, 1, 1, 0.3f);
        
        // Joystick handle
        GameObject joystickHandle = new GameObject("JoystickHandle");
        joystickHandle.transform.SetParent(joystickBG.transform);
        
        RectTransform handleRect = joystickHandle.AddComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0.5f, 0.5f);
        handleRect.anchorMax = new Vector2(0.5f, 0.5f);
        handleRect.anchoredPosition = Vector2.zero;
        handleRect.sizeDelta = new Vector2(80, 80);
        
        Image handleImage = joystickHandle.AddComponent<Image>();
        handleImage.color = new Color(1, 1, 1, 0.8f);
        
        // Add joystick component
        Joystick joystick = joystickBG.AddComponent<Joystick>();
        joystick.background = bgRect;
        joystick.handle = handleRect;
    }
    
    void CreateActionButtons(GameObject parent)
    {
        // Jump button
        CreateActionButton(parent, "JumpButton", "JUMP", new Vector2(1700, 150), new Vector2(150, 150), Color.green);
        
        // Interact button
        CreateActionButton(parent, "InteractButton", "INTERACT", new Vector2(1500, 150), new Vector2(150, 150), Color.blue);
        
        // Inventory button
        CreateActionButton(parent, "InventoryButton", "INVENTORY", new Vector2(1300, 150), new Vector2(150, 150), Color.yellow);
    }
    
    void CreateActionButton(GameObject parent, string name, string text, Vector2 position, Vector2 size, Color color)
    {
        GameObject button = new GameObject(name);
        button.transform.SetParent(parent.transform);
        
        RectTransform buttonRect = button.AddComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0, 0);
        buttonRect.anchorMax = new Vector2(0, 0);
        buttonRect.anchoredPosition = position;
        buttonRect.sizeDelta = size;
        
        Image buttonImage = button.AddComponent<Image>();
        buttonImage.color = color;
        
        Button buttonComponent = button.AddComponent<Button>();
        
        // Button text
        GameObject buttonText = new GameObject("Text");
        buttonText.transform.SetParent(button.transform);
        
        TextMeshProUGUI textComponent = buttonText.AddComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.fontSize = 14;
        textComponent.color = Color.white;
        textComponent.alignment = TextAlignmentOptions.Center;
        
        RectTransform textRect = buttonText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
    }
    
    void CreateDebugPanel(GameObject canvas)
    {
        // Create debug panel
        GameObject debugPanel = new GameObject("DebugPanel");
        debugPanel.transform.SetParent(canvas.transform);
        
        RectTransform debugRect = debugPanel.AddComponent<RectTransform>();
        debugRect.anchorMin = new Vector2(1, 1);
        debugRect.anchorMax = new Vector2(1, 1);
        debugRect.anchoredPosition = new Vector2(-200, -200);
        debugRect.sizeDelta = new Vector2(300, 200);
        
        Image debugImage = debugPanel.AddComponent<Image>();
        debugImage.color = new Color(0, 0, 0, 0.5f);
        
        // Create debug text
        GameObject debugText = new GameObject("DebugText");
        debugText.transform.SetParent(debugPanel.transform);
        
        TextMeshProUGUI text = debugText.AddComponent<TextMeshProUGUI>();
        text.text = "Debug Info\nFPS: 60\nMemory: 0MB\nInput: None";
        text.fontSize = 12;
        text.color = Color.white;
        text.alignment = TextAlignmentOptions.TopLeft;
        
        RectTransform textRect = debugText.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 10);
        textRect.offsetMax = new Vector2(-10, -10);
    }
    
    void AddTestingComponents()
    {
        Debug.Log("Adding testing components...");
        
        // Add testing components to the scene
        if (FindObjectOfType<AndroidTestManager>() == null)
        {
            GameObject testManager = new GameObject("AndroidTestManager");
            testManager.AddComponent<AndroidTestManager>();
        }
        
        if (FindObjectOfType<MobileInputTester>() == null)
        {
            GameObject inputTester = new GameObject("MobileInputTester");
            inputTester.AddComponent<MobileInputTester>();
        }
        
        if (FindObjectOfType<AndroidEmulatorTester>() == null)
        {
            GameObject emulatorTester = new GameObject("AndroidEmulatorTester");
            emulatorTester.AddComponent<AndroidEmulatorTester>();
        }
        
        if (FindObjectOfType<MobileInputBridge>() == null)
        {
            GameObject inputBridge = new GameObject("MobileInputBridge");
            inputBridge.AddComponent<MobileInputBridge>();
        }
        
        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject gameManager = new GameObject("GameManager");
            gameManager.AddComponent<GameManager>();
        }
        
        if (FindObjectOfType<InventorySystem>() == null)
        {
            GameObject inventorySystem = new GameObject("InventorySystem");
            inventorySystem.AddComponent<InventorySystem>();
        }
        
        if (FindObjectOfType<DialogueSystem>() == null)
        {
            GameObject dialogueSystem = new GameObject("DialogueSystem");
            dialogueSystem.AddComponent<DialogueSystem>();
        }
        
        if (FindObjectOfType<QuestSystem>() == null)
        {
            GameObject questSystem = new GameObject("QuestSystem");
            questSystem.AddComponent<QuestSystem>();
        }
        
        Debug.Log("✓ Testing components added");
    }
    
    System.Collections.IEnumerator RunInitialTests()
    {
        Debug.Log("Running initial tests...");
        
        yield return new WaitForSeconds(2f);
        
        // Test 1: Check if all components exist
        TestComponentsExist();
        
        yield return new WaitForSeconds(1f);
        
        // Test 2: Test input system
        TestInputSystem();
        
        yield return new WaitForSeconds(1f);
        
        // Test 3: Test game systems
        TestGameSystems();
        
        yield return new WaitForSeconds(1f);
        
        // Test 4: Test mobile simulation
        TestMobileSimulation();
        
        Debug.Log("✓ Initial tests complete!");
    }
    
    void TestComponentsExist()
    {
        Debug.Log("Testing component existence...");
        
        bool allComponentsExist = true;
        
        // Check core components
        if (FindObjectOfType<PlayerController>() == null)
        {
            Debug.LogError("✗ PlayerController not found");
            allComponentsExist = false;
        }
        else
        {
            Debug.Log("✓ PlayerController found");
        }
        
        if (FindObjectOfType<CameraController>() == null)
        {
            Debug.LogError("✗ CameraController not found");
            allComponentsExist = false;
        }
        else
        {
            Debug.Log("✓ CameraController found");
        }
        
        if (FindObjectOfType<GameManager>() == null)
        {
            Debug.LogError("✗ GameManager not found");
            allComponentsExist = false;
        }
        else
        {
            Debug.Log("✓ GameManager found");
        }
        
        if (allComponentsExist)
        {
            Debug.Log("✓ All core components exist");
        }
    }
    
    void TestInputSystem()
    {
        Debug.Log("Testing input system...");
        
        MobileInputBridge inputBridge = FindObjectOfType<MobileInputBridge>();
        if (inputBridge != null)
        {
            Debug.Log("✓ MobileInputBridge found");
            
            Vector2 movement = inputBridge.GetMovementInput();
            Debug.Log($"Movement input: {movement}");
        }
        else
        {
            Debug.LogError("✗ MobileInputBridge not found");
        }
    }
    
    void TestGameSystems()
    {
        Debug.Log("Testing game systems...");
        
        // Test inventory system
        InventorySystem inventory = FindObjectOfType<InventorySystem>();
        if (inventory != null)
        {
            Debug.Log("✓ InventorySystem found");
        }
        else
        {
            Debug.LogError("✗ InventorySystem not found");
        }
        
        // Test dialogue system
        DialogueSystem dialogue = FindObjectOfType<DialogueSystem>();
        if (dialogue != null)
        {
            Debug.Log("✓ DialogueSystem found");
        }
        else
        {
            Debug.LogError("✗ DialogueSystem not found");
        }
        
        // Test quest system
        QuestSystem quest = FindObjectOfType<QuestSystem>();
        if (quest != null)
        {
            Debug.Log("✓ QuestSystem found");
        }
        else
        {
            Debug.LogError("✗ QuestSystem not found");
        }
    }
    
    void TestMobileSimulation()
    {
        Debug.Log("Testing mobile simulation...");
        
        #if UNITY_EDITOR
        Debug.Log("✓ Running in Unity Editor - mobile simulation available");
        #else
        Debug.Log("✓ Running on target platform - native mobile input available");
        #endif
    }
    
    [ContextMenu("Run Initial Tests")]
    public void RunInitialTestsManual()
    {
        StartCoroutine(RunInitialTests());
    }
}
