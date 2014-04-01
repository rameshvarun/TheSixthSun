# Scripts Folder

The core of the game is split among the `Simulation` and `Behavior` folders.

## Behaviors
This folder contains scripts meant to be attached as components to Unity Game Objects. Ideally, every behavior script would contain a link back to the Simulation object that they represent.

## Simulation
Classes that actually control the simulation, as well as provide a container that stores all of the data for the game.

## HexPlanet
Because there are a significant amount of files involved in the management of the hex-tiled planets, it has been split into its own directory.

## Libraries
External libraries used by the program. Currently includes:
- LitJSON - Balance data will be stored as .json files, and read through this parser.