using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class AndroidEmulatorTester : MonoBehaviour
{
    [Header("Emulator Configuration")]
    public string emulatorName = "Pixel_7_API_33";
    public string androidSdkPath = "C:\\Users\\%USERNAME%\\AppData\\Local\\Android\\Sdk";
    public int emulatorPort = 5554;
    
    [Header("Test Settings")]
    public bool autoStartEmulator = true;
    public bool autoInstallAPK = true;
    public bool autoLaunchApp = true;
    public float testDuration = 60f;
    
    [Header("Test Results")]
    public bool emulatorRunning = false;
    public bool apkInstalled = false;
    public bool appLaunched = false;
    public float testStartTime;
    
    private Process emulatorProcess;
    private string fullAdbPath;
    
    void Start()
    {
        SetupPaths();
        
        if (autoStartEmulator)
        {
            StartCoroutine(RunFullTest());
        }
    }
    
    void SetupPaths()
    {
        androidSdkPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        fullAdbPath = Path.Combine(androidSdkPath, "platform-tools", "adb.exe");
        
        Debug.Log($"Android SDK Path: {androidSdkPath}");
        Debug.Log($"ADB Path: {fullAdbPath}");
    }
    
    [ContextMenu("Run Full Android Test")]
    public void RunFullTest()
    {
        StartCoroutine(RunFullTestCoroutine());
    }
    
    IEnumerator RunFullTestCoroutine()
    {
        Debug.Log("=== STARTING ANDROID EMULATOR TEST ===");
        testStartTime = Time.time;
        
        // Step 1: Check if emulator is already running
        yield return StartCoroutine(CheckEmulatorStatus());
        
        if (!emulatorRunning)
        {
            // Step 2: Start emulator
            yield return StartCoroutine(StartEmulator());
        }
        
        // Step 3: Wait for emulator to boot
        yield return StartCoroutine(WaitForEmulatorBoot());
        
        // Step 4: Install APK
        if (autoInstallAPK)
        {
            yield return StartCoroutine(InstallAPK());
        }
        
        // Step 5: Launch app
        if (autoLaunchApp)
        {
            yield return StartCoroutine(LaunchApp());
        }
        
        // Step 6: Run game tests
        yield return StartCoroutine(RunGameTests());
        
        // Step 7: Generate test report
        GenerateTestReport();
        
        Debug.Log("=== ANDROID EMULATOR TEST COMPLETE ===");
    }
    
    IEnumerator CheckEmulatorStatus()
    {
        Debug.Log("Checking emulator status...");
        
        string command = "devices";
        string output = RunADBCommand(command);
        
        emulatorRunning = output.Contains($"emulator-{emulatorPort}");
        
        if (emulatorRunning)
        {
            Debug.Log("✓ Emulator is already running");
        }
        else
        {
            Debug.Log("✗ Emulator is not running");
        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator StartEmulator()
    {
        Debug.Log("Starting Android emulator...");
        
        string emulatorPath = Path.Combine(androidSdkPath, "emulator", "emulator.exe");
        
        if (!File.Exists(emulatorPath))
        {
            Debug.LogError($"Emulator not found at: {emulatorPath}");
            yield break;
        }
        
        string arguments = $"-avd {emulatorName} -port {emulatorPort} -no-audio -no-window";
        
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = emulatorPath,
            Arguments = arguments,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        emulatorProcess = Process.Start(startInfo);
        Debug.Log($"Emulator started with PID: {emulatorProcess.Id}");
        
        yield return new WaitForSeconds(5f);
    }
    
    IEnumerator WaitForEmulatorBoot()
    {
        Debug.Log("Waiting for emulator to boot...");
        
        float timeout = 120f; // 2 minutes timeout
        float elapsed = 0f;
        
        while (elapsed < timeout)
        {
            string command = "devices";
            string output = RunADBCommand(command);
            
            if (output.Contains($"emulator-{emulatorPort}") && output.Contains("device"))
            {
                Debug.Log("✓ Emulator is ready!");
                emulatorRunning = true;
                yield break;
            }
            
            elapsed += 2f;
            yield return new WaitForSeconds(2f);
            
            Debug.Log($"Waiting for emulator... ({elapsed:F0}s/{timeout:F0}s)");
        }
        
        Debug.LogError("Emulator failed to boot within timeout period");
    }
    
    IEnumerator InstallAPK()
    {
        Debug.Log("Installing APK to emulator...");
        
        string apkPath = "Builds/Android/AdventureGame.apk";
        
        if (!File.Exists(apkPath))
        {
            Debug.LogError($"APK not found at: {apkPath}");
            yield break;
        }
        
        string command = $"-s emulator-{emulatorPort} install -r \"{apkPath}\"";
        string output = RunADBCommand(command);
        
        if (output.Contains("Success"))
        {
            Debug.Log("✓ APK installed successfully");
            apkInstalled = true;
        }
        else
        {
            Debug.LogError($"✗ Failed to install APK: {output}");
        }
        
        yield return new WaitForSeconds(2f);
    }
    
    IEnumerator LaunchApp()
    {
        Debug.Log("Launching app on emulator...");
        
        string packageName = "com.yourcompany.adventuregame";
        string activityName = "com.unity3d.player.UnityPlayerActivity";
        string command = $"-s emulator-{emulatorPort} shell am start -n {packageName}/{activityName}";
        
        string output = RunADBCommand(command);
        
        if (output.Contains("Starting") || output.Contains("Activity"))
        {
            Debug.Log("✓ App launched successfully");
            appLaunched = true;
        }
        else
        {
            Debug.LogError($"✗ Failed to launch app: {output}");
        }
        
        yield return new WaitForSeconds(3f);
    }
    
    IEnumerator RunGameTests()
    {
        Debug.Log("Running game tests...");
        
        // Test 1: Check if app is running
        yield return StartCoroutine(TestAppRunning());
        
        // Test 2: Test touch input
        yield return StartCoroutine(TestTouchInput());
        
        // Test 3: Test performance
        yield return StartCoroutine(TestPerformance());
        
        // Test 4: Take screenshot
        yield return StartCoroutine(TakeScreenshot());
        
        // Test 5: Test app stability
        yield return StartCoroutine(TestAppStability());
    }
    
    IEnumerator TestAppRunning()
    {
        Debug.Log("Testing if app is running...");
        
        string command = $"-s emulator-{emulatorPort} shell ps | grep {packageName}";
        string output = RunADBCommand(command);
        
        if (output.Contains(packageName))
        {
            Debug.Log("✓ App is running");
        }
        else
        {
            Debug.Log("✗ App is not running");
        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestTouchInput()
    {
        Debug.Log("Testing touch input...");
        
        // Simulate touch input
        string command = $"-s emulator-{emulatorPort} shell input tap 500 500";
        RunADBCommand(command);
        
        Debug.Log("✓ Touch input simulated");
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestPerformance()
    {
        Debug.Log("Testing performance...");
        
        // Get CPU usage
        string command = $"-s emulator-{emulatorPort} shell dumpsys cpuinfo | grep {packageName}";
        string output = RunADBCommand(command);
        
        Debug.Log($"CPU usage: {output}");
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TakeScreenshot()
    {
        Debug.Log("Taking screenshot...");
        
        string screenshotName = $"test_screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string command = $"-s emulator-{emulatorPort} shell screencap -p /sdcard/{screenshotName}";
        RunADBCommand(command);
        
        // Pull screenshot
        string pullCommand = $"-s emulator-{emulatorPort} pull /sdcard/{screenshotName} Screenshots/";
        RunADBCommand(pullCommand);
        
        Debug.Log($"✓ Screenshot saved: {screenshotName}");
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestAppStability()
    {
        Debug.Log("Testing app stability...");
        
        // Test for 30 seconds
        float testTime = 30f;
        float elapsed = 0f;
        
        while (elapsed < testTime)
        {
            // Check if app is still running
            string command = $"-s emulator-{emulatorPort} shell ps | grep {packageName}";
            string output = RunADBCommand(command);
            
            if (!output.Contains(packageName))
            {
                Debug.LogError("✗ App crashed during stability test");
                yield break;
            }
            
            elapsed += 5f;
            yield return new WaitForSeconds(5f);
        }
        
        Debug.Log("✓ App stability test passed");
    }
    
    void GenerateTestReport()
    {
        float testDuration = Time.time - testStartTime;
        
        Debug.Log("=== ANDROID EMULATOR TEST REPORT ===");
        Debug.Log($"Test Duration: {testDuration:F1} seconds");
        Debug.Log($"Emulator Running: {(emulatorRunning ? "✓" : "✗")}");
        Debug.Log($"APK Installed: {(apkInstalled ? "✓" : "✗")}");
        Debug.Log($"App Launched: {(appLaunched ? "✓" : "✗")}");
        Debug.Log($"Platform: {Application.platform}");
        Debug.Log($"Unity Version: {Application.unityVersion}");
        
        // Save report to file
        string report = $"Android Emulator Test Report\n";
        report += $"============================\n\n";
        report += $"Test Duration: {testDuration:F1} seconds\n";
        report += $"Emulator Running: {(emulatorRunning ? "PASS" : "FAIL")}\n";
        report += $"APK Installed: {(apkInstalled ? "PASS" : "FAIL")}\n";
        report += $"App Launched: {(appLaunched ? "PASS" : "FAIL")}\n";
        report += $"Platform: {Application.platform}\n";
        report += $"Unity Version: {Application.unityVersion}\n";
        report += $"Timestamp: {System.DateTime.Now}\n";
        
        System.IO.File.WriteAllText($"AndroidEmulatorTest_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt", report);
        Debug.Log("Test report saved to file");
    }
    
    string RunADBCommand(string arguments)
    {
        if (!File.Exists(fullAdbPath))
        {
            Debug.LogError($"ADB not found at: {fullAdbPath}");
            return "";
        }
        
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = fullAdbPath,
            Arguments = arguments,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };
        
        Process process = Process.Start(startInfo);
        string output = process.StandardOutput.ReadToEnd();
        string error = process.StandardError.ReadToEnd();
        process.WaitForExit();
        
        if (!string.IsNullOrEmpty(error))
        {
            Debug.LogWarning($"ADB Warning: {error}");
        }
        
        return output;
    }
    
    [ContextMenu("Stop Emulator")]
    public void StopEmulator()
    {
        if (emulatorProcess != null && !emulatorProcess.HasExited)
        {
            emulatorProcess.Kill();
            Debug.Log("Emulator stopped");
        }
        
        string command = $"-s emulator-{emulatorPort} emu kill";
        RunADBCommand(command);
    }
    
    void OnDestroy()
    {
        if (emulatorProcess != null && !emulatorProcess.HasExited)
        {
            emulatorProcess.Kill();
        }
    }
}
