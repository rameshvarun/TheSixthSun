using UnityEngine;
using System.Collections;

public static class MapGenerators {
	public static void BasicMap(GameState gameState) {
		//Standard map for player counts of 3 or less
		if(gameState.players.Count <= 3) {
			int radius = 10;
			for(int x = -radius; x < radius; ++x) {
				for(int y = -radius; y < radius; ++y) {
					for(int z = -radius; z < radius; ++z) {
						if(x + y + z == 0)
							gameState.grid.tiles[new HexCoord(x, y, z)] = 0;
					}
				}
			}

			//Randomly distribute asteroids in circleish around the sun

			//Single test planet
			Planet testPlanet = new Planet(new HexCoord(-2, -2), "Test Planet", "Placeholder description", 2);
			gameState.planets.Add(testPlanet);

			//Add a colonist to the test planet
			testPlanet.landObjects.Add(new GroundUnit(0, GroundUnit.Colonist, null));

			//Add test space units
			gameState.spaceUnits.Add(new Spaceship(new HexCoord(-1, -1), Spaceship.TestShip, null));

			//Put one star at the center of the map
			Star star = new Star(new HexCoord(0,0), "General Star", "Placeholder description.");
			gameState.stars.Add(star);

			return;
		}

		Debug.LogError("No generator exists for this map size.");
	}
}
