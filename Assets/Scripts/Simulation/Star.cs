using UnityEngine;
using System.Collections;

public class Star : ISpaceObject
{
	/// <summary>The location of the star in space.</summary> 
	public HexCoord coordinate { get; set; }

	public string name;
	public string description;

	public Star(HexCoord coordinate, string name, string description) {
		this.coordinate = coordinate;
		this.name = name;
		this.description = description;
	}
}

