using UnityEngine;
using System.Collections.Generic;

public class AndroidEmulatorGuide : MonoBehaviour
{
    [Header("Guide Information")]
    public bool showGuideOnStart = true;
    public List<string> setupSteps = new List<string>();
    public List<string> troubleshootingTips = new List<string>();
    
    void Start()
    {
        InitializeGuide();
        
        if (showGuideOnStart)
        {
            ShowSetupGuide();
        }
    }
    
    void InitializeGuide()
    {
        // Setup steps
        setupSteps.Add("1. Install Android Studio from https://developer.android.com/studio");
        setupSteps.Add("2. Open Android Studio and go to Tools > AVD Manager");
        setupSteps.Add("3. Click 'Create Virtual Device' and select a phone (e.g., Pixel 7)");
        setupSteps.Add("4. Choose a system image (API 33 or higher recommended)");
        setupSteps.Add("5. Configure the AVD settings and click 'Finish'");
        setupSteps.Add("6. Start the emulator from AVD Manager");
        setupSteps.Add("7. In Unity, go to File > Build Settings and switch to Android");
        setupSteps.Add("8. Configure Player Settings for Android");
        setupSteps.Add("9. Build and run the project on the emulator");
        
        // Troubleshooting tips
        troubleshootingTips.Add("• If emulator is slow, enable hardware acceleration in AVD settings");
        troubleshootingTips.Add("• Make sure HAXM is installed for Intel processors");
        troubleshootingTips.Add("• For AMD processors, use Hyper-V or enable Windows Hypervisor Platform");
        troubleshootingTips.Add("• Increase emulator RAM to 4GB or more for better performance");
        troubleshootingTips.Add("• Enable 'Use Host GPU' in AVD settings");
        troubleshootingTips.Add("• If touch input doesn't work, check emulator settings");
        troubleshootingTips.Add("• Make sure USB debugging is enabled in emulator");
        troubleshootingTips.Add("• Check that ADB is in your system PATH");
    }
    
    [ContextMenu("Show Setup Guide")]
    public void ShowSetupGuide()
    {
        Debug.Log("=== ANDROID EMULATOR SETUP GUIDE ===");
        Debug.Log("Follow these steps to set up Android emulator testing:");
        Debug.Log("");
        
        foreach (string step in setupSteps)
        {
            Debug.Log(step);
        }
        
        Debug.Log("");
        Debug.Log("=== TROUBLESHOOTING TIPS ===");
        foreach (string tip in troubleshootingTips)
        {
            Debug.Log(tip);
        }
        
        Debug.Log("");
        Debug.Log("=== QUICK START ===");
        Debug.Log("1. Add AndroidTestManager to a GameObject in your scene");
        Debug.Log("2. Use the context menu to run tests");
        Debug.Log("3. Check the console for test results");
        Debug.Log("4. View detailed reports in the project folder");
    }
    
    [ContextMenu("Check System Requirements")]
    public void CheckSystemRequirements()
    {
        Debug.Log("=== SYSTEM REQUIREMENTS CHECK ===");
        
        // Check Unity version
        Debug.Log($"Unity Version: {Application.unityVersion}");
        if (Application.unityVersion.StartsWith("2022.3"))
        {
            Debug.Log("✓ Unity version is compatible");
        }
        else
        {
            Debug.Log("⚠ Unity version may not be optimal (2022.3+ recommended)");
        }
        
        // Check platform
        Debug.Log($"Current Platform: {Application.platform}");
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            Debug.Log("✓ Windows platform detected");
        }
        else if (Application.platform == RuntimePlatform.OSXEditor)
        {
            Debug.Log("✓ macOS platform detected");
        }
        else
        {
            Debug.Log("⚠ Platform may not be optimal for Android development");
        }
        
        // Check memory
        long memory = System.GC.GetTotalMemory(false);
        float memoryGB = memory / 1024f / 1024f / 1024f;
        Debug.Log($"Available Memory: {memoryGB:F1}GB");
        if (memoryGB >= 8f)
        {
            Debug.Log("✓ Sufficient memory for emulator");
        }
        else
        {
            Debug.Log("⚠ Low memory - emulator may run slowly");
        }
    }
    
    [ContextMenu("Test Emulator Connection")]
    public void TestEmulatorConnection()
    {
        Debug.Log("=== EMULATOR CONNECTION TEST ===");
        
        // This would typically check ADB connection
        // For now, just show instructions
        Debug.Log("To test emulator connection:");
        Debug.Log("1. Open Command Prompt or Terminal");
        Debug.Log("2. Navigate to Android SDK platform-tools folder");
        Debug.Log("3. Run: adb devices");
        Debug.Log("4. You should see your emulator listed");
        Debug.Log("5. If not, restart the emulator and try again");
    }
    
    [ContextMenu("Show Performance Tips")]
    public void ShowPerformanceTips()
    {
        Debug.Log("=== PERFORMANCE OPTIMIZATION TIPS ===");
        Debug.Log("• Use IL2CPP scripting backend for better performance");
        Debug.Log("• Enable compression in build settings");
        Debug.Log("• Use LZ4HC compression for faster loading");
        Debug.Log("• Set target architecture to ARM64");
        Debug.Log("• Enable multithreaded rendering");
        Debug.Log("• Use texture compression (ASTC for Android)");
        Debug.Log("• Optimize mesh complexity and polygon count");
        Debug.Log("• Use object pooling for frequently spawned objects");
        Debug.Log("• Profile with Unity Profiler to identify bottlenecks");
        Debug.Log("• Test on actual devices for real-world performance");
    }
    
    [ContextMenu("Show Build Settings")]
    public void ShowBuildSettings()
    {
        Debug.Log("=== RECOMMENDED BUILD SETTINGS ===");
        Debug.Log("Platform: Android");
        Debug.Log("Target API Level: 33 (Android 13)");
        Debug.Log("Minimum API Level: 21 (Android 5.0)");
        Debug.Log("Target Architecture: ARM64");
        Debug.Log("Scripting Backend: IL2CPP");
        Debug.Log("Graphics API: OpenGL ES 3.0");
        Debug.Log("Compression: LZ4HC");
        Debug.Log("Orientation: Landscape");
        Debug.Log("Internet Permission: Required");
        Debug.Log("Write Permission: External (SDCard)");
    }
}
