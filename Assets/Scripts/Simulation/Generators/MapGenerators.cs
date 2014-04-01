using UnityEngine;
using System.Collections;

public static class MapGenerators {
	public static void BasicMap(GameState gameState) {
		int radius = 10;
		for(int x = -radius; x < radius; ++x) {
			for(int y = -radius; y < radius; ++y) {
				for(int z = -radius; z < radius; ++z) {
					if(x + y + z == 0)
						gameState.grid.tiles[new HexCoord(x, y, z)] = 0;
				}
			}
		}

		//Single test planet
		Planet testPlanet = new Planet(new HexCoord(-2, -2), "Test Planet", "Placeholder description", 2);
		gameState.planets.Add(testPlanet);

		//Put one star at the center of the map
		Star star = new Star(new HexCoord(0,0), "General Star", "Placeholder description.");
		gameState.stars.Add(star);
	}
}
