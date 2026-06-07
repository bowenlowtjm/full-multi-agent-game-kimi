# Arcade Gesture Game - Unity Project

A fast-paced mobile arcade game built in Unity 6 LTS. Players interact with targets using specific gestures: Tap, Double Tap, Long Press, and Drag.

## Target Types

| Target | Visual | Gesture | Points |
|--------|--------|---------|--------|
| Pop | Blue Sphere | Single Tap | +10 |
| Heavy | Gold Square | Double Tap | +25 |
| Charge | Green Anchor | Long Press (1.5s) | +50 |
| Trash | Red Trash Can | Drag to Bin | +40 |

## Requirements

- Unity 6 LTS (6000.0.x) or Unity 2022.3 LTS
- Unity Modules:
  - Android Build Support
  - Input System
  - TextMeshPro

## Setup Instructions

### 1. Clone/Download

```bash
git clone <repo_url> arcade-gesture-game
cd arcade-gesture-game
```

### 2. Open in Unity

1. Launch Unity Hub
2. Click "Open"
3. Select the project folder
4. Unity will automatically download packages

### 3. Configure Build Settings

1. File → Build Settings
2. Select "Android" platform
3. Click "Switch Platform"
4. Set:
   - Minimum API Level: 24 (Android 7.0)
   - Target API Level: 34+
   - Orientation: Portrait

### 4. Scene Setup

Create the following scenes in `Assets/_Game/Scenes/`:

#### MainMenu Scene
1. Create empty scene
2. Add GameObject: `GameStateManager` (add GameStateManager.cs)
3. Add GameObject: `SceneLoader` (add SceneLoader.cs)
4. Add Canvas with:
   - Title Text: "ARCADE GESTURE"
   - Start Button → MainMenuManager
   - High Score Text
5. Add MainMenuManager script to a GameObject

#### Gameplay Scene
1. Create empty scene
2. Add Camera (orthographic, size 5)
3. Add GameObject: `GameStateManager`
4. Add GameObject: `SceneLoader`
5. Add GameObject: `ScoreManager`
6. Add GameObject: `InputManager` (set Target Layer to "Target")
7. Add GameObject: `SpawnerManager` (assign Target Prefab)
8. Add GameObject: `EffectsManager`
9. Add Canvas with:
   - Score Text (TMP)
   - Combo Text (TMP)
   - Lives Container (for heart icons)
   - Pause Button
   - Pause Panel (hidden)
   - Trash Bin Zone (at bottom of screen)
10. Add EventSystem

#### GameOver Scene
1. Create empty scene
2. Add GameObject: `GameStateManager`
3. Add Canvas with:
   - Final Score Text
   - High Score Text
   - Retry Button
   - Menu Button
4. Add GameOverManager script

### 5. Target Prefab Setup

1. Create empty GameObject named "Target"
2. Add SpriteRenderer component
3. Add CircleCollider2D (or BoxCollider2D for square targets)
4. Add Rigidbody2D (gravity scale = 0, body type = Kinematic)
5. Add Target.cs script
6. Create prefab in `Assets/_Game/Prefabs/`
7. Assign to SpawnerManager.targetPrefab

### 6. Sprites

Create or import sprites for:
- Pop (blue circle)
- Heavy (gold square)
- Charge (green anchor)
- Trash (red trash can)
- LifeIcon (heart)

Place in `Assets/_Game/Sprites/`

### 7. Build

1. File → Build Settings
2. Click "Build"
3. Output: `Builds/Android/arcade-gesture.apk`

## Project Structure

```
Assets/_Game/
├── Scripts/           # Core game systems
│   ├── GameStateManager.cs
│   ├── ScoreManager.cs
│   ├── SpawnerManager.cs
│   ├── InputManager.cs
│   ├── Target.cs
│   ├── TargetDefinition.cs
│   ├── EffectsManager.cs
│   ├── HUDManager.cs
│   ├── MainMenuManager.cs
│   ├── GameOverManager.cs
│   ├── SceneLoader.cs
│   └── TrashBinZone.cs
├── Prefabs/           # Game prefabs
│   └── Target.prefab
├── Sprites/           # Game assets
├── Scenes/            # Game scenes
│   ├── MainMenu.unity
│   ├── Gameplay.unity
│   └── GameOver.unity
└── ScriptableObjects/ # Data assets

Assets/Tests/
├── EditMode/          # Unit tests
└── PlayMode/          # Integration tests
```

## Controls

- **Tap**: Single touch/release on Pop targets
- **Double Tap**: Two quick taps on Heavy targets
- **Long Press**: Hold for 1.5 seconds on Charge targets
- **Drag**: Touch and drag Trash targets to the Trash Bin Zone

## Game Loop

1. Targets spawn based on difficulty curve
2. Player performs correct gestures
3. Correct = points + combo multiplier (every 5 hits)
4. Wrong/Miss = combo breaks + lose life
5. 3 lives, then Game Over

## QA Verification (PRD Section 7)

- [x] Verification 01: Tapping a Charge target breaks combo
- [x] Verification 02: Dragging Trash outside bin penalizes
- [ ] Verification 03: 60 FPS with 10+ targets
- [x] Verification 04: High score persists (PlayerPrefs)

## License

MIT
