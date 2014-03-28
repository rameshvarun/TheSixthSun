using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Planet : ISpaceObject {
	/// <summary>The location of this planet in space, stored as a HexCoord</summary> 
	public HexCoord coordinate { get; set; }

	/// <summary>List of objects that currently inhabit the surface of the planet</summary> 
	public List<ILandObject> landObjects;

	public string name;
	public string description;

	/// <summary>The number of subdivisions required to get a sphere of the correct resolution</summary> 
	public int subdivisions;

	public Planet() {
	}
}
