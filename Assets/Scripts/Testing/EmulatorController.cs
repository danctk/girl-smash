using UnityEngine;
using System.Diagnostics;
using System.IO;

public class EmulatorController : MonoBehaviour
{
    [Header("Emulator Settings")]
    public string emulatorName = "Pixel_7_API_33";
    public string androidSdkPath = "C:\\Users\\%USERNAME%\\AppData\\Local\\Android\\Sdk";
    public int emulatorPort = 5554;
    
    [Header("ADB Settings")]
    public string adbPath = "platform-tools\\adb.exe";
    public bool autoConnect = true;
    
    private string fullAdbPath;
    private bool isEmulatorRunning = false;
    
    void Start()
    {
        SetupPaths();
        if (autoConnect)
        {
            CheckEmulatorStatus();
        }
    }
    
    void SetupPaths()
    {
        // Expand environment variables
        androidSdkPath = System.Environment.ExpandEnvironmentVariables(androidSdkPath);
        fullAdbPath = Path.Combine(androidSdkPath, adbPath);
        
        Debug.Log($"Android SDK Path: {androidSdkPath}");
        Debug.Log($"ADB Path: {fullAdbPath}");
    }
    
    [ContextMenu("Start Emulator")]
    public void StartEmulator()
    {
        if (isEmulatorRunning)
        {
            Debug.Log("Emulator is already running");
            return;
        }
        
        string emulatorPath = Path.Combine(androidSdkPath, "emulator", "emulator.exe");
        string avdPath = Path.Combine(androidSdkPath, "avd");
        
        if (!File.Exists(emulatorPath))
        {
            Debug.LogError($"Emulator not found at: {emulatorPath}");
            return;
        }
        
        string command = $"-avd {emulatorName} -port {emulatorPort}";
        
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = emulatorPath,
            Arguments = command,
            UseShellExecute = false,
            CreateNoWindow = false
        };
        
        Process emulatorProcess = Process.Start(startInfo);
        Debug.Log($"Starting emulator: {emulatorName} on port {emulatorPort}");
        
        // Wait a bit for emulator to start
        System.Threading.Thread.Sleep(5000);
        CheckEmulatorStatus();
    }
    
    [ContextMenu("Stop Emulator")]
    public void StopEmulator()
    {
        if (!isEmulatorRunning)
        {
            Debug.Log("No emulator is running");
            return;
        }
        
        string command = $"-s emulator-{emulatorPort} emu kill";
        RunADBCommand(command);
        
        Debug.Log($"Stopping emulator on port {emulatorPort}");
        isEmulatorRunning = false;
    }
    
    [ContextMenu("Check Emulator Status")]
    public void CheckEmulatorStatus()
    {
        string command = "devices";
        string output = RunADBCommand(command);
        
        isEmulatorRunning = output.Contains($"emulator-{emulatorPort}");
        
        if (isEmulatorRunning)
        {
            Debug.Log($"Emulator is running on port {emulatorPort}");
        }
        else
        {
            Debug.Log("No emulator is running");
        }
    }
    
    [ContextMenu("Install APK to Emulator")]
    public void InstallAPKToEmulator()
    {
        if (!isEmulatorRunning)
        {
            Debug.LogError("No emulator is running. Start emulator first.");
            return;
        }
        
        string apkPath = "Builds/Android/AdventureGame.apk";
        if (!File.Exists(apkPath))
        {
            Debug.LogError($"APK not found at: {apkPath}");
            return;
        }
        
        string command = $"-s emulator-{emulatorPort} install -r \"{apkPath}\"";
        string output = RunADBCommand(command);
        
        if (output.Contains("Success"))
        {
            Debug.Log("APK installed successfully to emulator");
        }
        else
        {
            Debug.LogError($"Failed to install APK: {output}");
        }
    }
    
    [ContextMenu("Launch App on Emulator")]
    public void LaunchAppOnEmulator()
    {
        if (!isEmulatorRunning)
        {
            Debug.LogError("No emulator is running. Start emulator first.");
            return;
        }
        
        string packageName = "com.yourcompany.adventuregame";
        string activityName = "com.unity3d.player.UnityPlayerActivity";
        string command = $"-s emulator-{emulatorPort} shell am start -n {packageName}/{activityName}";
        
        string output = RunADBCommand(command);
        Debug.Log($"Launching app on emulator: {output}");
    }
    
    [ContextMenu("Take Screenshot")]
    public void TakeScreenshot()
    {
        if (!isEmulatorRunning)
        {
            Debug.LogError("No emulator is running. Start emulator first.");
            return;
        }
        
        string screenshotPath = $"screenshot_{System.DateTime.Now:yyyyMMdd_HHmmss}.png";
        string command = $"-s emulator-{emulatorPort} shell screencap -p /sdcard/{screenshotPath}";
        RunADBCommand(command);
        
        // Pull screenshot to local machine
        string pullCommand = $"-s emulator-{emulatorPort} pull /sdcard/{screenshotPath} Screenshots/";
        RunADBCommand(pullCommand);
        
        Debug.Log($"Screenshot saved as: {screenshotPath}");
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
    
    public bool IsEmulatorRunning()
    {
        return isEmulatorRunning;
    }
}
