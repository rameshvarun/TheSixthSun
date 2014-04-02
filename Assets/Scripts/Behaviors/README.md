# Behaviors
This folder consists of script components meant to be attached to Unity objects.

## SceneBuilder
This script is vital, as it constructs the Unity scene from the `GameState` singleton instance. This involves iterating through tiles, planets, stars, and units.

## CameraControls
This currently just handles the panning/rotating of the camera. It has yet to be determined how the actual GUI for commanding units will be decomposed.