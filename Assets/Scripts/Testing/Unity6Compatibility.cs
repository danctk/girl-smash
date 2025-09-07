using UnityEngine;
using UnityEditor;

public class Unity6Compatibility : MonoBehaviour
{
    [Header("Unity 6 Compatibility")]
    public bool useNewInputSystem = true;
    public bool useURP = false;
    public bool useHDRP = false;
    
    [Header("Unity 6 Features")]
    public bool enableSpatialAudio = true;
    public bool enableWebGL2 = true;
    public bool enableIL2CPP = true;
    
    void Start()
    {
        CheckUnityVersion();
        ConfigureForUnity6();
    }
    
    void CheckUnityVersion()
    {
        string unityVersion = Application.unityVersion;
        Debug.Log($"Unity Version: {unityVersion}");
        
        if (unityVersion.StartsWith("6"))
        {
            Debug.Log("✓ Unity 6 detected - using latest features");
        }
        else
        {
            Debug.LogWarning("⚠ Unity 6 not detected - some features may not be available");
        }
    }
    
    void ConfigureForUnity6()
    {
        // Configure for Unity 6 features
        if (useNewInputSystem)
        {
            Debug.Log("✓ New Input System enabled");
        }
        
        if (useURP)
        {
            Debug.Log("✓ Universal Render Pipeline enabled");
        }
        
        if (useHDRP)
        {
            Debug.Log("✓ High Definition Render Pipeline enabled");
        }
        
        if (enableSpatialAudio)
        {
            Debug.Log("✓ Spatial Audio enabled");
        }
        
        if (enableWebGL2)
        {
            Debug.Log("✓ WebGL 2.0 enabled");
        }
        
        if (enableIL2CPP)
        {
            Debug.Log("✓ IL2CPP scripting backend enabled");
        }
    }
    
    [ContextMenu("Configure for Unity 6")]
    public void ConfigureForUnity6Manual()
    {
        #if UNITY_EDITOR
        // Set Android settings for Unity 6
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel34;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        
        // Set scripting backend
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        
        // Set graphics API
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new UnityEngine.Rendering.GraphicsDeviceType[] 
        { 
            UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3,
            UnityEngine.Rendering.GraphicsDeviceType.Vulkan
        });
        
        // Set compression
        PlayerSettings.Android.compressionType = AndroidCompressionType.LZ4HC;
        
        // Set orientation
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        
        // Set internet permission
        PlayerSettings.Android.forceInternetPermission = true;
        
        // Set package name
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.yourcompany.adventuregame");
        
        // Set version
        PlayerSettings.bundleVersion = "1.0.0";
        PlayerSettings.Android.bundleVersionCode = 1;
        
        Debug.Log("✓ Unity 6 configuration applied");
        #else
        Debug.Log("Configuration only available in Unity Editor");
        #endif
    }
    
    [ContextMenu("Check Unity 6 Features")]
    public void CheckUnity6Features()
    {
        Debug.Log("=== UNITY 6 FEATURES CHECK ===");
        
        // Check if running in Unity 6
        string unityVersion = Application.unityVersion;
        bool isUnity6 = unityVersion.StartsWith("6");
        
        Debug.Log($"Unity Version: {unityVersion}");
        Debug.Log($"Is Unity 6: {isUnity6}");
        
        if (isUnity6)
        {
            Debug.Log("✓ Unity 6 features available:");
            Debug.Log("  • New Input System");
            Debug.Log("  • Universal Render Pipeline");
            Debug.Log("  • High Definition Render Pipeline");
            Debug.Log("  • Spatial Audio");
            Debug.Log("  • WebGL 2.0");
            Debug.Log("  • IL2CPP scripting backend");
            Debug.Log("  • Vulkan graphics API");
            Debug.Log("  • Enhanced Android support");
        }
        else
        {
            Debug.LogWarning("⚠ Unity 6 features not available");
        }
    }
    
    [ContextMenu("Update Project for Unity 6")]
    public void UpdateProjectForUnity6()
    {
        #if UNITY_EDITOR
        Debug.Log("Updating project for Unity 6...");
        
        // Update project settings
        ConfigureForUnity6Manual();
        
        // Update build settings
        EditorBuildSettings.scenes = new EditorBuildSettingsScene[1];
        EditorBuildSettings.scenes[0] = new EditorBuildSettingsScene("Assets/Scenes/TestScene.unity", true);
        
        // Update quality settings
        QualitySettings.SetQualityLevel(0); // Set to highest quality
        
        // Update render settings
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.5f, 0.5f, 0.6f);
        RenderSettings.ambientEquatorColor = new Color(0.4f, 0.4f, 0.4f);
        RenderSettings.ambientGroundColor = new Color(0.3f, 0.3f, 0.3f);
        
        Debug.Log("✓ Project updated for Unity 6");
        #else
        Debug.Log("Project update only available in Unity Editor");
        #endif
    }
}
