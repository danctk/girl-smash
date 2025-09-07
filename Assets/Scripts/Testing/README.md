# Android Testing Suite

This testing suite provides comprehensive tools for testing your 3D adventure game on Android emulators and devices.

## Components

### Core Testing Scripts

1. **AndroidTestManager** - Main test coordinator
2. **AndroidTestRunner** - Individual test execution
3. **MobileInputTester** - Input system testing
4. **AndroidBuildHelper** - Build automation
5. **EmulatorController** - Emulator management
6. **AndroidEmulatorSetup** - Automated setup
7. **AndroidEmulatorGuide** - Setup instructions

### Input Testing

1. **MobileInputBridge** - Input system bridge
2. **UnityEditorMobileInput** - Editor input simulation

## Quick Start

### 1. Setup Android Emulator

1. Install Android Studio
2. Create a virtual device (AVD)
3. Start the emulator
4. Configure Unity for Android

### 2. Add Testing Components

1. Add `AndroidTestManager` to a GameObject in your scene
2. The manager will automatically find other testing components
3. Use the context menu to run tests

### 3. Run Tests

- **Right-click on AndroidTestManager** → "Run Test Suite"
- **Right-click on AndroidTestRunner** → "Run All Tests"
- **Right-click on EmulatorController** → "Start Emulator"

## Test Types

### Automated Tests

- **Emulator Status** - Checks if emulator is running
- **Input System** - Tests mobile input functionality
- **Performance** - Measures FPS and performance
- **Memory Usage** - Monitors memory consumption
- **Touch Input** - Validates touch input on Android

### Manual Tests

- **Visual Testing** - Use MobileInputTester for visual feedback
- **Input Simulation** - Test touch controls in Unity Editor
- **Build Testing** - Automated APK building and installation

## Configuration

### Emulator Settings

```csharp
public string emulatorName = "Pixel_7_API_33";
public int emulatorPort = 5554;
public string androidSdkPath = "C:\\Users\\%USERNAME%\\AppData\\Local\\Android\\Sdk";
```

### Build Settings

```csharp
public string apkName = "AdventureGame.apk";
public string packageName = "com.yourcompany.adventuregame";
public bool developmentBuild = true;
```

## Usage Examples

### Running Tests Programmatically

```csharp
// Get test manager
AndroidTestManager testManager = FindObjectOfType<AndroidTestManager>();

// Run test suite
testManager.RunTestSuite();

// Start continuous testing
testManager.StartContinuousTesting();
```

### Testing Input System

```csharp
// Get input bridge
MobileInputBridge inputBridge = FindObjectOfType<MobileInputBridge>();

// Check input values
Vector2 movement = inputBridge.GetMovementInput();
bool jumpPressed = inputBridge.IsJumpPressed();
```

### Emulator Management

```csharp
// Get emulator controller
EmulatorController emulator = FindObjectOfType<EmulatorController>();

// Start emulator
emulator.StartEmulator();

// Install APK
emulator.InstallAPKToEmulator();

// Launch app
emulator.LaunchAppOnEmulator();
```

## Troubleshooting

### Common Issues

1. **Emulator not starting**
   - Check Android SDK installation
   - Verify AVD configuration
   - Enable hardware acceleration

2. **APK installation fails**
   - Check emulator is running
   - Verify package name
   - Check ADB connection

3. **Touch input not working**
   - Enable touch in emulator settings
   - Check input system configuration
   - Verify mobile input components

4. **Performance issues**
   - Increase emulator RAM
   - Enable hardware acceleration
   - Use ARM64 architecture

### Debug Commands

```bash
# Check emulator status
adb devices

# Install APK
adb install -r AdventureGame.apk

# Launch app
adb shell am start -n com.yourcompany.adventuregame/com.unity3d.player.UnityPlayerActivity

# Take screenshot
adb shell screencap -p /sdcard/screenshot.png
adb pull /sdcard/screenshot.png
```

## Test Reports

Test results are automatically saved to:
- `TestReport.txt` - Basic test results
- `TestReport_YYYYMMDD_HHMMSS.txt` - Detailed timestamped reports

## Performance Monitoring

The testing suite monitors:
- **FPS** - Frames per second
- **Memory Usage** - RAM consumption
- **Input Latency** - Touch response time
- **Load Times** - Scene loading performance

## Continuous Testing

Enable continuous testing for automated monitoring:
1. Set `continuousTesting = true` in AndroidTestManager
2. Set `testInterval` for test frequency
3. Tests will run automatically in the background

## Best Practices

1. **Test on multiple devices** - Use different emulator configurations
2. **Monitor performance** - Watch for memory leaks and FPS drops
3. **Test input thoroughly** - Verify all touch controls work
4. **Automate builds** - Use build scripts for consistent testing
5. **Document issues** - Keep track of bugs and fixes

## Integration with CI/CD

The testing suite can be integrated with continuous integration:
- Use `AndroidBuildHelper` for automated builds
- Use `AndroidTestRunner` for automated testing
- Use `EmulatorController` for emulator management
- Generate reports for build pipelines
