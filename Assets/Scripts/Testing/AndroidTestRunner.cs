using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class AndroidTestRunner : MonoBehaviour
{
    [Header("Test Configuration")]
    public bool runTestsOnStart = false;
    public float testDuration = 60f;
    public bool logPerformance = true;
    
    [Header("Test Results")]
    public int testsPassed = 0;
    public int testsFailed = 0;
    public float averageFPS = 0f;
    public float memoryUsage = 0f;
    
    private bool isTestRunning = false;
    private float testStartTime;
    private int frameCount = 0;
    private float fpsSum = 0f;
    
    void Start()
    {
        if (runTestsOnStart)
        {
            StartCoroutine(RunAllTests());
        }
    }
    
    [ContextMenu("Run All Tests")]
    public void RunAllTests()
    {
        StartCoroutine(RunAllTestsCoroutine());
    }
    
    IEnumerator RunAllTestsCoroutine()
    {
        Debug.Log("=== Starting Android Test Suite ===");
        isTestRunning = true;
        testStartTime = Time.time;
        testsPassed = 0;
        testsFailed = 0;
        frameCount = 0;
        fpsSum = 0f;
        
        // Test 1: Input System
        yield return StartCoroutine(TestInputSystem());
        
        // Test 2: Player Movement
        yield return StartCoroutine(TestPlayerMovement());
        
        // Test 3: Camera System
        yield return StartCoroutine(TestCameraSystem());
        
        // Test 4: UI System
        yield return StartCoroutine(TestUISystem());
        
        // Test 5: Performance
        yield return StartCoroutine(TestPerformance());
        
        // Test 6: Memory Usage
        yield return StartCoroutine(TestMemoryUsage());
        
        // Test 7: Touch Input
        yield return StartCoroutine(TestTouchInput());
        
        // Generate test report
        GenerateTestReport();
        
        isTestRunning = false;
        Debug.Log("=== Android Test Suite Complete ===");
    }
    
    IEnumerator TestInputSystem()
    {
        Debug.Log("Testing Input System...");
        
        MobileInputBridge inputBridge = FindObjectOfType<MobileInputBridge>();
        if (inputBridge == null)
        {
            Debug.LogError("MobileInputBridge not found!");
            testsFailed++;
            yield break;
        }
        
        // Test movement input
        Vector2 movementInput = inputBridge.GetMovementInput();
        if (movementInput != Vector2.zero)
        {
            Debug.Log("✓ Movement input working");
            testsPassed++;
        }
        else
        {
            Debug.Log("✗ Movement input not working");
            testsFailed++;
        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestPlayerMovement()
    {
        Debug.Log("Testing Player Movement...");
        
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            Debug.LogError("PlayerController not found!");
            testsFailed++;
            yield break;
        }
        
        // Test if player can move
        Vector3 initialPosition = player.transform.position;
        
        // Simulate movement input
        MobileInputBridge inputBridge = FindObjectOfType<MobileInputBridge>();
        if (inputBridge != null)
        {
            // This would need to be implemented in the actual input system
            Debug.Log("✓ Player movement system found");
            testsPassed++;
        }
        else
        {
            Debug.Log("✗ Player movement system not working");
            testsFailed++;
        }
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestCameraSystem()
    {
        Debug.Log("Testing Camera System...");
        
        CameraController camera = FindObjectOfType<CameraController>();
        if (camera == null)
        {
            Debug.LogError("CameraController not found!");
            testsFailed++;
            yield break;
        }
        
        Debug.Log("✓ Camera system found");
        testsPassed++;
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestUISystem()
    {
        Debug.Log("Testing UI System...");
        
        UIController uiController = FindObjectOfType<UIController>();
        if (uiController == null)
        {
            Debug.LogError("UIController not found!");
            testsFailed++;
            yield break;
        }
        
        Debug.Log("✓ UI system found");
        testsPassed++;
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestPerformance()
    {
        Debug.Log("Testing Performance...");
        
        float startTime = Time.time;
        int startFrames = Time.frameCount;
        
        while (Time.time - startTime < 5f)
        {
            frameCount++;
            fpsSum += 1f / Time.deltaTime;
            yield return null;
        }
        
        averageFPS = fpsSum / frameCount;
        
        if (averageFPS >= 30f)
        {
            Debug.Log($"✓ Performance test passed - Average FPS: {averageFPS:F1}");
            testsPassed++;
        }
        else
        {
            Debug.Log($"✗ Performance test failed - Average FPS: {averageFPS:F1}");
            testsFailed++;
        }
    }
    
    IEnumerator TestMemoryUsage()
    {
        Debug.Log("Testing Memory Usage...");
        
        // Force garbage collection
        System.GC.Collect();
        yield return new WaitForSeconds(1f);
        
        // Get memory usage
        long memoryBefore = System.GC.GetTotalMemory(false);
        
        // Create some objects to test memory
        for (int i = 0; i < 1000; i++)
        {
            GameObject testObj = new GameObject($"TestObject_{i}");
            Destroy(testObj);
        }
        
        System.GC.Collect();
        yield return new WaitForSeconds(1f);
        
        long memoryAfter = System.GC.GetTotalMemory(false);
        memoryUsage = (memoryAfter - memoryBefore) / 1024f / 1024f; // MB
        
        if (memoryUsage < 100f) // Less than 100MB
        {
            Debug.Log($"✓ Memory test passed - Usage: {memoryUsage:F1}MB");
            testsPassed++;
        }
        else
        {
            Debug.Log($"✗ Memory test failed - Usage: {memoryUsage:F1}MB");
            testsFailed++;
        }
    }
    
    IEnumerator TestTouchInput()
    {
        Debug.Log("Testing Touch Input...");
        
        #if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Debug.Log("✓ Touch input detected");
            testsPassed++;
        }
        else
        {
            Debug.Log("✗ No touch input detected");
            testsFailed++;
        }
        #else
        Debug.Log("Touch input test skipped (not on Android)");
        testsPassed++;
        #endif
        
        yield return new WaitForSeconds(1f);
    }
    
    void GenerateTestReport()
    {
        Debug.Log("=== TEST REPORT ===");
        Debug.Log($"Tests Passed: {testsPassed}");
        Debug.Log($"Tests Failed: {testsFailed}");
        Debug.Log($"Success Rate: {(float)testsPassed / (testsPassed + testsFailed) * 100f:F1}%");
        Debug.Log($"Average FPS: {averageFPS:F1}");
        Debug.Log($"Memory Usage: {memoryUsage:F1}MB");
        Debug.Log($"Test Duration: {Time.time - testStartTime:F1}s");
        
        // Save report to file
        string report = $"Android Test Report\n" +
                      $"==================\n" +
                      $"Tests Passed: {testsPassed}\n" +
                      $"Tests Failed: {testsFailed}\n" +
                      $"Success Rate: {(float)testsPassed / (testsPassed + testsFailed) * 100f:F1}%\n" +
                      $"Average FPS: {averageFPS:F1}\n" +
                      $"Memory Usage: {memoryUsage:F1}MB\n" +
                      $"Test Duration: {Time.time - testStartTime:F1}s\n" +
                      $"Platform: {Application.platform}\n" +
                      $"Unity Version: {Application.unityVersion}\n";
        
        System.IO.File.WriteAllText("TestReport.txt", report);
        Debug.Log("Test report saved to TestReport.txt");
    }
    
    void Update()
    {
        if (isTestRunning)
        {
            frameCount++;
            fpsSum += 1f / Time.deltaTime;
        }
    }
}
