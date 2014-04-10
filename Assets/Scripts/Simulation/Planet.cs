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

	public Planet(HexCoord coordinate, string name, string description, int subdivisions) {
		this.coordinate = coordinate;
		this.name = name;
		this.description = description;
		this.subdivisions = subdivisions;

		//Generate array for storing terrain
		Dictionary<int, HashSet<int>> graph = HexPlanet.createOnlyPlanetGraph(subdivisions);
		terrain = new int[graph.Count];

		//Randomize terrain
		for(int i = 0; i < terrain.Length; ++i) {
			terrain[i] = Random.Range(0, 6);
		}

	}
}
