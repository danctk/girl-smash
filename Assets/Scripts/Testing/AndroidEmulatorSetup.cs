using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

public class AndroidEmulatorSetup : MonoBehaviour
{
    [Header("Emulator Configuration")]
    public string emulatorName = "Pixel_7_API_33";
    public string androidSdkPath = "C:\\Users\\%USERNAME%\\AppData\\Local\\Android\\Sdk";
    public int emulatorPort = 5554;
    public string avdName = "Pixel_7_API_33";
    
    [Header("Build Settings")]
    public string apkPath = "Builds/Android/AdventureGame.apk";
    public string packageName = "com.yourcompany.adventuregame";
    
    [ContextMenu("Setup Android Emulator")]
    public void SetupAndroidEmulator()
    {
        #if UNITY_EDITOR
        Debug.Log("Setting up Android Emulator...");
        
        // Check if Android SDK is installed
        if (!CheckAndroidSDK())
        {
            Debug.LogError("Android SDK not found! Please install Android Studio and SDK.");
            return;
        }
        
        // Create AVD if it doesn't exist
        CreateAVD();
        
        // Start emulator
        StartEmulator();
        
        // Wait for emulator to boot
        EditorUtility.DisplayProgressBar("Android Emulator", "Waiting for emulator to boot...", 0.5f);
        System.Threading.Thread.Sleep(10000); // Wait 10 seconds
        EditorUtility.ClearProgressBar();
        
        // Install APK
        InstallAPK();
        
        Debug.Log("Android Emulator setup complete!");
        #else
        Debug.Log("This script only works in Unity Editor");
        #endif
    }
    
    #if UNITY_EDITOR
    bool CheckAndroidSDK()
    {
        string expandedPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        string emulatorPath = Path.Combine(expandedPath, "emulator", "emulator.exe");
        string adbPath = Path.Combine(expandedPath, "platform-tools", "adb.exe");
        
        bool emulatorExists = File.Exists(emulatorPath);
        bool adbExists = File.Exists(adbPath);
        
        Debug.Log($"Emulator exists: {emulatorExists} at {emulatorPath}");
        Debug.Log($"ADB exists: {adbExists} at {adbPath}");
        
        return emulatorExists && adbExists;
    }
    
    void CreateAVD()
    {
        string expandedPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        string avdManagerPath = Path.Combine(expandedPath, "cmdline-tools", "latest", "bin", "avdmanager.bat");
        
        if (!File.Exists(avdManagerPath))
        {
            Debug.LogWarning("AVD Manager not found. Please create AVD manually in Android Studio.");
            return;
        }
        
        // Check if AVD already exists
        string listCommand = "list avd";
        string output = RunCommand(avdManagerPath, listCommand);
        
        if (output.Contains(avdName))
        {
            Debug.Log($"AVD '{avdName}' already exists.");
            return;
        }
        
        // Create AVD
        Debug.Log($"Creating AVD: {avdName}");
        string createCommand = $"create avd -n {avdName} -k \"system-images;android-33;google_apis;x86_64\"";
        RunCommand(avdManagerPath, createCommand);
    }
    
    void StartEmulator()
    {
        string expandedPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        string emulatorPath = Path.Combine(expandedPath, "emulator", "emulator.exe");
        
        Debug.Log($"Starting emulator: {emulatorName}");
        
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = emulatorPath,
            Arguments = $"-avd {avdName} -port {emulatorPort}",
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        Process.Start(startInfo);
    }
    
    void InstallAPK()
    {
        if (!File.Exists(apkPath))
        {
            Debug.LogError($"APK not found at: {apkPath}. Please build the project first.");
            return;
        }
        
        string expandedPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        string adbPath = Path.Combine(expandedPath, "platform-tools", "adb.exe");
        
        Debug.Log("Installing APK to emulator...");
        string installCommand = $"-s emulator-{emulatorPort} install -r \"{apkPath}\"";
        string output = RunCommand(adbPath, installCommand);
        
        if (output.Contains("Success"))
        {
            Debug.Log("APK installed successfully!");
        }
        else
        {
            Debug.LogError($"Failed to install APK: {output}");
        }
    }
    
    string RunCommand(string command, string arguments)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = command,
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
            Debug.LogWarning($"Command warning: {error}");
        }
        
        return output;
    }
    #endif
    
    [ContextMenu("Launch App on Emulator")]
    public void LaunchAppOnEmulator()
    {
        #if UNITY_EDITOR
        string expandedPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        string adbPath = Path.Combine(expandedPath, "platform-tools", "adb.exe");
        
        string activityName = "com.unity3d.player.UnityPlayerActivity";
        string command = $"-s emulator-{emulatorPort} shell am start -n {packageName}/{activityName}";
        
        string output = RunCommand(adbPath, command);
        Debug.Log($"Launching app: {output}");
        #endif
    }
    
    [ContextMenu("Take Screenshot")]
    public void TakeScreenshot()
    {
        #if UNITY_EDITOR
        string expandedPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        string adbPath = Path.Combine(expandedPath, "platform-tools", "adb.exe");
        
        string screenshotName = $"screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string command = $"-s emulator-{emulatorPort} shell screencap -p /sdcard/{screenshotName}";
        RunCommand(adbPath, command);
        
        // Pull screenshot
        string pullCommand = $"-s emulator-{emulatorPort} pull /sdcard/{screenshotName} Screenshots/";
        RunCommand(adbPath, pullCommand);
        
        Debug.Log($"Screenshot saved: {screenshotName}");
        #endif
    }
}
