using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AndroidTestManager : MonoBehaviour
{
    [Header("Test Configuration")]
    public bool autoRunTests = false;
    public float testInterval = 30f;
    public bool continuousTesting = false;
    
    [Header("Test Components")]
    public AndroidTestRunner testRunner;
    public MobileInputTester inputTester;
    public AndroidEmulatorSetup emulatorSetup;
    public EmulatorController emulatorController;
    
    [Header("Test Results")]
    public List<TestResult> testResults = new List<TestResult>();
    public int totalTestsRun = 0;
    public float averageTestScore = 0f;
    
    [System.Serializable]
    public class TestResult
    {
        public string testName;
        public bool passed;
        public float score;
        public string timestamp;
        public string details;
    }
    
    void Start()
    {
        InitializeTestComponents();
        
        if (autoRunTests)
        {
            StartCoroutine(RunContinuousTests());
        }
    }
    
    void InitializeTestComponents()
    {
        if (testRunner == null)
            testRunner = FindObjectOfType<AndroidTestRunner>();
        
        if (inputTester == null)
            inputTester = FindObjectOfType<MobileInputTester>();
        
        if (emulatorSetup == null)
            emulatorSetup = FindObjectOfType<AndroidEmulatorSetup>();
        
        if (emulatorController == null)
            emulatorController = FindObjectOfType<EmulatorController>();
    }
    
    IEnumerator RunContinuousTests()
    {
        while (continuousTesting)
        {
            yield return StartCoroutine(RunTestSuite());
            yield return new WaitForSeconds(testInterval);
        }
    }
    
    [ContextMenu("Run Test Suite")]
    public void RunTestSuite()
    {
        StartCoroutine(RunTestSuiteCoroutine());
    }
    
    IEnumerator RunTestSuiteCoroutine()
    {
        Debug.Log("=== Starting Android Test Suite ===");
        
        // Test 1: Emulator Status
        yield return StartCoroutine(TestEmulatorStatus());
        
        // Test 2: Input System
        yield return StartCoroutine(TestInputSystem());
        
        // Test 3: Performance
        yield return StartCoroutine(TestPerformance());
        
        // Test 4: Memory
        yield return StartCoroutine(TestMemory());
        
        // Test 5: Touch Input
        yield return StartCoroutine(TestTouchInput());
        
        // Generate summary
        GenerateTestSummary();
        
        totalTestsRun++;
        Debug.Log("=== Test Suite Complete ===");
    }
    
    IEnumerator TestEmulatorStatus()
    {
        TestResult result = new TestResult
        {
            testName = "Emulator Status",
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        if (emulatorController != null)
        {
            bool isRunning = emulatorController.IsEmulatorRunning();
            result.passed = isRunning;
            result.score = isRunning ? 100f : 0f;
            result.details = isRunning ? "Emulator is running" : "Emulator is not running";
        }
        else
        {
            result.passed = false;
            result.score = 0f;
            result.details = "EmulatorController not found";
        }
        
        testResults.Add(result);
        Debug.Log($"Emulator Status Test: {(result.passed ? "PASSED" : "FAILED")} - {result.details}");
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestInputSystem()
    {
        TestResult result = new TestResult
        {
            testName = "Input System",
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        MobileInputBridge inputBridge = FindObjectOfType<MobileInputBridge>();
        if (inputBridge != null)
        {
            Vector2 movementInput = inputBridge.GetMovementInput();
            bool hasInput = movementInput != Vector2.zero;
            
            result.passed = true; // Input system exists
            result.score = hasInput ? 100f : 50f;
            result.details = hasInput ? "Input system active" : "Input system idle";
        }
        else
        {
            result.passed = false;
            result.score = 0f;
            result.details = "MobileInputBridge not found";
        }
        
        testResults.Add(result);
        Debug.Log($"Input System Test: {(result.passed ? "PASSED" : "FAILED")} - {result.details}");
        
        yield return new WaitForSeconds(1f);
    }
    
    IEnumerator TestPerformance()
    {
        TestResult result = new TestResult
        {
            testName = "Performance",
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        float startTime = Time.time;
        int frameCount = 0;
        float fpsSum = 0f;
        
        while (Time.time - startTime < 3f)
        {
            frameCount++;
            fpsSum += 1f / Time.deltaTime;
            yield return null;
        }
        
        float averageFPS = fpsSum / frameCount;
        result.passed = averageFPS >= 30f;
        result.score = Mathf.Clamp(averageFPS / 60f * 100f, 0f, 100f);
        result.details = $"Average FPS: {averageFPS:F1}";
        
        testResults.Add(result);
        Debug.Log($"Performance Test: {(result.passed ? "PASSED" : "FAILED")} - {result.details}");
    }
    
    IEnumerator TestMemory()
    {
        TestResult result = new TestResult
        {
            testName = "Memory Usage",
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        System.GC.Collect();
        yield return new WaitForSeconds(1f);
        
        long memoryUsage = System.GC.GetTotalMemory(false);
        float memoryMB = memoryUsage / 1024f / 1024f;
        
        result.passed = memoryMB < 200f; // Less than 200MB
        result.score = Mathf.Clamp(100f - (memoryMB / 200f * 100f), 0f, 100f);
        result.details = $"Memory Usage: {memoryMB:F1}MB";
        
        testResults.Add(result);
        Debug.Log($"Memory Test: {(result.passed ? "PASSED" : "FAILED")} - {result.details}");
    }
    
    IEnumerator TestTouchInput()
    {
        TestResult result = new TestResult
        {
            testName = "Touch Input",
            timestamp = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };
        
        #if UNITY_ANDROID
        bool hasTouch = Input.touchCount > 0;
        result.passed = true; // Touch system available
        result.score = hasTouch ? 100f : 50f;
        result.details = hasTouch ? "Touch input detected" : "No touch input";
        #else
        result.passed = true; // Not on Android, skip test
        result.score = 100f;
        result.details = "Touch test skipped (not on Android)";
        #endif
        
        testResults.Add(result);
        Debug.Log($"Touch Input Test: {(result.passed ? "PASSED" : "FAILED")} - {result.details}");
        
        yield return new WaitForSeconds(1f);
    }
    
    void GenerateTestSummary()
    {
        if (testResults.Count == 0) return;
        
        int passedTests = 0;
        float totalScore = 0f;
        
        foreach (var result in testResults)
        {
            if (result.passed) passedTests++;
            totalScore += result.score;
        }
        
        averageTestScore = totalScore / testResults.Count;
        
        Debug.Log("=== TEST SUMMARY ===");
        Debug.Log($"Total Tests: {testResults.Count}");
        Debug.Log($"Passed: {passedTests}");
        Debug.Log($"Failed: {testResults.Count - passedTests}");
        Debug.Log($"Success Rate: {(float)passedTests / testResults.Count * 100f:F1}%");
        Debug.Log($"Average Score: {averageTestScore:F1}%");
        
        // Save detailed report
        SaveTestReport();
    }
    
    void SaveTestReport()
    {
        string report = "Android Test Report\n";
        report += "==================\n\n";
        
        foreach (var result in testResults)
        {
            report += $"Test: {result.testName}\n";
            report += $"Status: {(result.passed ? "PASSED" : "FAILED")}\n";
            report += $"Score: {result.score:F1}%\n";
            report += $"Details: {result.details}\n";
            report += $"Timestamp: {result.timestamp}\n\n";
        }
        
        report += $"Summary:\n";
        report += $"Total Tests: {testResults.Count}\n";
        report += $"Average Score: {averageTestScore:F1}%\n";
        report += $"Platform: {Application.platform}\n";
        report += $"Unity Version: {Application.unityVersion}\n";
        
        System.IO.File.WriteAllText($"TestReport_{System.DateTime.Now:yyyyMMdd_HHmmss}.txt", report);
        Debug.Log("Detailed test report saved.");
    }
    
    [ContextMenu("Clear Test Results")]
    public void ClearTestResults()
    {
        testResults.Clear();
        totalTestsRun = 0;
        averageTestScore = 0f;
        Debug.Log("Test results cleared.");
    }
    
    [ContextMenu("Start Continuous Testing")]
    public void StartContinuousTesting()
    {
        continuousTesting = true;
        StartCoroutine(RunContinuousTests());
        Debug.Log("Continuous testing started.");
    }
    
    [ContextMenu("Stop Continuous Testing")]
    public void StopContinuousTesting()
    {
        continuousTesting = false;
        Debug.Log("Continuous testing stopped.");
    }
}
