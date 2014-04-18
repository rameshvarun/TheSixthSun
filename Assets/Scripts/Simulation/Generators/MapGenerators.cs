using UnityEngine;
using System.Collections;

public static class MapGenerators {

	/// <summary>
	/// Generate a standard map - supports 2 to 3 players.
	/// </summary>
	/// <param name="gameState">The GameState object to be populated with a playable match.</param>
	public static void BasicMap(GameState gameState) {
		//Standard map for player counts of 3 or less
		if(gameState.players.Count <= 3) {
			int radius = 11;
			for(int x = -radius; x < radius; ++x) {
				for(int y = -radius; y < radius; ++y) {
					for(int z = -radius; z < radius; ++z) {
						if(x + y + z == 0)
							gameState.grid.tiles[new HexCoord(x, y, z)] = HexGrid.SpaceTile;
					}
				}
			}

			//TODO: Randomly distribute asteroid tiles in circleish around the sun
			int planet_radius = radius - 3;

			//Create planets in symmetrical locations
			const int start_planet_subdiv = 2;
			gameState.planets.Add(new Planet(new HexCoord(-planet_radius, 0), "Test Planet", "Placeholder description", start_planet_subdiv));
			gameState.planets.Add(new Planet(new HexCoord(planet_radius, 0), "Test Planet", "Placeholder description", start_planet_subdiv));

			gameState.planets.Add(new Planet(new HexCoord(0, -planet_radius), "Test Planet", "Placeholder description", start_planet_subdiv));
			gameState.planets.Add(new Planet(new HexCoord(0, planet_radius), "Test Planet", "Placeholder description", start_planet_subdiv));

			gameState.planets.Add(new Planet(new HexCoord(planet_radius, -planet_radius), "Test Planet", "Placeholder description", start_planet_subdiv));
			gameState.planets.Add(new Planet(new HexCoord(-planet_radius, planet_radius), "Test Planet", "Placeholder description", start_planet_subdiv));

			//Seed colonist units
			if(gameState.players.Count == 2) {
				gameState.planets[4].landObjects.Add(new GroundUnit(0, GroundUnit.Colonist, gameState.players[0], gameState.planets[4]));
				gameState.planets[5].landObjects.Add(new GroundUnit(0, GroundUnit.Colonist, gameState.players[1], gameState.planets[5]));
			}
			if(gameState.players.Count == 3) {
				gameState.planets[2].landObjects.Add(new GroundUnit(0, GroundUnit.Colonist, gameState.players[0], gameState.planets[2]));
				gameState.planets[5].landObjects.Add(new GroundUnit(0, GroundUnit.Colonist, gameState.players[1], gameState.planets[5]));
				gameState.planets[1].landObjects.Add(new GroundUnit(0, GroundUnit.Colonist, gameState.players[2], gameState.planets[1]));
			}

			/*//Add a colonist to the test planet
			testPlanet.landObjects.Add(new GroundUnit(0, GroundUnit.Colonist, null));

			//Add test space units
			gameState.spaceUnits.Add(new Spaceship(new HexCoord(-1, -1), Spaceship.TestShip, null));
			*/
			
			//Put one star at the center of the map
			gameState.stars.Add(new Star(new HexCoord(0,0), "General Star", "Placeholder description."));

			return;
		}

		Debug.LogError("No generator exists for this map size.");
	}
}
