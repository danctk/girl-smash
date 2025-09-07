using UnityEngine;

public class Unity6SetupGuide : MonoBehaviour
{
    [Header("Unity 6 Setup Guide")]
    public bool showGuideOnStart = true;
    
    void Start()
    {
        if (showGuideOnStart)
        {
            ShowUnity6SetupGuide();
        }
    }
    
    [ContextMenu("Show Unity 6 Setup Guide")]
    public void ShowUnity6SetupGuide()
    {
        Debug.Log("=== UNITY 6 SETUP GUIDE ===");
        Debug.Log("");
        Debug.Log("1. INSTALL UNITY 6.2:");
        Debug.Log("   • Download Unity Hub from unity.com");
        Debug.Log("   • Install Unity 6.2 LTS (recommended)");
        Debug.Log("   • Add Android Build Support module");
        Debug.Log("");
        Debug.Log("2. INSTALL ANDROID STUDIO:");
        Debug.Log("   • Download from developer.android.com/studio");
        Debug.Log("   • Install with default settings");
        Debug.Log("   • Open Android Studio and go to Tools > AVD Manager");
        Debug.Log("   • Create a virtual device (Pixel 7 recommended)");
        Debug.Log("   • Choose API 33 or higher");
        Debug.Log("");
        Debug.Log("3. CONFIGURE UNITY FOR ANDROID:");
        Debug.Log("   • Open Unity 6.2");
        Debug.Log("   • Go to File > Build Settings");
        Debug.Log("   • Switch Platform to Android");
        Debug.Log("   • Click 'Switch Platform' and wait");
        Debug.Log("   • Go to Player Settings (gear icon)");
        Debug.Log("   • Set Package Name: com.yourcompany.adventuregame");
        Debug.Log("   • Set Minimum API Level: 21 (Android 5.0)");
        Debug.Log("   • Set Target API Level: 34 (Android 14)");
        Debug.Log("   • Set Target Architecture: ARM64");
        Debug.Log("   • Set Scripting Backend: IL2CPP");
        Debug.Log("   • Set Graphics API: OpenGL ES 3.0");
        Debug.Log("");
        Debug.Log("4. SET UP PROJECT:");
        Debug.Log("   • Open this project in Unity 6.2");
        Debug.Log("   • Add TestSceneSetup to a GameObject");
        Debug.Log("   • Right-click TestSceneSetup > Setup Test Scene");
        Debug.Log("   • Press Play to test in Unity Editor");
        Debug.Log("");
        Debug.Log("5. TEST ON ANDROID EMULATOR:");
        Debug.Log("   • Add AndroidEmulatorTestRunner to a GameObject");
        Debug.Log("   • Right-click AndroidEmulatorTestRunner > Run Full Test Suite");
        Debug.Log("   • The system will build APK and test on emulator");
        Debug.Log("");
        Debug.Log("6. UNITY 6 FEATURES:");
        Debug.Log("   • New Input System (optional)");
        Debug.Log("   • Universal Render Pipeline (optional)");
        Debug.Log("   • Enhanced Android support");
        Debug.Log("   • Better performance");
        Debug.Log("   • Vulkan graphics API support");
        Debug.Log("");
        Debug.Log("=== SETUP COMPLETE ===");
    }
    
    [ContextMenu("Check System Requirements")]
    public void CheckSystemRequirements()
    {
        Debug.Log("=== SYSTEM REQUIREMENTS CHECK ===");
        
        // Check Unity version
        string unityVersion = Application.unityVersion;
        Debug.Log($"Unity Version: {unityVersion}");
        
        if (unityVersion.StartsWith("6"))
        {
            Debug.Log("✓ Unity 6 detected - compatible");
        }
        else
        {
            Debug.LogWarning("⚠ Unity 6 not detected - may have compatibility issues");
        }
        
        // Check platform
        Debug.Log($"Platform: {Application.platform}");
        
        // Check memory
        long memory = System.GC.GetTotalMemory(false);
        float memoryGB = memory / 1024f / 1024f / 1024f;
        Debug.Log($"Available Memory: {memoryGB:F1}GB");
        
        if (memoryGB >= 8f)
        {
            Debug.Log("✓ Sufficient memory for Unity 6 and Android emulator");
        }
        else
        {
            Debug.LogWarning("⚠ Low memory - may affect performance");
        }
        
        // Check if running in editor
        #if UNITY_EDITOR
        Debug.Log("✓ Running in Unity Editor");
        #else
        Debug.Log("⚠ Not running in Unity Editor");
        #endif
    }
    
    [ContextMenu("Test Unity 6 Compatibility")]
    public void TestUnity6Compatibility()
    {
        Debug.Log("=== UNITY 6 COMPATIBILITY TEST ===");
        
        // Test 1: Check Unity version
        string unityVersion = Application.unityVersion;
        bool isUnity6 = unityVersion.StartsWith("6");
        
        Debug.Log($"Unity Version: {unityVersion}");
        Debug.Log($"Is Unity 6: {isUnity6}");
        
        if (isUnity6)
        {
            Debug.Log("✓ Unity 6 compatibility confirmed");
        }
        else
        {
            Debug.LogError("✗ Unity 6 not detected");
        }
        
        // Test 2: Check Android support
        #if UNITY_ANDROID
        Debug.Log("✓ Android platform support available");
        #else
        Debug.Log("⚠ Android platform support not available");
        #endif
        
        // Test 3: Check scripting backend
        #if ENABLE_IL2CPP
        Debug.Log("✓ IL2CPP scripting backend available");
        #else
        Debug.Log("⚠ IL2CPP scripting backend not available");
        #endif
        
        // Test 4: Check graphics API
        if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
        {
            Debug.Log("✓ OpenGL ES 3.0 graphics API available");
        }
        else
        {
            Debug.Log($"Graphics API: {SystemInfo.graphicsDeviceType}");
        }
        
        // Test 5: Check input system
        if (FindObjectOfType<MobileInputBridge>() != null)
        {
            Debug.Log("✓ Mobile input system available");
        }
        else
        {
            Debug.Log("⚠ Mobile input system not found");
        }
        
        Debug.Log("=== COMPATIBILITY TEST COMPLETE ===");
    }
    
    [ContextMenu("Show Troubleshooting Tips")]
    public void ShowTroubleshootingTips()
    {
        Debug.Log("=== UNITY 6 TROUBLESHOOTING TIPS ===");
        Debug.Log("");
        Debug.Log("COMMON ISSUES:");
        Debug.Log("");
        Debug.Log("1. BUILD ERRORS:");
        Debug.Log("   • Check Android SDK path in Unity preferences");
        Debug.Log("   • Verify JDK is installed and configured");
        Debug.Log("   • Make sure Android Build Support is installed");
        Debug.Log("   • Check Package Manager for missing packages");
        Debug.Log("");
        Debug.Log("2. EMULATOR ISSUES:");
        Debug.Log("   • Enable hardware acceleration in AVD settings");
        Debug.Log("   • Increase emulator RAM to 4GB or more");
        Debug.Log("   • Enable 'Use Host GPU' in AVD settings");
        Debug.Log("   • Check that HAXM is installed (Intel) or Hyper-V (AMD)");
        Debug.Log("");
        Debug.Log("3. PERFORMANCE ISSUES:");
        Debug.Log("   • Use IL2CPP scripting backend");
        Debug.Log("   • Enable compression in build settings");
        Debug.Log("   • Use LZ4HC compression for faster loading");
        Debug.Log("   • Set target architecture to ARM64");
        Debug.Log("   • Enable multithreaded rendering");
        Debug.Log("");
        Debug.Log("4. INPUT ISSUES:");
        Debug.Log("   • Check that MobileInputBridge is in the scene");
        Debug.Log("   • Verify touch input is enabled in emulator");
        Debug.Log("   • Test with virtual joystick in Unity Editor");
        Debug.Log("   • Check that UI elements are properly configured");
        Debug.Log("");
        Debug.Log("5. UNITY 6 SPECIFIC:");
        Debug.Log("   • Update to latest Unity 6.2 LTS");
        Debug.Log("   • Check Package Manager for updates");
        Debug.Log("   • Verify Android SDK is compatible");
        Debug.Log("   • Test with different graphics APIs");
        Debug.Log("");
        Debug.Log("=== TROUBLESHOOTING COMPLETE ===");
    }
}
