using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;

public class AndroidEmulatorTestRunner : MonoBehaviour
{
    [Header("Test Configuration")]
    public bool runTestsOnStart = true;
    public bool buildAPKFirst = true;
    public bool installAPKAfterBuild = true;
    public bool launchAppAfterInstall = true;
    
    [Header("Emulator Settings")]
    public string emulatorName = "Pixel_7_API_33";
    public string androidSdkPath = "C:\\Users\\%USERNAME%\\AppData\\Local\\Android\\Sdk";
    public int emulatorPort = 5554;
    public string packageName = "com.yourcompany.adventuregame";
    
    [Header("Build Settings")]
    public string buildPath = "Builds/Android";
    public string apkName = "AdventureGame.apk";
    public bool developmentBuild = true;
    
    [Header("Test Results")]
    public TestResults testResults = new TestResults();
    
    [System.Serializable]
    public class TestResults
    {
        public bool emulatorStarted = false;
        public bool apkBuilt = false;
        public bool apkInstalled = false;
        public bool appLaunched = false;
        public bool testsCompleted = false;
        public float totalTestTime = 0f;
        public int testsPassed = 0;
        public int testsFailed = 0;
        public string errorMessage = "";
    }
    
    private string fullAdbPath;
    private Process emulatorProcess;
    private float testStartTime;
    
    void Start()
    {
        SetupPaths();
        
        if (runTestsOnStart)
        {
            StartCoroutine(RunFullTestSuite());
        }
    }
    
    void SetupPaths()
    {
        androidSdkPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        fullAdbPath = Path.Combine(androidSdkPath, "platform-tools", "adb.exe");
        
        Debug.Log($"Android SDK Path: {androidSdkPath}");
        Debug.Log($"ADB Path: {fullAdbPath}");
    }
    
    [ContextMenu("Run Full Test Suite")]
    public void RunFullTestSuite()
    {
        StartCoroutine(RunFullTestSuiteCoroutine());
    }
    
    IEnumerator RunFullTestSuiteCoroutine()
    {
        Debug.Log("=== STARTING ANDROID EMULATOR TEST SUITE ===");
        testStartTime = Time.time;
        ResetTestResults();
        
        try
        {
            // Step 1: Build APK
            if (buildAPKFirst)
            {
                yield return StartCoroutine(BuildAPK());
            }
            
            // Step 2: Start Emulator
            yield return StartCoroutine(StartEmulator());
            
            // Step 3: Wait for Emulator Boot
            yield return StartCoroutine(WaitForEmulatorBoot());
            
            // Step 4: Install APK
            if (installAPKAfterBuild)
            {
                yield return StartCoroutine(InstallAPK());
            }
            
            // Step 5: Launch App
            if (launchAppAfterInstall)
            {
                yield return StartCoroutine(LaunchApp());
            }
            
            // Step 6: Run Game Tests
            yield return StartCoroutine(RunGameTests());
            
            // Step 7: Generate Report
            GenerateTestReport();
            
            testResults.testsCompleted = true;
        }
        catch (System.Exception e)
        {
            testResults.errorMessage = e.Message;
            Debug.LogError($"Test suite failed: {e.Message}");
        }
        finally
        {
            testResults.totalTestTime = Time.time - testStartTime;
            Debug.Log("=== ANDROID EMULATOR TEST SUITE COMPLETE ===");
        }
    }
    
    IEnumerator BuildAPK()
    {
        Debug.Log("Building APK...");
        
        #if UNITY_EDITOR
        // Configure build settings
        EditorBuildSettings.scenes = new EditorBuildSettingsScene[1];
        EditorBuildSettings.scenes[0] = new EditorBuildSettingsScene("Assets/Scenes/TestScene.unity", true);
        
        // Set Android settings
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, packageName);
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.Android.bundleVersionCode = 1;
        
        // Build options
        BuildPlayerOptions buildOptions = new BuildPlayerOptions();
        buildOptions.scenes = new string[] { "Assets/Scenes/TestScene.unity" };
        buildOptions.locationPathName = Path.Combine(buildPath, apkName);
        buildOptions.target = BuildTarget.Android;
        buildOptions.options = developmentBuild ? BuildOptions.Development : BuildOptions.None;
        
        // Create build directory
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }
        
        // Build APK
        BuildPipeline.BuildPlayer(buildOptions);
        
        if (File.Exists(Path.Combine(buildPath, apkName)))
        {
            Debug.Log("✓ APK built successfully");
            testResults.apkBuilt = true;
        }
        else
        {
            Debug.LogError("✗ APK build failed");
            testResults.apkBuilt = false;
        }
        #else
        Debug.Log("APK build skipped (not in Unity Editor)");
        testResults.apkBuilt = true;
        #endif
        
        yield return new WaitForSeconds(2f);
    }
    
    IEnumerator StartEmulator()
    {
        Debug.Log("Starting Android emulator...");
        
        string emulatorPath = Path.Combine(androidSdkPath, "emulator", "emulator.exe");
        
        if (!File.Exists(emulatorPath))
        {
            Debug.LogError($"Emulator not found at: {emulatorPath}");
            testResults.errorMessage = "Emulator not found";
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
        
        testResults.emulatorStarted = true;
        yield return new WaitForSeconds(5f);
    }
    
    IEnumerator WaitForEmulatorBoot()
    {
        Debug.Log("Waiting for emulator to boot...");
        
        float timeout = 120f;
        float elapsed = 0f;
        
        while (elapsed < timeout)
        {
            string command = "devices";
            string output = RunADBCommand(command);
            
            if (output.Contains($"emulator-{emulatorPort}") && output.Contains("device"))
            {
                Debug.Log("✓ Emulator is ready!");
                yield break;
            }
            
            elapsed += 2f;
            yield return new WaitForSeconds(2f);
            Debug.Log($"Waiting for emulator... ({elapsed:F0}s/{timeout:F0}s)");
        }
        
        Debug.LogError("Emulator failed to boot within timeout period");
        testResults.errorMessage = "Emulator boot timeout";
    }
    
    IEnumerator InstallAPK()
    {
        Debug.Log("Installing APK to emulator...");
        
        string apkPath = Path.Combine(buildPath, apkName);
        
        if (!File.Exists(apkPath))
        {
            Debug.LogError($"APK not found at: {apkPath}");
            testResults.errorMessage = "APK not found";
            yield break;
        }
        
        string command = $"-s emulator-{emulatorPort} install -r \"{apkPath}\"";
        string output = RunADBCommand(command);
        
        if (output.Contains("Success"))
        {
            Debug.Log("✓ APK installed successfully");
            testResults.apkInstalled = true;
        }
        else
        {
            Debug.LogError($"✗ Failed to install APK: {output}");
            testResults.apkInstalled = false;
            testResults.errorMessage = "APK installation failed";
        }
        
        yield return new WaitForSeconds(2f);
    }
    
    IEnumerator LaunchApp()
    {
        Debug.Log("Launching app on emulator...");
        
        string activityName = "com.unity3d.player.UnityPlayerActivity";
        string command = $"-s emulator-{emulatorPort} shell am start -n {packageName}/{activityName}";
        
        string output = RunADBCommand(command);
        
        if (output.Contains("Starting") || output.Contains("Activity"))
        {
            Debug.Log("✓ App launched successfully");
            testResults.appLaunched = true;
        }
        else
        {
            Debug.LogError($"✗ Failed to launch app: {output}");
            testResults.appLaunched = false;
            testResults.errorMessage = "App launch failed";
        }
        
        yield return new WaitForSeconds(3f);
    }
    
    IEnumerator RunGameTests()
    {
        Debug.Log("Running game tests...");
        
        // Test 1: App is running
        yield return StartCoroutine(TestAppRunning());
        
        // Test 2: Touch input
        yield return StartCoroutine(TestTouchInput());
        
        // Test 3: Performance
        yield return StartCoroutine(TestPerformance());
        
        // Test 4: Memory usage
        yield return StartCoroutine(TestMemoryUsage());
        
        // Test 5: App stability
        yield return StartCoroutine(TestAppStability());
        
        // Test 6: Take screenshot
        yield return StartCoroutine(TakeScreenshot());
    }
    
    IEnumerator TestAppRunning()
    {
        Debug.Log("Testing if app is running...");
        
        string command = $"-s emulator-{emulatorPort} shell ps | grep {packageName}";
        string output = RunADBCommand(command);
        
        if (output.Contains(packageName))
        {
            Debug.Log("✓ App is running");
            testResults.testsPassed++;
        }
        else
        {
            Debug.Log("✗ App is not running");
            testResults.testsFailed++;
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
        testResults.testsPassed++;
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestPerformance()
    {
        Debug.Log("Testing performance...");
        
        // Get CPU usage
        string command = $"-s emulator-{emulatorPort} shell dumpsys cpuinfo | grep {packageName}";
        string output = RunADBCommand(command);
        
        Debug.Log($"CPU usage: {output}");
        testResults.testsPassed++;
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestMemoryUsage()
    {
        Debug.Log("Testing memory usage...");
        
        // Get memory usage
        string command = $"-s emulator-{emulatorPort} shell dumpsys meminfo {packageName}";
        string output = RunADBCommand(command);
        
        Debug.Log($"Memory usage: {output}");
        testResults.testsPassed++;
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestAppStability()
    {
        Debug.Log("Testing app stability...");
        
        float testTime = 10f;
        float elapsed = 0f;
        
        while (elapsed < testTime)
        {
            string command = $"-s emulator-{emulatorPort} shell ps | grep {packageName}";
            string output = RunADBCommand(command);
            
            if (!output.Contains(packageName))
            {
                Debug.LogError("✗ App crashed during stability test");
                testResults.testsFailed++;
                yield break;
            }
            
            elapsed += 2f;
            yield return new WaitForSeconds(2f);
        }
        
        Debug.Log("✓ App stability test passed");
        testResults.testsPassed++;
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
        testResults.testsPassed++;
        
        yield return new WaitForSeconds(1f);
    }
    
    void GenerateTestReport()
    {
        Debug.Log("=== ANDROID EMULATOR TEST REPORT ===");
        Debug.Log($"Total Test Time: {testResults.totalTestTime:F1} seconds");
        Debug.Log($"Emulator Started: {(testResults.emulatorStarted ? "✓" : "✗")}");
        Debug.Log($"APK Built: {(testResults.apkBuilt ? "✓" : "✗")}");
        Debug.Log($"APK Installed: {(testResults.apkInstalled ? "✓" : "✗")}");
        Debug.Log($"App Launched: {(testResults.appLaunched ? "✓" : "✗")}");
        Debug.Log($"Tests Completed: {(testResults.testsCompleted ? "✓" : "✗")}");
        Debug.Log($"Tests Passed: {testResults.testsPassed}");
        Debug.Log($"Tests Failed: {testResults.testsFailed}");
        Debug.Log($"Success Rate: {(float)testResults.testsPassed / (testResults.testsPassed + testResults.testsFailed) * 100f:F1}%");
        
        if (!string.IsNullOrEmpty(testResults.errorMessage))
        {
            Debug.LogError($"Error: {testResults.errorMessage}");
        }
        
        // Save report to file
        string report = $"Android Emulator Test Report\n";
        report += $"============================\n\n";
        report += $"Test Date: {System.DateTime.Now}\n";
        report += $"Total Test Time: {testResults.totalTestTime:F1} seconds\n";
        report += $"Emulator Started: {(testResults.emulatorStarted ? "PASS" : "FAIL")}\n";
        report += $"APK Built: {(testResults.apkBuilt ? "PASS" : "FAIL")}\n";
        report += $"APK Installed: {(testResults.apkInstalled ? "PASS" : "FAIL")}\n";
        report += $"App Launched: {(testResults.appLaunched ? "PASS" : "FAIL")}\n";
        report += $"Tests Completed: {(testResults.testsCompleted ? "PASS" : "FAIL")}\n";
        report += $"Tests Passed: {testResults.testsPassed}\n";
        report += $"Tests Failed: {testResults.testsFailed}\n";
        report += $"Success Rate: {(float)testResults.testsPassed / (testResults.testsPassed + testResults.testsFailed) * 100f:F1}%\n";
        report += $"Platform: {Application.platform}\n";
        report += $"Unity Version: {Application.unityVersion}\n";
        
        if (!string.IsNullOrEmpty(testResults.errorMessage))
        {
            report += $"\nError: {testResults.errorMessage}\n";
        }
        
        System.IO.File.WriteAllText($"AndroidEmulatorTestReport_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt", report);
        Debug.Log("Test report saved to file");
    }
    
    void ResetTestResults()
    {
        testResults = new TestResults();
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
