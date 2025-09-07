using UnityEngine;
using UnityEditor;
using System.IO;

public class AndroidBuildHelper : MonoBehaviour
{
    [Header("Build Settings")]
    public string buildPath = "Builds/Android";
    public string apkName = "AdventureGame.apk";
    public bool developmentBuild = true;
    public bool allowDebugging = true;
    
    [Header("Android Settings")]
    public AndroidSdkVersions minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
    public AndroidSdkVersions targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;
    public AndroidArchitecture targetArchitecture = AndroidArchitecture.ARM64;
    
    [ContextMenu("Build for Android")]
    public void BuildForAndroid()
    {
        #if UNITY_EDITOR
        BuildAndroidAPK();
        #else
        Debug.Log("This script only works in Unity Editor");
        #endif
    }
    
    #if UNITY_EDITOR
    void BuildAndroidAPK()
    {
        // Configure Android settings
        ConfigureAndroidSettings();
        
        // Set build options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetScenePaths();
        buildPlayerOptions.locationPathName = Path.Combine(buildPath, apkName);
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = GetBuildOptions();
        
        // Create build directory
        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
        }
        
        // Build the APK
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        Debug.Log($"Android APK built successfully at: {Path.Combine(buildPath, apkName)}");
    }
    
    void ConfigureAndroidSettings()
    {
        // Set Android SDK versions
        PlayerSettings.Android.minSdkVersion = minSdkVersion;
        PlayerSettings.Android.targetSdkVersion = targetSdkVersion;
        
        // Set target architecture
        PlayerSettings.Android.targetArchitectures = targetArchitecture;
        
        // Set package name
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.yourcompany.adventuregame");
        
        // Set version
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.Android.bundleVersionCode = 1;
        
        // Set orientation
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        
        // Set graphics API
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[] 
        { 
            UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 
        });
        
        // Set scripting backend
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        
        // Set compression method
        PlayerSettings.Android.compressionType = AndroidCompressionType.LZ4HC;
        
        // Set internet permission
        PlayerSettings.Android.forceInternetPermission = true;
        
        Debug.Log("Android settings configured successfully");
    }
    
    string[] GetScenePaths()
    {
        string[] scenes = new string[EditorBuildSettings.scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorBuildSettings.scenes[i].path;
        }
        return scenes;
    }
    
    BuildOptions GetBuildOptions()
    {
        BuildOptions options = BuildOptions.None;
        
        if (developmentBuild)
            options |= BuildOptions.Development;
        
        if (allowDebugging)
            options |= BuildOptions.AllowDebugging;
        
        return options;
    }
    #endif
    
    [ContextMenu("Install APK to Device")]
    public void InstallAPKToDevice()
    {
        #if UNITY_EDITOR
        string apkPath = Path.Combine(buildPath, apkName);
        if (File.Exists(apkPath))
        {
            string adbCommand = $"adb install -r \"{apkPath}\"";
            System.Diagnostics.Process.Start("cmd", $"/c {adbCommand}");
            Debug.Log($"Installing APK: {adbCommand}");
        }
        else
        {
            Debug.LogError($"APK not found at: {apkPath}");
        }
        #endif
    }
    
    [ContextMenu("Launch App on Device")]
    public void LaunchAppOnDevice()
    {
        #if UNITY_EDITOR
        string packageName = "com.yourcompany.adventuregame";
        string activityName = "com.unity3d.player.UnityPlayerActivity";
        string adbCommand = $"adb shell am start -n {packageName}/{activityName}";
        System.Diagnostics.Process.Start("cmd", $"/c {adbCommand}");
        Debug.Log($"Launching app: {adbCommand}");
        #endif
    }
}
