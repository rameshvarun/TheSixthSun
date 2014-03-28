using UnityEngine;
using System.Collections;

public static class MapGenerators {
	public static void TestMap(GameState gameState) {
		int radius = 5;
		for(int x = -radius; x < radius; ++x) {
			for(int y = -radius; y < radius; ++y) {
				for(int z = -radius; z < radius; ++z) {
					if(x + y + z == 0)
						gameState.grid.tiles[new HexCoord(x, y, z)] = 0;
				}
			}
		}

		Planet testPlanet = new Planet();
		testPlanet.subdivisions = 2;
		testPlanet.coordinate = new HexCoord(-2, -2);
		gameState.planets.Add(testPlanet);
	}
}
