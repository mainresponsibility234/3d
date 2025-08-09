# 3D Endless Runner Setup Guide

This guide will help you set up the 3D endless runner game with neon effects in Unity.

## Prerequisites

1. Unity 2022.3 LTS or newer
2. Basic knowledge of Unity interface
3. 3D project template

## Project Setup

### 1. Create New Unity Project

1. Open Unity Hub
2. Click "New Project"
3. Select "3D" template
4. Name your project "3D Endless Runner"
5. Choose location and click "Create"

### 2. Import Scripts

1. Create a `Scripts` folder in your Assets
2. Copy all the provided C# scripts into the Scripts folder:
   - `PlayerController.cs`
   - `GameManager.cs`
   - `ObstacleGenerator.cs`
   - `CameraFollow.cs`
   - `NeonMaterial.cs`
   - `UIManager.cs`
   - `Obstacle.cs`
   - `Collectible.cs`
   - `NeonEnvironment.cs`

### 3. Scene Setup

#### Create Player
1. Create an empty GameObject named "Player"
2. Add a CharacterController component
3. Add a Capsule as child (this will be the player model)
4. Add the `PlayerController` script
5. Tag the Player as "Player"

#### Create Camera
1. Select the Main Camera
2. Add the `CameraFollow` script
3. Set the target to the Player
4. Adjust offset values (recommended: 0, 5, -10)

#### Create Game Manager
1. Create an empty GameObject named "GameManager"
2. Add the `GameManager` script
3. This will handle scoring and game state

#### Create Obstacle Generator
1. Create an empty GameObject named "ObstacleGenerator"
2. Add the `ObstacleGenerator` script
3. This will spawn obstacles and collectibles

#### Create Neon Environment
1. Create an empty GameObject named "NeonEnvironment"
2. Add the `NeonEnvironment` script
3. This will handle the neon lighting and atmosphere

### 4. UI Setup

#### Create Canvas
1. Right-click in Hierarchy → UI → Canvas
2. Set Canvas Scaler to "Scale With Screen Size"
3. Reference resolution: 1920x1080

#### Create UI Panels
1. Create empty GameObjects as children of Canvas:
   - MainMenuPanel
   - GamePanel
   - GameOverPanel
   - PausePanel

#### Create UI Elements
1. In GamePanel, add:
   - Score Text (Top-left)
   - High Score Text (Top-right)
   - Speed Text (Top-center)

2. In GameOverPanel, add:
   - Final Score Text
   - New High Score Text
   - Restart Button
   - Main Menu Button

3. In MainMenuPanel, add:
   - Title Text
   - Start Button
   - Settings Button

### 5. Materials and Effects

#### Create Neon Materials
1. Create new materials in Materials folder
2. Set Shader to "Standard"
3. Enable "Emission"
4. Set Emission Color to neon colors (cyan, magenta, yellow, etc.)
5. Set Emission Intensity to 2-3

#### Create Particle Effects
1. Create Particle Systems for:
   - Player trail effect
   - Background particles
   - Collection effects
   - Obstacle destruction effects

### 6. Prefabs

#### Create Obstacle Prefabs
1. Create basic shapes (cubes, spheres) as obstacles
2. Add the `Obstacle` script
3. Add colliders (set as triggers)
4. Apply neon materials
5. Create prefabs

#### Create Collectible Prefabs
1. Create collectible objects (crystals, coins)
2. Add the `Collectible` script
3. Add colliders (set as triggers)
4. Apply neon materials
5. Create prefabs

### 7. Lighting Setup

#### Neon Lights
1. Add Point Lights around the scene
2. Set colors to neon colors
3. Adjust intensity and range
4. Enable shadows if desired

#### Global Lighting
1. Set Ambient Light to dark blue/black
2. Add fog for atmosphere
3. Set post-processing for bloom effect

### 8. Audio Setup

#### Audio Sources
1. Add AudioSource to Player for jump sounds
2. Add AudioSource to GameManager for background music
3. Add AudioSource to collectibles for collection sounds

#### Audio Clips
1. Import or create audio files for:
   - Jump sound
   - Collection sound
   - Background music
   - Ambient sounds

## Configuration

### PlayerController Settings
- Move Speed: 10
- Jump Force: 15
- Gravity: -30
- Lane Distance: 3

### GameManager Settings
- Game Speed: 10
- Speed Increase Rate: 0.1
- Max Speed: 25

### ObstacleGenerator Settings
- Spawn Distance: 50
- Min Spawn Interval: 2
- Max Spawn Interval: 5
- Lane Width: 3

## Testing

1. Press Play in Unity
2. Test player movement (WASD/Arrow Keys)
3. Test jumping (Space)
4. Test obstacle spawning
5. Test collectible collection
6. Test game over condition

## Troubleshooting

### Common Issues

1. **Player not moving**: Check if PlayerController script is attached and CharacterController component is present
2. **Camera not following**: Ensure CameraFollow script is attached and target is set
3. **Obstacles not spawning**: Check ObstacleGenerator script and prefab assignments
4. **Neon effects not working**: Ensure materials have emission enabled and neon scripts are attached
5. **UI not updating**: Check if UI elements are properly assigned in UIManager

### Performance Tips

1. Use object pooling for obstacles and collectibles
2. Limit particle effects
3. Optimize neon lighting
4. Use LOD for distant objects

## Next Steps

1. Add more obstacle types
2. Implement power-ups
3. Add sound effects
4. Create multiple levels
5. Add particle effects
6. Implement save system
7. Add mobile controls
8. Create build settings

## Credits

This project was created as a 3D endless runner with neon effects. Feel free to modify and extend the functionality!
