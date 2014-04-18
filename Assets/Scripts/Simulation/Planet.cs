using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : ISpaceObject {
	/// <summary>The location of this planet in space, stored as a HexCoord</summary> 
	public HexCoord coordinate { get; set; }

	/// <summary>List of objects that currently inhabit the surface of the planet</summary> 
	public List<ILandObject> landObjects = new List<ILandObject>();

	/// <summary>Keep track of the terrain data</summary> 
	public int[] terrain;

	public string name;
	public string description;

	/// <summary>The number of subdivisions required to get a sphere of the correct resolution</summary> 
	public int subdivisions;

	public static int WATER = 0;
	public static int DESERT = 1;
	public static int TUNDRA = 2;
	public static int FOREST = 3;
	public static int GRASSLAND = 4;
	public static int MOUNTAIN = 5;

	//Links to Unity MonoBehaviors. These variables should not be serialized
	public HexPlanet hexPlanet;
	public PlanetBehavior behavior;

	public Planet(HexCoord coordinate, string name, string description, int subdivisions) {
		this.coordinate = coordinate;
		this.name = name;
		this.description = description;
		this.subdivisions = subdivisions;

		//Generate array for storing terrain
		Dictionary<int, HashSet<int>> graph = HexPlanet.createOnlyPlanetGraph(subdivisions);
		terrain = new int[graph.Count];

		//Initialize
		for(int i = 0; i < terrain.Length; ++i) {
			terrain[i] = WATER;
		}

		//Seed some land
		for(int i = 0; i < 3; ++i) {
			terrain[Random.Range(0, terrain.Length)] = GRASSLAND;
		}

		//Evolve land
		for(int i = 0; i < 3; ++i) {
			int[] new_terrain = new int[terrain.Length];
			System.Array.Copy(terrain, newterrain, terrain.Length);

			for(int j = 0; j < terrain.Length; ++j) {
				if(terrain[j] == GRASSLAND) {
					foreach(int neighbor in graph[j]) {
						if(Random.value > 0.1) {
							newterrain[neighbor] = GRASSLAND;
						}
					}
				}
			}

			terrain = new_terrain;
		}


		//
		//terrain[0] = 2;
		//terrain[1] = 2;

	}
}
