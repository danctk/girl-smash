# 3D Adventure Game for Android - Unity Boilerplate

A complete boilerplate for creating a 3D adventure game for Android using Unity and C#.

## Features

### Core Systems
- **Player Controller**: 3D movement with mobile touch controls
- **Camera System**: Third-person camera with touch controls
- **Mobile Input**: Virtual joystick and touch buttons
- **Game Management**: Pause, restart, scene management
- **UI System**: Mobile-optimized interface

### Adventure Game Mechanics
- **Inventory System**: Item collection and management
- **Dialogue System**: NPC conversations with text display
- **Quest System**: Objective tracking and completion
- **Interaction System**: Interactable objects and NPCs
- **Collectibles**: Items to gather throughout the game

### Mobile Optimizations
- Touch controls for movement and interaction
- Mobile UI layout and scaling
- Android build configuration
- Performance optimizations

## Project Structure

```
Assets/
├── Scripts/
│   ├── Player/
│   │   └── PlayerController.cs
│   ├── Camera/
│   │   └── CameraController.cs
│   ├── Input/
│   │   └── MobileInputManager.cs
│   ├── Game/
│   │   └── GameManager.cs
│   ├── Adventure/
│   │   ├── InventorySystem.cs
│   │   ├── DialogueSystem.cs
│   │   ├── QuestSystem.cs
│   │   ├── Interactable.cs
│   │   ├── Collectible.cs
│   │   ├── Door.cs
│   │   └── NPC.cs
│   ├── UI/
│   │   └── UIController.cs
│   └── Utilities/
│       ├── SceneLoader.cs
│       ├── AudioManager.cs
│       └── Joystick.cs
└── Scenes/
    ├── MainMenu.unity
    └── GameScene.unity
```

## Setup Instructions

1. **Open in Unity**: Import this project into Unity 2022.3.25f1 or later
2. **Android Setup**: 
   - Go to File > Build Settings
   - Switch Platform to Android
   - Configure Player Settings for Android
3. **Scene Setup**: Create the MainMenu and GameScene scenes
4. **Prefab Creation**: Create prefabs for:
   - Player with PlayerController
   - Camera with CameraController
   - UI Canvas with mobile controls
   - Interactable objects (doors, NPCs, collectibles)

## Usage

### Player Movement
- Use virtual joystick for movement
- Tap to jump
- Touch and drag to look around

### Interaction
- Approach interactable objects
- Tap interact button or press E key
- Follow dialogue prompts

### Inventory
- Tap inventory button to open/close
- Items are automatically added when collected
- Use items by tapping them

## Customization

### Adding New Items
1. Create new InventoryItem instances
2. Set item properties (ID, name, type, etc.)
3. Add to collectible objects

### Creating Quests
1. Define Quest objects with objectives
2. Use QuestSystem to manage quest states
3. Connect to NPCs and interactables

### Mobile Controls
1. Modify MobileInputManager for custom controls
2. Adjust Joystick settings for sensitivity
3. Add new UI buttons as needed

## Build Settings

- **Platform**: Android
- **API Level**: 21+ (Android 5.0+)
- **Target Architecture**: ARM64
- **Graphics API**: OpenGL ES 3.0
- **Scripting Backend**: IL2CPP

## Dependencies

- Unity 2022.3.25f1 or later
- Android SDK
- TextMeshPro (for UI text)
- Input System (for enhanced input handling)

## License

This boilerplate is provided as-is for educational and commercial use.
